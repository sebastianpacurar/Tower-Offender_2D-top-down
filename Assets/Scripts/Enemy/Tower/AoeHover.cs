using System.Collections;
using Enemy.Tower.Hp;
using ScriptableObjects;
using UnityEngine;

namespace Enemy.Tower {
    public class AoeHover : MonoBehaviour {
        [SerializeField] private TankStatsSo tankStatsSo;
        [SerializeField] private SpriteRenderer turret;
        [SerializeField] private GameObject hoverIconsContainer;

        [Header("Container Sine Data")]
        [SerializeField] private float amp;

        [SerializeField] private float freq;


        [Header("Nuke Icon Rotation Data")]
        [SerializeField] private float rotationSpeed;

        private SpriteRenderer _arrowSr, _empLightningColor;

        private GameObject _arrow, _empIcon, _nukeIcon;
        private TurretHpManager _turretHpManager;
        private Vector3 _containerInitialPos;
        private bool _isHovered;
        private float _nukeIconRotZ;
        private Coroutine moveHoverIcons;

        private void Awake() {
            _turretHpManager = GetComponent<TurretHpManager>();
            _arrowSr = hoverIconsContainer.transform.Find("HoverArrow").GetComponent<SpriteRenderer>();
            _empIcon = hoverIconsContainer.transform.Find("EmpIcon").gameObject;
            _empLightningColor = _empIcon.transform.Find("LightningIcon").GetComponent<SpriteRenderer>();
            _nukeIcon = hoverIconsContainer.transform.Find("NukeIcon").gameObject;
        }

        private void Start() {
            _containerInitialPos = hoverIconsContainer.transform.position;
            moveHoverIcons = StartCoroutine(SwapEmpLightningIcon());
        }

        private void Update() {
            if (!hoverIconsContainer) return;

            if (_turretHpManager.IsDead) {
                StopCoroutine(moveHoverIcons);
                Destroy(hoverIconsContainer);
                return;
            }
            HandleTowerHoverTransparency();
            HoverIconsSineMovement();
            RotateNukeIcon();
        }

        private void OnTriggerStay2D(Collider2D col) {
            if (_turretHpManager.IsDead) return;
            if (CompareTag("TurretObj")) {
                if (col.gameObject.CompareTag("EmpAoeGhost")) {
                    _empIcon.SetActive(true);
                    _arrowSr.enabled = true;
                    _arrowSr.color = tankStatsSo.EmpShellStatsSo.HoverArrowColor;
                    _isHovered = true;
                }

                if (col.gameObject.CompareTag("NukeAoeGhost")) {
                    _nukeIcon.SetActive(true);
                    _arrowSr.enabled = true;
                    _arrowSr.color = tankStatsSo.NukeShellStatsSo.HoverArrowColor;
                    _isHovered = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D col) {
            if (_turretHpManager.IsDead) return;
            if (col.gameObject.CompareTag("EmpAoeGhost") || col.gameObject.CompareTag("NukeAoeGhost")) {
                _empIcon.SetActive(false);
                _nukeIcon.SetActive(false);
                _arrowSr.enabled = false;
                _isHovered = false;
            }
        }

        private void HoverIconsSineMovement() {
            hoverIconsContainer.transform.position = new Vector3(_containerInitialPos.x, Mathf.Sin(Time.time * freq) * amp + _containerInitialPos.y, 0);
        }

        private void RotateNukeIcon() {
            _nukeIconRotZ += Time.deltaTime * rotationSpeed;
            _nukeIcon.transform.rotation = Quaternion.Euler(0, 0, _nukeIconRotZ);
        }

        private IEnumerator SwapEmpLightningIcon() {
            while (true) {
                yield return new WaitForSeconds(0.5f);
                _empLightningColor.color = _empLightningColor.color.Equals(Color.white) ? Color.red : Color.white;
            }
        }

        private void HandleTowerHoverTransparency() {
            if (_isHovered) {
                // body.color = Color.Lerp(new Color(0.75f, 0.75f, 0.75f, 0.75f), new Color(0.25f, 0.25f, 0.25f, 0.25f), Mathf.PingPong(Time.time, 0.5f));
                if (!_turretHpManager.IsDead) {
                    turret.color = Color.Lerp(new Color(0.75f, 0.75f, 0.75f, 0.75f), new Color(0.25f, 0.25f, 0.25f, 0.25f), Mathf.PingPong(Time.time, 0.5f));
                }
            } else {
                // body.color = new Color(1f, 1f, 1f, 1f);
                if (!_turretHpManager.IsDead) {
                    turret.color = new Color(1f, 1f, 1f, 1f);
                }
            }
        }
    }
}