using UnityEngine;

public class script1 : MonoBehaviour
{
    public float length = 2f;  // Довжина маятника
    public float maxAngle = 45f;  // Максимальний кут (в градусах)
    public float gravity = 19.81f;  // Прискорення вільного падіння
    public float damping = 0.995f;  // Загасання (щоб коливання поступово спинялись)

    private float angle;  // Поточний кут
    private float angularVelocity;  // Кутова швидкість
    private float angularAcceleration;  // Кутове прискорення
    private float time;  // Час для обчислень

    void Start()
    {
        // Ініціалізація початкових значень
        angle = maxAngle;
        angularVelocity = 0;
        time = 0;
    }

    void Update()
    {
        // Обчислення кутового прискорення за допомогою фізики маятника
        angularAcceleration = (-gravity / length) * Mathf.Sin(Mathf.Deg2Rad * angle);
        
        // Оновлення кутової швидкості та кута
        angularVelocity += angularAcceleration * Time.deltaTime;
        angle += angularVelocity * Time.deltaTime;

        // Додавання загасання
        angularVelocity *= damping;

        // Обмеження кута, щоб не перевищити задану межу
        angle = Mathf.Clamp(angle, -maxAngle, maxAngle);

        // Зміна обертання об'єкта на основі кута
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
