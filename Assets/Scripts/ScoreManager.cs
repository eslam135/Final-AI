using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public int score { get; private set; } = 0;
    public static ScoreManager Instance { get; private set; }
    [SerializeField] private int winScore = 100;
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
    public void AddScore()
    {
        score++;
        if(score >= winScore)
        {
            Debug.Log("You win!");
            // You can add more logic here, like showing a win screen or restarting the game
            SceneManager.LoadScene(0); // Assuming scene 0 is the main menu or restart scene
        }
    }
}
