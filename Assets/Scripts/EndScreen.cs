using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI endScreenText;
    ScoreKeeper scoreKeeper;

    void Awake()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public void DisplayFinalScore()
    {
        endScreenText.text = "Congratulations!\nYour Score: " + scoreKeeper.CalculateScore();
    }

    
}
