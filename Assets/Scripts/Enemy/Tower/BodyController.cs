using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemy.Tower.Hp;
using Menus;
using TileMap;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

namespace Enemy.Tower {
    public class BodyController : MonoBehaviour {
        [SerializeField] private ParticleSystem center, fireWave, shockWave;
        private ParticleSystem.EmissionModule _centerEm, _fireWaveEm, _shockWaveEm;

        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Transform fadingCashTransform;
        [SerializeField] private TextMeshProUGUI fadingCashValue;
        private Camera _mainCam;

        [SerializeField] private Sprite[] lightSprites;
        [SerializeField] private Sprite[] midSprites;
        [SerializeField] private Sprite[] heavySprites;
        private Sprite[] _spriteSet;

        [SerializeField] private GameObject[] turretPrefabs;
        private float _timeToSpawnNextTurret;
        private int _currTurretIndex;
        private bool _turretUpdateInProgress;
        private float _explosionTimer;
        private float _explosionIntensity;
        private bool _isTurretInList;

        private Dictionary<string, TileBase> _wallPoints;
        private Tilemap _wallMap;
        private WallTileManager _wallMapManager;
        private LevelCompleteMenu _levelCompleteMenu;

        // the box which results after the explosion occurs when there are no more turrets in list
        private BoxCollider2D _boxCollider2D;
        // circle which increases in range during explosion
        private CircleCollider2D _circleCollider2D;
        private Light2D _light2D;

        private SpriteRenderer _sr;
        private Transform _turretsTilemapObj;
        private GameObject _turretEntity;
        private TurretHpManager _turretHpManager;

        // this refers to the type of tile used, in case there are walls nearby
        private int _spriteIndex;

        // used by fade object as addition to position.y. its value is Time.deltaTime * 2
        private float timer;

        private void Awake() {
            _wallMapManager = FindObjectOfType<WallTileManager>();

            _centerEm = center.emission;
            _fireWaveEm = fireWave.emission;
            _shockWaveEm = shockWave.emission;

            _wallPoints = new Dictionary<string, TileBase>();
            _sr = GetComponent<SpriteRenderer>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _circleCollider2D = GetComponent<CircleCollider2D>();
            _light2D = GetComponent<Light2D>();

            _wallMap = GameObject.FindGameObjectWithTag("Wall").GetComponent<Tilemap>();
            _turretsTilemapObj = GameObject.FindGameObjectWithTag("TowerTurretsTilemap").transform;
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _levelCompleteMenu = GameObject.FindGameObjectWithTag("Menu").GetComponent<LevelCompleteMenu>();

            name = $"{name[..2]}-{transform.position.ToString()}"; // set the name of the body to its position in world space
        }

        private void Start() {
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = _mainCam;

            _timeToSpawnNextTurret = 2f; // default to 2 seconds
            _isTurretInList = true;

            // instantiate based on the prefab used
            if (name.StartsWith("LB")) {
                _currTurretIndex = 0;
            } else if (name.StartsWith("MB")) {
                _currTurretIndex = 3;
            } else if (name.StartsWith("HB")) {
                _currTurretIndex = 6;
            }

            InstantiateTurret(_currTurretIndex);
        }

        private void Update() {
            PerformWallPositionCalculations();
            HandleTurretUpgrade();
            HandleTurretExplosionCollider();
            DisplayEarnedCash();
        }

        // when the turret is updating, trigger fadingCash to fade out and move upwards
        private void DisplayEarnedCash() {
            if (_turretUpdateInProgress) {
                fadingCashValue.gameObject.SetActive(true);

                var position = transform.position;
                timer += Time.deltaTime * 2;
                fadingCashTransform.position = new Vector3(position.x, position.y + timer, 1);

                // needed due to the rotation of the body to match the connecting walls. Set always to 0 rotation 
                fadingCashTransform.rotation = Quaternion.Euler(0f, 0f, 0f);

                if (canvasGroup.alpha > 0) {
                    canvasGroup.alpha -= Time.deltaTime / 2f;
                } else {
                    timer = 0f;
                }
            }
        }

        // hide the cash text, set the position to the body's position, and reset alpha to 1
        private void RestartDisplayEarnedCash() {
            fadingCashValue.gameObject.SetActive(false);
            fadingCashTransform.position = transform.position;
            canvasGroup.alpha = 1f;
        }


        // destroy the current turret var, nd re-instantiate it to the new turret obj
        private IEnumerator UpdateTurret() {
            while (true) {
                yield return new WaitForSeconds(_timeToSpawnNextTurret);
                Destroy(_turretEntity);
                // increase the index 
                // as long as the current index can match any item from the list, return true
                _currTurretIndex++;
                _isTurretInList = _currTurretIndex < turretPrefabs.Length;


                if (_isTurretInList) {
                    // instantiate if the current index matches entry in list
                    InstantiateTurret(_currTurretIndex);
                } else {
                    // otherwise enable the box-collider as replacement for the last destroyed turret  
                    _boxCollider2D.enabled = true;
                    _levelCompleteMenu.towersLeft--;
                }

                // set the update to false to prevent immediate re-trigger from Update()
                _turretUpdateInProgress = false;

                // disable PS emission
                _centerEm.enabled = false;
                _fireWaveEm.enabled = false;
                _shockWaveEm.enabled = false;

                StopCoroutine(nameof(UpdateTurret));
            }
        }


        private void InstantiateTurret(int prefabIndex) {
            _turretEntity = Instantiate(turretPrefabs[prefabIndex], transform.position, Quaternion.identity, _turretsTilemapObj);
            var turretObj = _turretEntity.transform.Find("TurretObj");
            _turretHpManager = turretObj.GetComponent<TurretHpManager>();

            var turretData = turretObj.Find("Turret").GetComponent<TurretController>();

            // update the color of the sprite set used. if LT then yellow; else if MT then red; else if HT then purple
            // update the explosion light color, and fading CashValue text
            if (_turretEntity.name.Contains("LT")) {
                _spriteSet = lightSprites;
                _light2D.color = Color.yellow;
                fadingCashValue.color = Color.yellow;
            } else if (_turretEntity.name.Contains("MT")) {
                _spriteSet = midSprites;
                _light2D.color = Color.red;
                fadingCashValue.color = Color.red;
            } else if (_turretEntity.name.Contains("HT")) {
                _spriteSet = heavySprites;
                _light2D.color = Color.magenta;
                fadingCashValue.color = Color.magenta;
            }

            // set cash value text to its initial setup
            RestartDisplayEarnedCash();

            // set the cash value based on the following turret's stats
            fadingCashValue.text = $"${turretData.turretStatsSo.CashValue}";

            // Generate the Fort when the new turret is spawned
            _wallMapManager.GenerateFort(_turretEntity);
        }


        private void HandleTurretUpgrade() {
            // if there is no turretEntity attached, it means that the last prefab has been processed and destroyed (1HT - purple)
            if (_turretEntity) {
                if (_isTurretInList) {
                    // if the turret is dead set the update progress to true, and begin updating the turret
                    if (_turretHpManager.IsDead && !_turretUpdateInProgress) {
                        _centerEm.enabled = true;
                        center.Play();

                        _fireWaveEm.enabled = true;
                        fireWave.Play();

                        _shockWaveEm.enabled = true;
                        shockWave.Play();

                        StartCoroutine(nameof(UpdateTurret));
                        _turretUpdateInProgress = true;
                    }
                }
            }
        }

        // creates a small light along with a circle collider to push the tank away from the surrounding tiles
        // avoid getting caught in the gaps formed between the surrounding walls and newly spawned turret
        private void HandleTurretExplosionCollider() {
            if (_turretUpdateInProgress) {
                _explosionTimer += Time.deltaTime;
            } else {
                _explosionTimer = 0f;
            }

            // set intensity to default to 2, and when the explosion is half way through, start decreasing intensity to 0 twice the speed
            if (_explosionTimer > _timeToSpawnNextTurret / 2) {
                _explosionIntensity -= Time.deltaTime * 2;
            } else {
                _explosionIntensity = 2f;
            }

            _circleCollider2D.radius = _explosionTimer; // update the collider radius to push the tank gradually off the spawnable wall tiles
            _light2D.pointLightOuterRadius = Mathf.Clamp(_explosionTimer, 0, _timeToSpawnNextTurret);
            _light2D.pointLightInnerRadius = Mathf.Clamp(_explosionTimer / 2, 0, _timeToSpawnNextTurret / 2);
            _light2D.intensity = Mathf.Clamp(_explosionIntensity, 0, 2);
        }


        private void PerformWallPositionCalculations() {
            UpdateWallTilesPositions();
            CalculateIndexAndSpriteRotation(_spriteSet);
        }


        private void UpdateWallTilesPositions() {
            var selfPos = new Vector2(transform.position.x, transform.position.y);
            var gridPosition = _wallMap.WorldToCell(selfPos);
            _wallPoints[Cardinal.N.ToString()] = _wallMap.GetTile(gridPosition + Vector3Int.up);
            _wallPoints[Cardinal.S.ToString()] = _wallMap.GetTile(gridPosition + Vector3Int.down);
            _wallPoints[Cardinal.E.ToString()] = _wallMap.GetTile(gridPosition + Vector3Int.right);
            _wallPoints[Cardinal.W.ToString()] = _wallMap.GetTile(gridPosition + Vector3Int.left);
        }


        // calculate how many sides should have edges, and then rotate the sprite to connect the corresponding walls
        private void CalculateIndexAndSpriteRotation(Sprite[] currentSpriteSet) {
            var north = _wallPoints[Cardinal.N.ToString()];
            var south = _wallPoints[Cardinal.S.ToString()];
            var east = _wallPoints[Cardinal.E.ToString()];
            var west = _wallPoints[Cardinal.W.ToString()];

            // evaluate based on how many keys have non-null values
            switch (_wallPoints.Count(entry => entry.Value)) {
                case 0: // no surrounding walls
                    _sr.sprite = currentSpriteSet[0];
                    break;

                case 1: // handle 1 wall connection
                    _sr.sprite = currentSpriteSet[1];
                    string point = null;

                    // retrieve the only key-value pair from the dictionary with valid tile
                    foreach (var key in _wallPoints.Keys) {
                        if (_wallPoints[key]) {
                            point = key;
                            break;
                        }
                    }

                    transform.rotation = point switch {
                        "N" => Quaternion.Euler(0f, 0f, 90f),
                        "W" => Quaternion.Euler(0f, 0f, 180f),
                        "S" => Quaternion.Euler(0f, 0f, 270f),
                        _ => Quaternion.identity
                    };
                    break;

                case 2: // handle 2 wall connection
                    // for 180 degrees based wall edges (like N-S and W-E)
                    if ((north && south) || (east && west)) {
                        _sr.sprite = currentSpriteSet[2];

                        if (north && south) {
                            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                        }
                    } else {
                        // for 90 degrees based wall edges (like N-W or N-E or E-S)
                        _sr.sprite = currentSpriteSet[3];

                        if (east && south) {
                            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                        } else if (north && east) {
                            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                        } else if (north && west) {
                            transform.rotation = Quaternion.Euler(0f, 0f, 270f);
                        } else if (south && west) {
                            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                        }
                    }

                    break;
                case 3: // handle 3 wall connection
                    _sr.sprite = currentSpriteSet[4];

                    if (north && east && south) {
                        transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                    } else if (west && north && east) {
                        transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                    } else if (south && west && north) {
                        transform.rotation = Quaternion.Euler(0f, 0f, 270f);
                    } else if (south && west && east) {
                        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    }

                    break;

                case 4: // handle 4 wall connection
                    _sr.sprite = currentSpriteSet[5];
                    break;
            }
        }

        private enum Cardinal {
            N,
            S,
            E,
            W,
        }
    }
}