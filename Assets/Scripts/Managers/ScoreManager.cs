using UnityEngine;
using UnityEngine.UI; // Use TMPro if you're using TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int score = 0;

    [SerializeField] private Text scoreText;
    
    private void Awake()
    {
        // Ensure only one instance of ScoreManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UpdateScoreText();
    }

    /// <summary>
    /// Adds the given amount to the score and updates the UI.
    /// </summary>
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    /// <summary>
    /// Updates the score display.
    /// </summary>
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    /// <summary>
    /// Resets the score to zero and updates the UI.
    /// </summary>
    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
}