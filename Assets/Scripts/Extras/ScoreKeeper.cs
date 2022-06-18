using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreKeeper : Singleton<ScoreKeeper>
{
    public int Score = 0;
    public int Wave = 0;
    public int Life = 10;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI wave;
    [SerializeField] TextMeshProUGUI lives;
    private void Update()
    {
        wave.text = ("Wave: " + Wave);
        score.text = ("Score: " + Score);
        lives.text = ("Lives: " + Life);
        if (Life <= 0)
        {
            SceneManager.LoadScene(0);
        }




    }
}
