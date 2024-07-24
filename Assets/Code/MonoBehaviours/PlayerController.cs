using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.MonoBehaviours {
    public class PlayerController : MonoBehaviour {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _jumpForce = 7f;
        
        private Rigidbody2D _rb;
        private InputAction _horizontalMoveAction;
        private InputAction _jumpAction;
        private GameObject[] _floors;
        private bool _isGrounded;
        
       private void Start() {
           _rb = GetComponent<Rigidbody2D>();
           _horizontalMoveAction = InputSystem.actions.FindAction("HorizontalMove");
           _jumpAction = InputSystem.actions.FindAction("Jump");
           _floors = GameObject.FindGameObjectsWithTag("Floors");
       }

       private void Update() {
           if (_jumpAction.triggered && _isGrounded) {
               _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
           }
       }

       private void FixedUpdate() {
           float horizontalMoveInput = _horizontalMoveAction.ReadValue<float>();
           _rb.velocity = new Vector2(horizontalMoveInput * _moveSpeed, _rb.velocity.y);
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
    }
}