using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score { get; private set; } = 0;
    public static ScoreManager Instance { get; private set; }
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
    }
}
