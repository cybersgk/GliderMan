using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BuySilver : MonoBehaviour
{

    Image BuyImage;

    public Sprite select;
    public Sprite selected;
    public Sprite hundredCoins;

    private void Update()
    {
        BuyImage = GetComponent<Image>();

        if (!PlayerPrefs.HasKey("BuyedSilver"))
            {
                PlayerPrefs.SetInt("BuyedSilver", 2);
            }
        int condition = PlayerPrefs.GetInt("BuyedSilver");
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
            BuyImage.sprite = hundredCoins;
        }

    }
}
