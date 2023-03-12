using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemy.Tower.Hp;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Enemy.Tower {
    public class BodyController : MonoBehaviour {
        [SerializeField] private Sprite[] lightSprites;
        [SerializeField] private Sprite[] midSprites;
        [SerializeField] private Sprite[] heavySprites;
        private Sprite[] _spriteSet;

        [SerializeField] private GameObject[] turretPrefabs;
        private float _timeToSpawnNextTurret;
        private int _currTurretIndex;
        private bool _turretUpdateInProgress;
        private bool _isTurretInList;

        private Dictionary<string, TileBase> _wallPoints;
        private Tilemap _wallMap;

        private SpriteRenderer _sr;
        private BoxCollider2D _boxCollider2D;
        private Transform _turretsTilemapObj;
        private GameObject _turretEntity;
        private TurretHpManager _turretHpManager;

        // this refers to the type of tile used, in case there are walls nearby
        private int _spriteIndex;

        private void Awake() {
            _wallPoints = new Dictionary<string, TileBase>();
            _sr = GetComponent<SpriteRenderer>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _wallMap = GameObject.FindGameObjectWithTag("Wall").GetComponent<Tilemap>();
            _turretsTilemapObj = GameObject.FindGameObjectWithTag("TowerTurretsTilemap").transform;
            name = transform.position.ToString(); // set the name of the body to its position in world space
        }

        private void Start() {
            _timeToSpawnNextTurret = 2f; // default to 2 seconds

            // instantiate 1LT as first turret, and set index checker to true
            _isTurretInList = true;
            InstantiateTurret(_currTurretIndex);
        }

        private void Update() {
            UpdateWallTilesPositions();
            CalculateIndexAndSpriteRotation(_spriteSet);
            HandleTurretUpgrade();
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
                }

                // set the update to false to prevent immediate re-trigger from Update()
                _turretUpdateInProgress = false;
                StopCoroutine(nameof(UpdateTurret));
            }
        }


        private void InstantiateTurret(int prefabIndex) {
            _turretEntity = Instantiate(turretPrefabs[prefabIndex], transform.position, Quaternion.identity, _turretsTilemapObj);
            _turretHpManager = _turretEntity.transform.Find("TurretObj").GetComponent<TurretHpManager>();

            // update the color of the sprite set used. if LT then yellow; else if MT then red; else if HT then purple
            if (_turretEntity.name.Contains("LT")) {
                _spriteSet = lightSprites;
            } else if (_turretEntity.name.Contains("MT")) {
                _spriteSet = midSprites;
            } else if (_turretEntity.name.Contains("HT")) {
                _spriteSet = heavySprites;
            }
        }


        private void HandleTurretUpgrade() {
            // if there is no turretEntity attached, it means that the last prefab has been processed and destroyed (1HT - purple)
            if (_turretEntity) {
                if (_isTurretInList) {
                    // if the turret is dead set the update progress to true, and begin updating the turret
                    if (_turretHpManager.IsDead && !_turretUpdateInProgress) {
                        StartCoroutine(nameof(UpdateTurret));
                        _turretUpdateInProgress = true;
                    }
                }
            }
        }


        // calculate how many sides should have edges, and then rotate the sprite to connect the corresponding walls
        private void CalculateIndexAndSpriteRotation(Sprite[] currentSpriteSet) {
            var north = _wallPoints[Cardinal.North.ToString()];
            var south = _wallPoints[Cardinal.South.ToString()];
            var east = _wallPoints[Cardinal.East.ToString()];
            var west = _wallPoints[Cardinal.West.ToString()];

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
                        "North" => Quaternion.Euler(0f, 0f, 90f),
                        "West" => Quaternion.Euler(0f, 0f, 180f),
                        "South" => Quaternion.Euler(0f, 0f, 270f),
                        _ => Quaternion.identity
                    };
                    break;

                case 2: // handle 2 wall connection
                    // for 180 degrees based wall edges (like North-South and West-East
                    if ((north && south) || (east && west)) {
                        _sr.sprite = currentSpriteSet[2];

                        if (north && south) {
                            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                        }
                    } else {
                        // for 90 degrees based wall edges (like North-West or North-East or East-South)
                        _sr.sprite = currentSpriteSet[3];

                        if (east && south) {
                            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                        } else if (north && east) {
                            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                        } else if (north && west) {
                            transform.rotation = Quaternion.Euler(0f, 0f, 270f);
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
                    }

                    break;

                case 4: // hand;e 4 wall connection
                    _sr.sprite = currentSpriteSet[5];
                    break;
            }
        }

        private void UpdateWallTilesPositions() {
            var selfPos = new Vector2(transform.position.x, transform.position.y);
            var gridPosition = _wallMap.WorldToCell(selfPos);
            _wallPoints[Cardinal.North.ToString()] = _wallMap.GetTile(gridPosition + Vector3Int.up);
            _wallPoints[Cardinal.South.ToString()] = _wallMap.GetTile(gridPosition + Vector3Int.down);
            _wallPoints[Cardinal.East.ToString()] = _wallMap.GetTile(gridPosition + Vector3Int.right);
            _wallPoints[Cardinal.West.ToString()] = _wallMap.GetTile(gridPosition + Vector3Int.left);
        }


        private enum Cardinal {
            North,
            South,
            East,
            West,
        }
    }
}