using UnityEngine;

public class CycloidMovement : MonoBehaviour
{
    public float radius = 5f;     // радіус циклоїди
    public float speed = 1f;      // швидкість
    public float directionChangeThreshold = 10f; // поріг для зміни напрямку

    private float t = 0f;         // параметр часу
    private bool movingForward = true; // напрямок руху (True — вперед, False — назад)

    void Update()
    {
        // Розрахунок координат для циклоїди по осі y
        float z = radius * (t - Mathf.Sin(t)); // z буде змінюватися в залежності від t
        float y = radius * (1 - Mathf.Cos(t)); // y буде рухатися по вертикалі

        // Зміщуємо кулю по обчислених координатах
        transform.position = new Vector3(0, y, z);

        // Якщо параметр t досягнув певного порогу, змінюємо напрямок
        if (Mathf.Abs(t) > directionChangeThreshold)
        {
            movingForward = !movingForward; // змінюємо напрямок
        }

        // Змінюємо параметр t в залежності від напрямку
        if (movingForward)
        {
            t += speed * Time.deltaTime; // Рух вперед
        }
        else
        {
            t -= speed * Time.deltaTime; // Рух назад
        }
    }
}
