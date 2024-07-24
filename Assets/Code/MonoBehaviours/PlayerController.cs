using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.MonoBehaviours {
    public class PlayerController : MonoBehaviour {
        [SerializeField] private float _moveSpeed = 5f;
        private Rigidbody2D _rb;
        private InputAction _moveAction;
        
       private void Start() {
           _rb = GetComponent<Rigidbody2D>();
           _moveAction = InputSystem.actions.FindAction("HorizontalMove");
       }

       private void FixedUpdate() {
           float horizontalMoveInput = _moveAction.ReadValue<float>();
           _rb.velocity = new Vector2(horizontalMoveInput * _moveSpeed, _rb.velocity.y);
       }
    }
}