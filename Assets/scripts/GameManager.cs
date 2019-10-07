using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;
    public static event GameDelegate OnGameLife;
    public static event GameDelegate OnChangeRigidBody;

    public float speedUp = 1f;

    public GameObject startPage, gameOverPage, countdownPage, storePage;
    public Text scoreText,coinText;
    enum PageState
    {
        none,
        start,
        gameOver,
        countdown,
        store
    }
    int score = 0;
    bool gameOver = true;
    public bool GameOver { get { return gameOver; } }
    public int Score { get { return score; } }

    private void Awake()
    {
        Instance = this;
        //PlayerPrefs.SetInt("Coins", 1000);
    }
    void OnPlyerDied()
    {
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("HighScore");
        if (score > savedScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        SetPageState(PageState.gameOver);
        Time.timeScale = 1f;
    }

    void OnPlyerScored()
    {
        score++;
        scoreText.text = score.ToString();
        int coins = PlayerPrefs.GetInt("Coins");
        coins++;
        PlayerPrefs.SetInt("Coins", coins);
        coinText.text = PlayerPrefs.GetInt("Coins").ToString();
        if (score % 3 == 0)
        {
            Time.timeScale = Time.timeScale + speedUp;
        }
    }
    void OnPlyerLife()
    {
        gameOver = true;
        //if (score>0)
            //score--;
        scoreText.text = score.ToString();
        OnGameLife();
        SetPageState(PageState.countdown);
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        int coins = PlayerPrefs.GetInt("Coins");
        if (coins == 0)
        {
            PlayerPrefs.SetInt("Coins", coins);
        }
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        TapController.OnPlayerDied += OnPlyerDied;
        TapController.OnPlayerScored += OnPlyerScored;
        TapController.OnPlayerLife += OnPlyerLife;
    }
    private void OnDisable()
    {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
        TapController.OnPlayerDied -= OnPlyerDied;
        TapController.OnPlayerScored -= OnPlyerScored;
        TapController.OnPlayerLife -= OnPlyerLife;
    }
    void OnCountdownFinished()
    {
        SetPageState(PageState.none);
        OnGameStarted();
        gameOver = false;
    }

    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.none:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                storePage.SetActive(false);
                break;
            case PageState.start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                storePage.SetActive(false);
                break;
            case PageState.gameOver:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countdownPage.SetActive(false);
                storePage.SetActive(false);
                break;
            case PageState.countdown:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);
                storePage.SetActive(false);
                break;
            case PageState.store:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                storePage.SetActive(true);
                break;
        }
    }

    public void ConfirmGameOver()
    {
        //Replay
        OnGameOverConfirmed();
        scoreText.text = "0";
        SetPageState(PageState.start);
        score = 0;
        OnChangeRigidBody();
    }
    public void StartGame()
    {
        //Start
        SetPageState(PageState.countdown);
    }
    public void ShowStore()
    {
        //Store
        if (storePage.activeSelf)
        {
            SetPageState(PageState.start);
        }
        else
        {
            SetPageState(PageState.store);
        }
    }
    public void selectBlackWings()
    {
        int condition = PlayerPrefs.GetInt("BuyedBlack");
        if (condition == 0)
        {
            PlayerPrefs.SetInt("BuyedBlack", 0);
            PlayerPrefs.SetInt("Wings", 0);
            OnChangeRigidBody();
        }
        else
        {
            PlayerPrefs.SetInt("BuyedBlack", 0);
            PlayerPrefs.SetInt("Wings", 0);
            OnChangeRigidBody();
            if (PlayerPrefs.GetInt("BuyedSilver") == 0)
            {
                PlayerPrefs.SetInt("BuyedSilver", 1);
            }
            if (PlayerPrefs.GetInt("BuyedGold") == 0)
            {
                PlayerPrefs.SetInt("BuyedGold", 1);
            }
            if (PlayerPrefs.GetInt("BuyedEmerald") == 0)
            {
                PlayerPrefs.SetInt("BuyedEmerald", 1);
            }
        }
    }
    public void selectSilverWings()
    {
        int condition = PlayerPrefs.GetInt("BuyedSilver");
        if (condition == 0)
        {
            PlayerPrefs.SetInt("BuyedSilver", 0);
            PlayerPrefs.SetInt("Wings", 1);
            OnChangeRigidBody();
        }
        else if (condition == 1)
        {
            PlayerPrefs.SetInt("BuyedSilver", 0);
            PlayerPrefs.SetInt("Wings", 1);
            OnChangeRigidBody();
            if (PlayerPrefs.GetInt("BuyedBlack") == 0)
            {
                PlayerPrefs.SetInt("BuyedBlack", 1);
            }
            if (PlayerPrefs.GetInt("BuyedGold") == 0)
            {
                PlayerPrefs.SetInt("BuyedGold", 1);
            }
            if (PlayerPrefs.GetInt("BuyedEmerald") == 0)
            {
                PlayerPrefs.SetInt("BuyedEmerald", 1);
            }
        }
        else
        {
            int coins = PlayerPrefs.GetInt("Coins");
            if (coins>=100)
            {
                PlayerPrefs.SetInt("Coins", (coins-100));
                coinText.text = PlayerPrefs.GetInt("Coins").ToString();
                PlayerPrefs.SetInt("BuyedSilver", 0);
                PlayerPrefs.SetInt("Wings", 1);
                OnChangeRigidBody();
                if (PlayerPrefs.GetInt("BuyedBlack") == 0)
                {
                    PlayerPrefs.SetInt("BuyedBlack", 1);
                }
                if (PlayerPrefs.GetInt("BuyedGold") == 0)
                {
                    PlayerPrefs.SetInt("BuyedGold", 1);
                }
                if (PlayerPrefs.GetInt("BuyedEmerald") == 0)
                {
                    PlayerPrefs.SetInt("BuyedEmerald", 1);
                }
            }
        }
    }
    public void selectGoldWings()
    {
        int condition = PlayerPrefs.GetInt("BuyedGold");
        if (condition == 0)
        {
            PlayerPrefs.SetInt("BuyedGold", 0);
            PlayerPrefs.SetInt("Wings", 2);
            OnChangeRigidBody();
        }
        else if (condition == 1)
        {
            PlayerPrefs.SetInt("BuyedGold", 0);
            PlayerPrefs.SetInt("Wings", 2);
            OnChangeRigidBody();
            if (PlayerPrefs.GetInt("BuyedBlack") == 0)
            {
                PlayerPrefs.SetInt("BuyedBlack", 1);
            }
            if (PlayerPrefs.GetInt("BuyedSilver") == 0)
            {
                PlayerPrefs.SetInt("BuyedSilver", 1);
            }
            if (PlayerPrefs.GetInt("BuyedEmerald") == 0)
            {
                PlayerPrefs.SetInt("BuyedEmerald", 1);
            }
        }
        else
        {
            int coins = PlayerPrefs.GetInt("Coins");
            if (coins >= 300)
            {
                PlayerPrefs.SetInt("Coins", (coins - 300));
                coinText.text = PlayerPrefs.GetInt("Coins").ToString();
                PlayerPrefs.SetInt("BuyedGold", 0);
                PlayerPrefs.SetInt("Wings", 2);
                OnChangeRigidBody();
                if (PlayerPrefs.GetInt("BuyedBlack") == 0)
                {
                    PlayerPrefs.SetInt("BuyedBlack", 1);
                }
                if (PlayerPrefs.GetInt("BuyedSilver") == 0)
                {
                    PlayerPrefs.SetInt("BuyedSilver", 1);
                }
                if (PlayerPrefs.GetInt("BuyedEmerald") == 0)
                {
                    PlayerPrefs.SetInt("BuyedEmerald", 1);
                }
            }
        }
    }
    public void selectEmeraldWings()
    {
        int condition = PlayerPrefs.GetInt("BuyedEmerald");
        if (condition == 0)
        {
            PlayerPrefs.SetInt("BuyedEmerald", 0);
            PlayerPrefs.SetInt("Wings", 3);
            OnChangeRigidBody();
        }
        else if (condition == 1)
        {
            PlayerPrefs.SetInt("BuyedEmerald", 0);
            PlayerPrefs.SetInt("Wings", 3);
            OnChangeRigidBody();
            if (PlayerPrefs.GetInt("BuyedBlack") == 0)
            {
                PlayerPrefs.SetInt("BuyedBlack", 1);
            }
            if (PlayerPrefs.GetInt("BuyedSilver") == 0)
            {
                PlayerPrefs.SetInt("BuyedSilver", 1);
            }
            if (PlayerPrefs.GetInt("BuyedGold") == 0)
            {
                PlayerPrefs.SetInt("BuyedGold", 1);
            }
        }
        else
        {
            int coins = PlayerPrefs.GetInt("Coins");
            if (coins >= 500)
            {
                PlayerPrefs.SetInt("Coins", (coins - 500));
                coinText.text = PlayerPrefs.GetInt("Coins").ToString();
                PlayerPrefs.SetInt("BuyedEmerald", 0);
                PlayerPrefs.SetInt("Wings", 3);
                OnChangeRigidBody();
                if (PlayerPrefs.GetInt("BuyedBlack") == 0)
                {
                    PlayerPrefs.SetInt("BuyedBlack", 1);
                }
                if (PlayerPrefs.GetInt("BuyedSilver") == 0)
                {
                    PlayerPrefs.SetInt("BuyedSilver", 1);
                }
                if (PlayerPrefs.GetInt("BuyedGold") == 0)
                {
                    PlayerPrefs.SetInt("BuyedGold", 1);
                }
            }
        }
    }
}
