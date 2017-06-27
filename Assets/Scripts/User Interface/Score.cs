using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Score : MonoBehaviour {
    public Text scoreText;

	void Start () {
        scoreText = transform.Find("ScoreNUM").GetComponent<Text>();
    }
    public void DisplayScore(int score)
    {
        scoreText.text = string.Format(score.ToString());
    }

    void Update () {
		
	}
}
