using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreBoard : MonoBehaviour
{

    public TMP_Text ScoreText;
    public TMP_Text CurrentSpeedText;

    public void UpdateScore(int score)
    {
        ScoreText.text = "Score: " + score.ToString();
    }
    public void UpdateSpeed(float speed)
    {
        CurrentSpeedText.text = "Speed: " + RoundUpToFirstSignificantDigit(speed).ToString();
    }

    private static float RoundUpToFirstSignificantDigit(float number)
    {
        if (number == 0)
        {
            return 0;
        }
        
        float scale = (float)Math.Pow(10, -Math.Floor(Math.Log10(Math.Abs(number))));
        float scaledNumber = number * scale;
        float roundedScaledNumber = (float)Math.Ceiling(scaledNumber);
        float roundedNumber = roundedScaledNumber / scale;
        
        return roundedNumber;
    }
}
