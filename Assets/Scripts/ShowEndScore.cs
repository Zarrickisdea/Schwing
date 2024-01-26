using UnityEngine;
using TMPro;

public class ShowEndScore : MonoBehaviour
{
    [SerializeField] private GameData score;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText.text = score.Score.ToString();
    }

    private void OnDestroy()
    {
        score.ResetScore();
    }
}
