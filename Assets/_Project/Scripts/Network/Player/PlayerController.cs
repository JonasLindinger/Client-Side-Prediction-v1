using LindoNoxStudio.Network.Input;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace LindoNoxStudio.Network.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : NetworkBehaviour
    {
        [Header("SETTINGS")]
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _sprintSpeed;
        [SerializeField] private float _crouchSpeed;
        [SerializeField] private float _groundDrag;
        [Space(2)]
        [SerializeField] private float _jumpForce; 
        [SerializeField] private float _jumpColldown;
        [SerializeField] private float _airMultipier;
        [Space(10)]
        [Header("REFERENCES")]
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private Transform _orientation;
        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private float _playerHeight;

        private bool _grounded;
        private bool _readyToJump = true;

        private Rigidbody rb;

        public override void OnNetworkSpawn()
        {
            // Referencing
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            _playerCamera.enabled = IsOwner;
            _playerCamera.GetComponent<AudioListener>().enabled = IsOwner;

            // Cursoe
            if (IsLocalPlayer)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public void OnInput(ClientInputState input)
        {
            if (input == null) return;
            
            // Applying movement
            // Setting the drag
            _grounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _whatIsGround);

            if (_grounded)
                rb.linearDamping = _groundDrag;
            else
                rb.linearDamping = 0;

            // Calculating movement
            Vector2 moveInput = input.GetMoveInput();

            _orientation.rotation = Quaternion.Euler(0, input.PlayerRotation, 0);
            Vector3 moveDirection = _orientation.forward * moveInput.y + _orientation.right * moveInput.x;

            // Applying movement

            float moveSpeed = input.IsSprinting ? _sprintSpeed : input.IsCrouching ? _crouchSpeed : _walkSpeed;

            // Grounded
            if (_grounded)
                rb.AddForce(moveDirection.normalized * (moveSpeed * 10), ForceMode.Force);

            // In air
            else
                rb.AddForce(moveDirection.normalized * (moveSpeed * 10 * _airMultipier), ForceMode.Force);

            // Speed Control
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }

            if (input.IsJumping && _grounded && _readyToJump)
            {
                // Resetting Y velocity
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

                // Applying Force
                rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);

                // Applying Cooldown
                _readyToJump = false;
                Invoke(nameof(ResetJump), _jumpColldown);
            }
        }
        
        private void ResetJump()
        {
            _readyToJump = true;
        }

        #region State

        public PlayerState GetState(uint tick, ClientInputState nextInput)
        {
            PlayerState state = new PlayerState();
            state.SetUp(tick, transform.position, transform.eulerAngles, rb.linearVelocity);
            
            return state;
        }

        public void ApplyState(PlayerState state, uint tick = 0)
        {
            transform.position = state.Position;
            transform.rotation = Quaternion.Euler(state.Rotation);

            rb.linearVelocity = state.Velocity;
        }

        #endregion
    }
}