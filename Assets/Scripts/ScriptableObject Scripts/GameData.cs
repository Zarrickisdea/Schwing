using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [SerializeField] private float score;
    [SerializeField] private float highScore;

    public float Score { get => score; set => score = value; }
    public float HighScore { get => highScore; set => highScore = value; }

    public void ResetScore()
    {
        score = 0;
    }

    public void AddScore(float amount)
    {
        score += amount;
    }

    public void SetHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
        }
    }

    public void ResetHighScore()
    {
        highScore = 0;
    }

    public void ResetAll()
    {
        ResetScore();
        ResetHighScore();
    }

    public void SaveHighScore()
    {
        PlayerPrefs.SetFloat("HighScore", highScore);
    }

    public void LoadHighScore()
    {
        highScore = PlayerPrefs.GetFloat("HighScore");
    }
}
