using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreText : MonoBehaviour {
    Text Score;
    private void Start()
    {
        Score = GetComponent<Text>();
        Score.text = "Score:" + GameManager.Instance.Score.ToString();
    }
}
