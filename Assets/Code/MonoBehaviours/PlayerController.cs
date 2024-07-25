using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.MonoBehaviours {
    public class PlayerController : MonoBehaviour {
        [SerializeField] private float _moveSpeed = 4f;
        [SerializeField] private float _jumpForce = 7f;
        [SerializeField] private float _maxDownwardVelocity = -10f;
        [SerializeField] private float _fastFallAfterJumpDelay = 0.18f;
        
        private Rigidbody2D _rb;
        private InputAction _horizontalMoveAction;
        private InputAction _jumpAction;
        private InputAction _fastFallAction;
        private GameObject[] _floors;
        private bool _isGrounded;

        private float _timeOfLastJump;
        private bool _isHoldingFastFallSinceBeforeLastJump;

       private void Start() {
           _rb = GetComponent<Rigidbody2D>();
           _horizontalMoveAction = InputSystem.actions.FindAction("HorizontalMove");
           _jumpAction = InputSystem.actions.FindAction("Jump");
           _fastFallAction = InputSystem.actions.FindAction("FastFall");
           _floors = GameObject.FindGameObjectsWithTag("Floors");
       }

       private void Update() {
           if (_jumpAction.triggered && _isGrounded) {
               _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
               _timeOfLastJump = Time.time;
               _isHoldingFastFallSinceBeforeLastJump = _fastFallAction.ReadValue<float>() > 0f;
           }

           if (_isHoldingFastFallSinceBeforeLastJump && timeSinceLastJump() > _fastFallAfterJumpDelay && (_isGrounded || _fastFallAction.ReadValue<float>() == 0f)) {
               _isHoldingFastFallSinceBeforeLastJump = false;
           }
       }

       private void FixedUpdate() {
           var newVelocity = new Vector2(_rb.velocity.x, _rb.velocity.y);
           var horizontalMoveValue = _horizontalMoveAction.ReadValue<float>();
           newVelocity.x = horizontalMoveValue > 0f ? _moveSpeed : horizontalMoveValue < 0f ? -_moveSpeed : 0;
           if (!_isGrounded && !_isHoldingFastFallSinceBeforeLastJump && _fastFallAction.ReadValue<float>() > 0f && timeSinceLastJump() >= _fastFallAfterJumpDelay) {
               newVelocity.y = Math.Max(newVelocity.y * 2 - _jumpForce, _maxDownwardVelocity);
           }
           _rb.velocity = newVelocity;
       }

       private void OnCollisionEnter2D(Collision2D other) {
           if(_floors.Contains(other.gameObject)) {
               _isGrounded = true;
           }
       }
       
       private void OnCollisionExit2D(Collision2D other) {
           if (_floors.Contains(other.gameObject)) {
               _isGrounded = false;
           }
       }

       private float timeSinceLastJump() {
           return Time.time - _timeOfLastJump;
       }
    }
}