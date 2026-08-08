using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScreenUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject endScreenPanel;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI statsText;
    public Button restartButton;

    void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        // Скрываем экран при старте
        if (endScreenPanel != null)
        {
            endScreenPanel.SetActive(false);
        }
    }

    public void ShowEndScreen(bool isVictory, int kills, float damage, float time)
    {
        if (endScreenPanel == null) return;

        endScreenPanel.SetActive(true);

        // Настраиваем текст результата
        if (resultText != null)
        {
            resultText.text = isVictory ? "VICTORY!" : "DEFEAT!";
            resultText.color = isVictory ? Color.green : Color.red;
        }

        // Настраиваем статистику
        if (statsText != null)
        {
            statsText.text = $"Kills: {kills}\nDamage Dealt: {damage:F0}\nTime: {time:F1}s";
        }
    }

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}