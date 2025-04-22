using UnityEngine;
using TMPro;


public class Test : MonoBehaviour
{
    public static Test Instance;

    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        Instance = this;
    }

    public void SetScoreText(string value)
    {
        scoreText.text = value;
    }

    public static void StaticSetScore(string value)
    {
        if (Instance != null)
        {
            Instance.scoreText.text = value;
        }
    }
}

