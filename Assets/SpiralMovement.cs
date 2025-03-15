using UnityEngine;

public class SpiralMovement : MonoBehaviour
{
    public float radius = 5f;        // Максимальний радіус спіралі
    public float speed = 2f;         // Швидкість обертання
    public float spiralSpeed = 0.5f; // Швидкість збільшення радіуса
    public float yVelocity = 1f; // Швидкість збільшення радіуса

    public bool returnToStart = true; // Чи повертатися в початкове положення
    
    private Vector3 startPosition;
    private float angle = 0f;
    private float currentRadius = 0f;
    private bool isReturning = false;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (isReturning)
        {
            ReturnToStart();
        }
        else
        {
            MoveInSpiral();
        }
    }

    void MoveInSpiral()
    {
        angle += speed * Time.deltaTime;
        currentRadius += spiralSpeed * Time.deltaTime;
        
        if (currentRadius >= radius)
        {
            if (returnToStart)
            {
                isReturning = true;
            }
            return;
        }

        float x = Mathf.Cos(angle) * currentRadius;
        float z = Mathf.Sin(angle) * currentRadius;
        transform.position = startPosition + new Vector3(x, yVelocity += Time.deltaTime, z);
    }

    void ReturnToStart()
    {
        transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, startPosition) < 0.1f)
        {
            isReturning = false;
            angle = 0f;
            currentRadius = 0f;
        }
    }
}