using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BuyGold : MonoBehaviour
{

    Image BuyImage;

    public Sprite select;
    public Sprite selected;
    public Sprite threeHundredCoins;

    private void Update()
    {
        BuyImage = GetComponent<Image>();

        if (!PlayerPrefs.HasKey("BuyedGold"))
            {
                PlayerPrefs.SetInt("BuyedGold", 2);
            }
        int condition = PlayerPrefs.GetInt("BuyedGold");
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
            BuyImage.sprite = threeHundredCoins;
        }

    }
}
