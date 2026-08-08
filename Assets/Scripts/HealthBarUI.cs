using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("Settings")]
    public HealthSystem healthSystem;
    public Slider slider;

    [Header("Colors")]
    public Color fullHealthColor = Color.green;
    public Color lowHealthColor = Color.red;
    public float lowHealthThreshold = 0.3f;

    void OnEnable()
    {
        if (healthSystem != null && slider != null)
        {
            // Подписываемся на события
            healthSystem.onHealthChanged.AddListener(UpdateHealthBar);
            healthSystem.onDeath.AddListener(OnDeath);
            Invoke(nameof(InitializeHealthBar), 0.1f);
        }
    }

    void InitializeHealthBar()
    {
        if (healthSystem != null && slider != null)
        {
            UpdateHealthBar(healthSystem.GetCurrentHealth());
        }
    }

    void UpdateHealthBar(float currentHealth)
    {
        if (slider == null || healthSystem == null) return;

        float healthPercent = healthSystem.GetHealthPercentage();
        slider.value = healthPercent;

        Image fillImage = slider.fillRect.GetComponent<Image>();
        if (fillImage != null)
        {
            if (healthPercent < lowHealthThreshold)
            {
                fillImage.color = lowHealthColor;
            }
            else
            {
                fillImage.color = fullHealthColor;
            }
        }

        Debug.Log($"HealthBar {gameObject.name}: {healthPercent * 100:F0}%");
    }

    void OnDeath()
    {
        if (slider != null)
        {
            slider.value = 0;
        }
    }

    void OnDisable()
    {
        if (healthSystem != null)
        {
            healthSystem.onHealthChanged.RemoveListener(UpdateHealthBar);
            healthSystem.onDeath.RemoveListener(OnDeath);
        }
    }
}