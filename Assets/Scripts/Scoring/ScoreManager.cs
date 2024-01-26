using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IObserver
{
    [SerializeField] private GameData gameData;
    [SerializeField] private TextMeshProUGUI scoreText;

    private float showScoreDuration = 1f;

    public void AddScore(float amount)
    {
        gameData.AddScore(amount);
        UpdateScoreUI();

        StopAllCoroutines();
        StartCoroutine(ShowScore());
    }

    public void ResetScore()
    {
        gameData.ResetScore();
    }

    public void SetHighScore()
    {
        gameData.SetHighScore();
    }

    public void ResetHighScore()
    {
        gameData.ResetHighScore();
    }

    public void ResetAll()
    {
        gameData.ResetAll();
    }

    public void SaveHighScore()
    {
        gameData.SaveHighScore();
    }

    public void LoadHighScore()
    {
        gameData.LoadHighScore();
    }

    public float GetScore()
    {
        return gameData.Score;
    }

    public float GetHighScore()
    {
        return gameData.HighScore;
    }

    public void OnNotify()
    {
        AddScore(1000);
    }

    private void Start()
    {
        UpdateScoreUI();
        scoreText.gameObject.SetActive(false);
    }

    private void UpdateScoreUI()
    {
        scoreText.text = gameData.Score.ToString();
    }

    private IEnumerator ShowScore()
    {
        scoreText.gameObject.SetActive(true);
        yield return new WaitForSeconds(showScoreDuration);
        scoreText.gameObject.SetActive(false);
    }
}
