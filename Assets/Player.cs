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

        // Підписуємося на подію програшу
        if (GameManager.Instance != null)
        {
            Debug.Log($"Coins: {GameManager.Instance.Coins}, Lifes: {GameManager.Instance.Lifes}, MaxCoinsCollected: {GameManager.Instance.MaxCoinsCollected}, CollisionsWithDangerTotal: {GameManager.Instance.CollisionsWithDangerTotal}");

            GameManager.Instance.OnGameOver += HandleGameOver;
        }
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Player triggered layer {other.gameObject.layer}, TimeAtStart: {GameManager.Instance.GetTimeElapsed()}");

        if (other.gameObject.layer == 7)
        {
            SomeDanger damageObject = other.gameObject.GetComponent<SomeDanger>();

            if (damageObject != null)
            {
                GameManager.Instance.DecreaseLifes(damageObject.damage);
                GameManager.Instance.IncreaseCollisionCount();

                Debug.Log($"lifes now: {GameManager.Instance.Lifes} damage was: {damageObject.damage}");
                // Destroy(other.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        // Відписуємося від події, щоб уникнути помилок
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameOver -= HandleGameOver;
        }
    }

    private void HandleGameOver()
    {
        Debug.Log("You lost! Game Over!");
    }
}