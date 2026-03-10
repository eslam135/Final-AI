using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private ScoreManager scoreManager;

    [SerializeField] private TextMeshProUGUI healthText;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        scoreText.text = "Score: " + scoreManager.score.ToString();
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + scoreManager.score.ToString();
    }

    public void UpdateHealth(float currentHealth)
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth.ToString();
        }
    }
}