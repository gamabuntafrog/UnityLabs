using System;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int coins;
    public int lifes;
    public int maxCoinsCollected;
    public int CollisionsWithDangerTotal;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private string savePath;
    public event Action OnGameOver; // Подія програшу

    public int Coins { get; private set; }
    public int MaxCoinsCollected { get; private set; }
    public int CollisionsWithDangerTotal { get; private set; }
    public int Lifes { get; private set; } = 5;
    private const int START_LIFES = 5;
    private const int START_COINS = 0;
    public float timeLimit = 60f; // Ліміт часу в секундах
    private float timeRemaining;
    private float timeAtStart; // Час, коли рівень був запущений

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Path.Combine(Application.persistentDataPath, "gameData.json");
            Debug.Log(savePath);
            LoadGame();
            timeAtStart = Time.time; // Збереження часу при запуску рівня
            timeRemaining = timeLimit;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Відрахунок часу
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 0;
            Debug.Log("Time's up!");
            ResetGame(); 
            OnGameOver?.Invoke(); 
        }
    }


    public void AddCoin()
    {
        Coins++;

        if (Coins > MaxCoinsCollected) {
            MaxCoinsCollected = Coins;
        }

        Debug.Log("ADD COIN TRIGGERED " + Coins);
        SaveGame();
    }

    public void DecreaseLifes(int num) {
        Lifes -= num;

        if (Lifes <= 0)
        {
            ResetGame(); 
            
            OnGameOver?.Invoke(); 
        } else {
            SaveGame();
        }
    }

    public void IncreaseCollisionCount() {
        CollisionsWithDangerTotal++;
    }

    private void SaveGame()
    {
        GameData data = new GameData { coins = Coins, lifes = Lifes, maxCoinsCollected = MaxCoinsCollected, CollisionsWithDangerTotal = CollisionsWithDangerTotal };
        File.WriteAllText(savePath, JsonUtility.ToJson(data));
    }

    private void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            Coins = data.coins;
            Lifes = data.lifes;
            MaxCoinsCollected = data.maxCoinsCollected;
            CollisionsWithDangerTotal = data.CollisionsWithDangerTotal;
        } else
        {
            ResetGame(); 
        }
    }

    private void ResetGame()
    {
        Coins = START_COINS;
        Lifes = START_LIFES;
        timeRemaining = timeLimit; // Скидаємо таймер
        timeAtStart = Time.time; // Оновлюємо час запуску рівня

        SaveGame();
        Debug.Log("Game progress reset.");
    }

    // Метод для отримання часу від початку рівня
    public float GetTimeElapsed()
    {
        return Time.time - timeAtStart;
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

}
