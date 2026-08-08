using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public GameObject playerBase;
    public GameObject enemyBase;

    [Header("UI")]
    public EndScreenUI endScreenUI;

    [Header("Stats")]
    private int kills = 0;
    // private int deaths = 0; // Удалено, так как не используется в текущей версии
    private float damageDealt = 0f;
    private float gameStartTime;
    private bool isGameActive = true;

    // Публичное свойство только для чтения для других скриптов
    public bool IsGameActive => isGameActive;

    public static GameManager Instance;

    void Awake()
    {
        // Singleton паттерн
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameStartTime = Time.time;

        // Подписываемся на события смерти баз
        if (playerBase != null)
        {
            HealthSystem playerBaseHealth = playerBase.GetComponent<HealthSystem>();
            if (playerBaseHealth != null)
            {
                playerBaseHealth.onDeath.AddListener(GameOver);
            }
        }

        if (enemyBase != null)
        {
            HealthSystem enemyBaseHealth = enemyBase.GetComponent<HealthSystem>();
            if (enemyBaseHealth != null)
            {
                enemyBaseHealth.onDeath.AddListener(Victory);
            }
        }

        Debug.Log("Game started!");
    }

    // Вызывается когда игрок убивает врага
    public void AddKill()
    {
        kills++;
        // Debug.Log($"Kills: {kills}"); // Закомментировано, чтобы не спамить консоль при массовых убийствах
    }

    // Вызывается когда игрок наносит урон
    public void AddDamageDealt(float damage)
    {
        damageDealt += damage;
    }

    // Победа
    void Victory()
    {
        if (!isGameActive) return; // Защита от двойного срабатывания
        isGameActive = false;

        float gameTime = Time.time - gameStartTime;
        Debug.Log($"VICTORY! Kills: {kills}, Damage: {damageDealt:F1}, Time: {gameTime:F1}s");

        if (endScreenUI != null)
        {
            endScreenUI.ShowEndScreen(true, kills, damageDealt, gameTime);
        }
    }

    // Поражение
    void GameOver()
    {
        if (!isGameActive) return; // Защита от двойного срабатывания
        isGameActive = false;

        float gameTime = Time.time - gameStartTime;
        Debug.Log($"DEFEAT! Kills: {kills}, Damage: {damageDealt:F1}, Time: {gameTime:F1}s");

        if (endScreenUI != null)
        {
            endScreenUI.ShowEndScreen(false, kills, damageDealt, gameTime);
        }
    }
}