using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public enum MovementMode
    {
        SideScroller,
        TopDown,
    }
    
    [Header("Movement")] 
    [SerializeField] private MovementMode movementMode = MovementMode.SideScroller;
    [SerializeField] private float moveSpeed = 7.5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (movementMode == MovementMode.SideScroller)
        {
            rb.gravityScale = 3f;
        }
        else
        {
            rb.gravityScale = 0f;
        }

    }

    private void FixedUpdate()
    {

        Move();
    }

    private void Move()
    {
        var input = UserInput.Instance;
        if (input == null) return;
        
        Vector2 move = input.Move;

        if (movementMode == MovementMode.SideScroller)
        {
            rb.linearVelocity = new Vector2(move.x * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = move * moveSpeed;
        }
    }
}
