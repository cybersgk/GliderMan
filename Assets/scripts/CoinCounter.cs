using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class CoinCounter : MonoBehaviour {

    Text CoinText;
    private void Start()
    {
        CoinText = GetComponent<Text>();
        CoinText.text = PlayerPrefs.GetInt("Coins").ToString();
    }
}
