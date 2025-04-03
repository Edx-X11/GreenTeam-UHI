using System;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    bool playerReady;
    bool initReadyScreen;
    bool isGameOver;

    int playerScore;
    float playerReadyDelay = 3f;
    float playerReadyTime;
    public float gameRestartTime;
    public float gameRestartDelay = 5f;

    TextMeshProUGUI screenMessageText;
    TextMeshProUGUI screenPlayerScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerReady)
        {
            if (initReadyScreen) //displays "READY" on screen for 3 seconds when scene is loaded
            {
                screenMessageText.alignment = TextAlignmentOptions.Center;
                screenMessageText.alignment = TextAlignmentOptions.Top;
                screenMessageText.fontStyle = FontStyles.UpperCase;
                screenMessageText.fontSize = 24;
                screenMessageText.text = "READY";
                initReadyScreen = false;
            }
            playerReadyTime -= Time.deltaTime;
            if (playerReadyTime < 0) //after 3 seconds removes "READY" text
            {
                screenMessageText.text = "";
                playerReady = false;
            }
            return;
        }
        if (screenPlayerScore != null) //sets score to 0 and changes based on playerScore parameter
        {
            screenPlayerScore.text = String.Format("<mspace=\"{0}\">{1:00000000}</mspace>", screenPlayerScore.fontSize, playerScore);
        }
        if(!isGameOver)
        {
            //if game is running do stuff here
        }
        else //if game is over reloads the scene after 5 seconds
        {
            gameRestartTime -= Time.deltaTime;
            if(gameRestartTime < 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartGame();
    }

    void StartGame()
    {
        isGameOver = false;
        initReadyScreen = true;
        playerReady = true;
        playerReadyTime = playerReadyDelay;
        screenPlayerScore = GameObject.Find("PlayerScore").GetComponent<TextMeshProUGUI>();
        screenMessageText = GameObject.Find("ReadyText").GetComponent<TextMeshProUGUI>();
    }

    public void AddScore(int points)
    {
        playerScore += points;
    }

    public void PlayerDied()
    {
        isGameOver = true;
        gameRestartTime = gameRestartDelay;

    }
}
