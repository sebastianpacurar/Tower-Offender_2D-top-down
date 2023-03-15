using System;
using System.Collections;
using Cinemachine;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Player.Controllers {
    public class TankController : MonoBehaviour {
        public float SpeedBoostVal { get; private set; }

        private PlayerControls _controls;
        private CinemachineVirtualCamera _cineMachineMainCam;
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

        [Header("For Debugging Purposes")]
        [SerializeField] private float rigidBodyDrag;
        [SerializeField] private Vector3 forwardVelocity;
        [SerializeField] private Vector3 rightVelocity;

        [Space(20)]
        [SerializeField] private GameObject exhaustBack;
        [SerializeField] private GameObject exhaustFront;


        private void Awake() {
            _controls = new PlayerControls();
            _rb = GetComponent<Rigidbody2D>();
            _cineMachineMainCam = GameObject.FindGameObjectWithTag("CM2D").GetComponent<CinemachineVirtualCamera>();

            accFactor = tankStatsSo.AccFactor;
            steerFactor = tankStatsSo.SteerFactor;
            driftFactor = tankStatsSo.DriftFactor;
            maxSpeed = tankStatsSo.MaxSpeed;
            SpeedBoostVal = tankStatsSo.SpeedBoostCapacity;
        }

        private void Start() {
            _cineMachineMainCam.Follow = transform;
        }

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


        private void ApplyEngineForce() {
            HandleEngineBreaks();

            // handle Exhausts Objects, accelerate and maxSpeed 
            if (_isSpeeding) {
                accFactor = tankStatsSo.SpeedBoostAccFactor;
                maxSpeed = tankStatsSo.MaxSpeedBoostVal;

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
                accFactor = tankStatsSo.AccFactor;
                maxSpeed = tankStatsSo.MaxSpeed;
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
            forwardVelocity = transform.up * Vector2.Dot(_rb.velocity, transform.up);
            rightVelocity = transform.right * Vector2.Dot(_rb.velocity, transform.right);

            _rb.velocity = forwardVelocity + rightVelocity * driftFactor;
        }

        private void HandleEngineBreaks() {
            opposingForce = Vector2.Dot(_rb.velocity, engineForce);

            if (_move == 0f) {
                _rb.drag = Mathf.Lerp(_rb.drag, 2.0f, Time.fixedDeltaTime * 3); // apply Engine Breaks
                accFactor = tankStatsSo.AccFactor;
            } else if (_move != 0f && opposingForce < 0) {
                accFactor = tankStatsSo.AccFactor * 3;
                _rb.drag = Mathf.Lerp(_rb.drag, 5.0f, Time.fixedDeltaTime * 3); // apply Manual Breaks - in case the tank is still moving towards the opposite of the desired direction
            } else {
                accFactor = tankStatsSo.AccFactor;
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
    }
}