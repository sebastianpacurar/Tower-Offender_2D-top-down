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

        private Dictionary<string, TileBase> _wallPoints;
        private Tilemap _wallMap;

        private SpriteRenderer _sr;
        private GameObject _towerTurrets;
        private GameObject _turret;
        private TurretHpManager _turretHpManager;

        // this refers to the type of tile used, in case there are walls nearby
        private int _spriteIndex;

        private void Awake() {
            _wallPoints = new Dictionary<string, TileBase>();
            _sr = GetComponent<SpriteRenderer>();
        }

        private void Start() {
            _wallMap = GameObject.FindGameObjectWithTag("Wall").GetComponent<Tilemap>();
            _towerTurrets = GameObject.FindGameObjectWithTag("TowerTurretsTilemap").gameObject;
            // _turret = _towerTurrets.transform.Find($"1LightTurret - {transform.position}").gameObject;
            // _turretHpManager = _turret.transform.Find("TurretObj").GetComponent<TurretHpManager>();
        }

        // Update is called once per frame
        private void Update() {
            UpdateWallTilesPositions();
            CalculateIndexAndSpriteRotation(lightSprites);
        }


        // 
        private void CalculateIndexAndSpriteRotation(Sprite[] currentSpriteSet) {
            var north = _wallPoints[Cardinal.North.ToString()];
            var south = _wallPoints[Cardinal.South.ToString()];
            var east = _wallPoints[Cardinal.East.ToString()];
            var west = _wallPoints[Cardinal.West.ToString()];

            switch (_wallPoints.Count(entry => entry.Value)) {
                case 0:
                    _sr.sprite = currentSpriteSet[0];
                    break;
                case 1:
                    _sr.sprite = currentSpriteSet[1];

                    string point = null;

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

                case 2:
                    _sr.sprite = currentSpriteSet[2];


                    if ((north && south) || (east && west)) {
                        _sr.sprite = currentSpriteSet[2];

                        if (north && south) {
                            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                        }
                    } else {
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
                case 3:
                    //TODO: add logic for 3 and 4 points;
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