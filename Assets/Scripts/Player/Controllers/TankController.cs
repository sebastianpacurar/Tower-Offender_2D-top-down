using System;
using Cinemachine;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Player.Controllers {
    public class TankController : MonoBehaviour {
        private PlayerControls _controls;
        private InputAction _moveAction, _steerAction;
        private CinemachineVirtualCamera _cineMachineCam;
        private Rigidbody2D _rb;

        private float _move, _rotation;
        private float _rotationAngle = 0f;

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


        private void Awake() {
            _controls = new PlayerControls();
            _moveAction = _controls.Player.Move;
            _steerAction = _controls.Player.Steer;
            _rb = GetComponent<Rigidbody2D>();

            accFactor = tankStatsSo.AccFactor;
            steerFactor = tankStatsSo.SteerFactor;
            driftFactor = tankStatsSo.DriftFactor;
            maxSpeed = tankStatsSo.MaxSpeed;
        }

        private void Start() {
            _cineMachineCam = GameObject.FindGameObjectWithTag("CM2D").GetComponent<CinemachineVirtualCamera>();
            _cineMachineCam.Follow = transform;
        }

        private void FixedUpdate() {
            ApplyEngineForce();
            KillOrthogonalVelocity();
            ApplySteering();
        }

        private void ApplyEngineForce() {
            HandleEngineBreaks();
            rigidBodyDrag = _rb.drag;

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
                    _move = _moveAction.ReadValue<float>();
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
                    _rotation = _steerAction.ReadValue<float>();
                    break;
                case InputActionPhase.Canceled:
                    _rotation = 0;
                    break;
            }

            _rotation = _steerAction.ReadValue<float>();
        }

        // add more friction to the tank to avoid drifting in a single direction
        private void KillOrthogonalVelocity() {
            forwardVelocity = transform.up * Vector2.Dot(_rb.velocity, transform.up);
            rightVelocity = transform.right * Vector2.Dot(_rb.velocity, transform.right);

            _rb.velocity = forwardVelocity + rightVelocity * driftFactor;
        }

        private void HandleEngineBreaks() {
            opposingForce = Vector2.Dot(_rb.velocity, engineForce);
            if (_move == 0f) {
                _rb.drag = Mathf.Lerp(_rb.drag, 3.0f, Time.fixedDeltaTime * 3); // apply Engine Breaks
                accFactor = 2.0f;
            } else if (_move != 0f && opposingForce < 0) {
                accFactor = 4.0f;
                _rb.drag = Mathf.Lerp(_rb.drag, 5.0f, Time.fixedDeltaTime * 5); // apply Manual Breaks - in case the tank is still moving towards the opposite of the desired direction
            } else {
                accFactor = 2.0f;
                _rb.drag = 0f; // don't apply any breaks when not moving forward/backward
            }
        }

        private void OnEnable() {
            _moveAction.Enable();
            _steerAction.Enable();

            _moveAction.performed += Move;
            _moveAction.canceled += Move;
            _steerAction.performed += Steer;
            _steerAction.canceled += Steer;
        }

        private void OnDisable() {
            _moveAction.performed -= Move;
            _moveAction.canceled -= Move;
            _steerAction.performed -= Steer;
            _steerAction.canceled -= Steer;

            _moveAction.Disable();
            _steerAction.Disable();
        }

        public void SetTrackAnimationTo(bool isMoving) {
            Array.ForEach(tankTracks, track => track.SetBool("IsMoving", isMoving));
        }
    }
}