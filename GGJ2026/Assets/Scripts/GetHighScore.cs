using TMPro;
using UnityEngine;

public class GetHighScore : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _score;
    void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        Debug.Log("High Score: " + highScore);
        _score.text = highScore.ToString();
    }
}
