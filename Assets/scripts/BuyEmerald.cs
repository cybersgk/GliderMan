using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BuyEmerald : MonoBehaviour {

    Image BuyImage;

    public Sprite select;
    public Sprite selected;
    public Sprite fiveHundredCoins;

    private void Update()
    {
        BuyImage = GetComponent<Image>();

        if (!PlayerPrefs.HasKey("BuyedEmerald"))
        {
            PlayerPrefs.SetInt("BuyedEmerald", 2);
        }
        int condition = PlayerPrefs.GetInt("BuyedEmerald");
        if (condition == 0)
        {
            BuyImage.sprite = selected;
        }
        else if (condition == 1)
        {
            BuyImage.sprite = select;
        }
        else
        {
            BuyImage.sprite = fiveHundredCoins;
        }
    }
}
