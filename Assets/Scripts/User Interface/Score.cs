using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Score : MonoBehaviour {
    public Text scoreText;

	void Start () {
        scoreText = transform.Find("ScoreNUM").GetComponent<Text>(); // Zoek tekstcomponent
    }
    public void DisplayScore(int score)
    {
        scoreText.text = string.Format(score.ToString()); // Parste Int naar String en plaats waarde in variable
    }

}
