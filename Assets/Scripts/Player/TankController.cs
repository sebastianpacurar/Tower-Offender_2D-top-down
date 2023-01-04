using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class TankController : MonoBehaviour {
        private PlayerControls _controls;
        private InputAction _moveAction, _steerAction;
        private Rigidbody2D _rb;

        private float _move, _rotation;
        private float _rotationAngle = 0f;

        [Header("Physics related")]
        [SerializeField] private float accFactor = 10f;

        [SerializeField] private float steerFactor = 3.5f;
        [SerializeField] private float driftFactor = 0f;
        [SerializeField] private Vector3 engineForce;
        [SerializeField] private float maxSpeed;

        [Header("Track Animators")]
        [SerializeField] private Animator[] tankTracks;

        private CinemachineVirtualCamera _cineMachineCam;

        private void Awake() {
            _controls = new PlayerControls();
            _moveAction = _controls.Player.Move;
            _steerAction = _controls.Player.Steer;
            _rb = GetComponent<Rigidbody2D>();
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
            // if Up or Down keys are not pressed, then cause the _rb to drag until it comes to a complete stop
            if (_move == 0f) {
                _rb.drag = Mathf.Lerp(_rb.drag, 3.0f, Time.fixedDeltaTime * 3);
            } else {
                _rb.drag = 0;
            }

            engineForce = transform.up * (_move * accFactor); // create a force Vector to move upwards (forward)
            _rb.AddForce(engineForce, ForceMode2D.Force);

            // set [maxSpeed] as max speed of the tank
            _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(_rb.velocity.y, -maxSpeed, maxSpeed));

            // handle tank tracks animation
            if (_rb.velocity.magnitude >= 0.1f || _rotation != 0) {
                Array.ForEach(tankTracks, track => track.SetBool("IsMoving", true));
            } else {
                Array.ForEach(tankTracks, track => track.SetBool("IsMoving", false));
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
            var forwardVelocity = transform.up * Vector2.Dot(_rb.velocity, transform.up);
            var rightVelocity = transform.right * Vector2.Dot(_rb.velocity, transform.right);

            _rb.velocity = forwardVelocity + rightVelocity * driftFactor;
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
    }
}