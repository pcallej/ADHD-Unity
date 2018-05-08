using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainCount : MonoBehaviour {

    public int score;
    public Text scoreText;
    
	void Start () {
        score = 0;
        Debug.Log(score);
	}

    public void UpdateScore(int numScore) {
        scoreText.text = score.ToString();
        if (score >= 10) {
            scoreText.text = "You Win!";
        }
    }
}
