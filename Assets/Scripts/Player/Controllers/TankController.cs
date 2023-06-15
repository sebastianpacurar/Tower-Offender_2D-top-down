using System;
using System.Collections;
using Cinemachine;
using ScriptableObjects;
using TileMap;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Player.Controllers {
    public class TankController : MonoBehaviour {
        public float SpeedBoostVal { get; private set; }

        private PlayerControls _controls;
        private CinemachineVirtualCamera _cineMachineMainCam;
        private RoadTileManager _roadTileManager;
        private Rigidbody2D _rb;

        private float _move, _rotation;
        private float _rotationAngle = 0f;
        private bool _isSpeeding;

        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

        [SerializeField] private TankStatsSo tankStatsSo;

        [Header("Physics related")]
        [SerializeField] private float accFactor;
        [SerializeField] private float steerFactor;
        [SerializeField] private float driftFactor;
        [SerializeField] private float maxSpeed;
        [SerializeField] private Vector2 engineForce;
        [SerializeField] private float opposingForce; // if negative then apply breaks

        [Header("Animators")]
        [SerializeField] private Animator[] tankTracks;

        [Space(20)]
        [SerializeField] private GameObject exhaustBack;
        [SerializeField] private GameObject exhaustFront;

        [Header("For Debugging Purposes")]
        [SerializeField] private GameObject targetDir;
        [SerializeField] private float rotDur;
        [SerializeField] private float rotDurInAtan2;
        [SerializeField] private float rigidBodyDrag;
        [SerializeField] private Vector2 upVelocity;
        [SerializeField] private Vector2 rightVelocity;
        [SerializeField] private Vector2 localVelocity;
        [SerializeField] private Vector2 tankTargetAlignment;


        private void Awake() {
            _controls = new PlayerControls();
            _rb = GetComponent<Rigidbody2D>();
            _cineMachineMainCam = GameObject.FindGameObjectWithTag("CM2D").GetComponent<CinemachineVirtualCamera>();
            _roadTileManager = FindObjectOfType<RoadTileManager>();

            steerFactor = tankStatsSo.SteerFactor;
            driftFactor = tankStatsSo.DriftFactor;
            SpeedBoostVal = tankStatsSo.SpeedBoostCapacity;
        }

        private void Start() {
            _cineMachineMainCam.Follow = transform;
        }

        private void Update() => Debugger();

        private void FixedUpdate() {
            ApplyEngineForce();
            KillOrthogonalVelocity();
            ApplySteering();
            StopTankWhenNoSpeedBoost();
        }

        // prevent the tank from using Speed Boost when 0% capacity left
        // also prevents the bar to get filled while shift is being held down
        private void StopTankWhenNoSpeedBoost() {
            if (SpeedBoostVal < 1f) {
                _isSpeeding = false;
            }
        }

        // decrease the SpeedBoostVal gradually when Shift is pressed
        private IEnumerator HandleSpeedBoostNegativeValue() {
            while (true) {
                yield return new WaitForSeconds(0.005f);
                SpeedBoostVal = SpeedBoostVal switch {
                    > 0 => SpeedBoostVal -= tankStatsSo.SpeedBoostFillUnit,
                    < 0 => SpeedBoostVal = 0,
                    _ => SpeedBoostVal = 0,
                };
            }
        }

        // increase the SpeedBoostVal gradually when Shift is not pressed
        private IEnumerator HandleSpeedBoostPositiveValue() {
            while (true) {
                yield return new WaitForSeconds(0.025f);

                if (SpeedBoostVal < tankStatsSo.SpeedBoostCapacity) {
                    SpeedBoostVal += tankStatsSo.SpeedBoostFillUnit;
                } else if (SpeedBoostVal > tankStatsSo.SpeedBoostCapacity) {
                    SpeedBoostVal = tankStatsSo.SpeedBoostCapacity;
                }
            }
        }


        // NOTE: apply engine force based on which tile position is on (if road tile or not road tile)
        private void ApplyEngineForce() {
            // set the default values for no road tiles
            var isOnRoad = _roadTileManager.IsRoadTile(transform.position);
            var currMaxSpeed = tankStatsSo.MaxSpeed;
            var currBoostMaxSpeed = tankStatsSo.MaxSpeedBoostVal;
            var currAccFactor = tankStatsSo.AccFactor;
            var currMaxSpeedBoostAccFactor = tankStatsSo.SpeedBoostAccFactor;

            // set the values properly based on the RoadTileDataSo contents
            if (isOnRoad) {
                var data = _roadTileManager.GetTileData(transform.position);
                currMaxSpeed = data.MaxSpeed;
                currAccFactor = data.AccFactor;
                currBoostMaxSpeed = data.BoostMaxSpeed;
                currMaxSpeedBoostAccFactor = data.BoostAccFactor;
            }

            // change rb.drag based on the opposingForce vector
            HandleEngineBreaks(currAccFactor);

            // handle Exhausts Objects, accelerate and maxSpeed 
            if (_isSpeeding) {
                accFactor = currMaxSpeedBoostAccFactor;
                maxSpeed = currBoostMaxSpeed;

                switch (_move) {
                    case > 0: {
                        exhaustBack.SetActive(true);
                        if (exhaustFront.activeSelf) exhaustFront.SetActive(false);
                        break;
                    }
                    case < 0: {
                        exhaustFront.SetActive(true);
                        if (exhaustFront.activeSelf) exhaustBack.SetActive(false);
                        break;
                    }
                }
            } else {
                accFactor = currAccFactor;
                maxSpeed = currMaxSpeed;
                if (exhaustFront.activeSelf) exhaustFront.SetActive(false);
                if (exhaustBack.activeSelf) exhaustBack.SetActive(false);
            }

            rigidBodyDrag = _rb.drag; // just for troubleshooting in inspector

            engineForce = transform.up * (_move * accFactor); // create a force Vector to move upwards (forward)
            _rb.AddForce(engineForce, ForceMode2D.Force);

            // set [maxSpeed] as max speed of the tank
            _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(_rb.velocity.y, -maxSpeed, maxSpeed));

            // handle tank tracks animation
            if (_rb.velocity.magnitude >= 0.1f || _rotation != 0) {
                SetTrackAnimationTo(true);
            } else {
                SetTrackAnimationTo(false);
            }
        }

        private void ApplySteering() {
            _rotationAngle -= _rotation * steerFactor; // update the angle based on input
            _rb.MoveRotation(_rotationAngle);
        }

        private void Move(InputAction.CallbackContext ctx) {
            switch (ctx.phase) {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    _move = _controls.Player.Move.ReadValue<float>();
                    break;
                case InputActionPhase.Canceled:
                    _move = 0;
                    break;
            }
        }

        private void Steer(InputAction.CallbackContext ctx) {
            switch (ctx.phase) {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    _rotation = _controls.Player.Steer.ReadValue<float>();
                    break;
                case InputActionPhase.Canceled:
                    _rotation = 0;
                    break;
            }

            _rotation = _controls.Player.Steer.ReadValue<float>();
        }

        private void SpeedUp(InputAction.CallbackContext ctx) {
            switch (ctx.phase) {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    if (_move != 0) {
                        _isSpeeding = true;
                        StartCoroutine(nameof(HandleSpeedBoostNegativeValue));
                        StopCoroutine(nameof(HandleSpeedBoostPositiveValue));
                    } else {
                        _isSpeeding = false;
                    }

                    break;
                case InputActionPhase.Canceled:
                    _isSpeeding = false;
                    StartCoroutine(nameof(HandleSpeedBoostPositiveValue));
                    StopCoroutine(nameof(HandleSpeedBoostNegativeValue));
                    break;
            }
        }

        // add more friction to the tank to avoid drifting in a single direction (multiply right by 0)
        private void KillOrthogonalVelocity() {
            var upAngle = Vector2.Dot(_rb.velocity, transform.up);
            upVelocity = transform.up * upAngle;

            var rightAngle = Vector2.Dot(_rb.velocity, transform.right);
            rightVelocity = transform.right * rightAngle;

            _rb.velocity = upVelocity + rightVelocity * driftFactor;
        }

        private void HandleEngineBreaks(float currAccFactor) {
            opposingForce = Vector2.Dot(_rb.velocity, engineForce);

            if (_move == 0f) {
                _rb.drag = Mathf.Lerp(_rb.drag, 2.0f, Time.fixedDeltaTime * 3); // apply Engine Breaks
                accFactor = currAccFactor;
            } else if (_move != 0f && opposingForce < 0) {
                accFactor = currAccFactor * 3;
                _rb.drag = Mathf.Lerp(_rb.drag, 5.0f, Time.fixedDeltaTime * 3); // apply Manual Breaks - in case the tank is still moving towards the opposite of the desired direction
            } else {
                accFactor = currAccFactor;
                _rb.drag = 0f; // don't apply any breaks when not moving forward/backward
            }
        }

        private void OnEnable() {
            _controls.Player.Move.Enable();
            _controls.Player.Steer.Enable();
            _controls.Player.SpeedBoost.Enable();

            _controls.Player.Move.performed += Move;
            _controls.Player.Move.canceled += Move;
            _controls.Player.Steer.performed += Steer;
            _controls.Player.Steer.canceled += Steer;
            _controls.Player.SpeedBoost.performed += SpeedUp;
            _controls.Player.SpeedBoost.canceled += SpeedUp;
        }

        private void OnDisable() {
            _controls.Player.Move.performed -= Move;
            _controls.Player.Move.canceled -= Move;
            _controls.Player.Steer.performed -= Steer;
            _controls.Player.Steer.canceled -= Steer;
            _controls.Player.SpeedBoost.performed -= SpeedUp;
            _controls.Player.SpeedBoost.canceled -= SpeedUp;

            _controls.Player.Move.Disable();
            _controls.Player.Steer.Disable();
            _controls.Player.SpeedBoost.Disable();
        }

        public void SetTrackAnimationTo(bool isMoving) {
            Array.ForEach(tankTracks, track => track.SetBool(IsMoving, isMoving));
        }


        #region AltTester related
        private void Debugger() {
            //NOTE: change stuff here!!
            // SetTankTargetAngle(targetDir.transform.position);
            // CalculateSpeed();
            // RotationDurationUsingAngle();
            // RotationDurationUsingAtan2();
        }

        private void OnDrawGizmos() {
            var pos = transform.position;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(pos, pos + transform.up.normalized * 3f);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(pos, pos + transform.right.normalized * 3f);

            // Gizmos.color = Color.cyan;
            // Gizmos.DrawLine(pos, pos + GetDirectionTowards(targetDir.transform.position) * 3f);
        }

        // set a Vector2(x, y), where => x = right * direction; and y = up * direction
        private void SetTankTargetAngle(Vector2 target) {
            var dir = GetDirectionTowards(target);
            var right = Vector2.Dot(transform.right, dir);
            var up = Vector2.Dot(transform.up, dir);
            tankTargetAlignment = new Vector2(right, up);
        }


        // localVelocity.x = Left vs Right Speed (>0 = right, <0 = moves left)
        // localVelocity.y = Up vs Down speed (>0 = up, <0 = down).
        private void CalculateSpeed() => localVelocity = transform.InverseTransformDirection(_rb.velocity);

        // set the duration needed to steer left or right, in seconds
        private void RotationDurationUsingAngle() {
            var angle = Vector2.Angle(transform.up, GetDirectionTowards(targetDir.transform.position));
            var rads = angle * Mathf.Deg2Rad;
            rotDur = (rads / steerFactor);
        }

        // NOTE: should work like the one above, but it doesn't...
        private void RotationDurationUsingAtan2() {
            var targetPos = targetDir.transform.position;
            var up = transform.up;
            rotDurInAtan2 = Mathf.Atan2(targetPos.y, targetPos.x) - Mathf.Atan2(up.y, up.x);
        }


        private Vector3 GetDirectionTowards(Vector3 objPos) {
            var v3 = (objPos - transform.position).normalized;
            return new Vector2(v3.x, v3.y);
        }
        #endregion
    }
}