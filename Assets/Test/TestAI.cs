using UnityEngine;
using TMPro;
public class TestAI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score = 0;

    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();
    }
}
