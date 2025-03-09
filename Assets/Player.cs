using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform groundCheckTransform = null;
    public float moveSpeed = 5f;  // Швидкість руху вліво/вправо
    public float sprintMultiplier = 2f; // Множник швидкості для прискорення
    public float jumpForce = 7f;  // Сила стрибка    
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = 1f;
        }

        float currentSpeed = moveSpeed;
        
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= sprintMultiplier;
        }
        
        Vector3 moveDirection = new Vector3(moveInput * currentSpeed, rb.linearVelocity.y, 0f);
        rb.linearVelocity = moveDirection;
    }

    void Jump()
    {
        isGrounded = Physics.OverlapSphere(groundCheckTransform.position, 0.3f).Length == 2;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }
}