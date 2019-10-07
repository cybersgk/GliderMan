using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BuyController : MonoBehaviour
{

    Image BuyImage;

    public Sprite select;
    public Sprite selected;

    private void Update()
    {
        BuyImage = GetComponent<Image>();
        if (!PlayerPrefs.HasKey("BuyedBlack"))
        {
            PlayerPrefs.SetInt("BuyedBlack", 0);
        }
        int condition = PlayerPrefs.GetInt("BuyedBlack");
        if (condition == 0)
        {
            BuyImage.sprite = selected;
        }
        else
        {
            BuyImage.sprite = select;
        }
    }
}
