using UnityEngine;

namespace TDS.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerInputSystem : MonoBehaviour
    {
        private float playerMoveSpeed;
        private Rigidbody2D playerRigidBody2D;
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        public void Initialize(float playerMoveSpeed)
        {
            this.playerMoveSpeed = playerMoveSpeed;
            playerRigidBody2D = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }



        public void HandleMovement()
        {
            float horiznontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            Vector2 direction = new Vector2(horiznontalInput, verticalInput).normalized;
            playerRigidBody2D.velocity = direction * playerMoveSpeed;

            if (horiznontalInput != 0)
            {
                spriteRenderer.flipX = horiznontalInput < 0;
            }

            animator.SetFloat("HorizontalMove", Mathf.Abs(horiznontalInput));
        }

        public void SetMoveSpeed(float newSpeed)
        {
            playerMoveSpeed = newSpeed;
        }

        public float GetMoveSpeed()
        {
            return playerMoveSpeed;
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }
    }
}