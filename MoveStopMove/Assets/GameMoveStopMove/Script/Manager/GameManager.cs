using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : Singleton<GameManager>, IInitializeVariables, ISubcribers
{
    //GameManager-LevelManager-OpenLevel-TryGame-OverGame-Score-level-ListEnemy - tagetFrame;
    public event Action<int> OnGameStarted = delegate { };
    public event Action OnLevelCompleted = delegate { };
    public event Action OnGameOver = delegate { };
    public static int sceneIndex = 0;
    private PlayerMain playerMain;
    private int curentLevel;
    private bool gameStarted;
    private bool levelCompleted;
    private bool gameOver;
    private int currentLevel;
    [SerializeField] private List<GameObject> listEnemy;
    [SerializeField] private List<GameObject> listObstacle;
    public List<GameObject> ListEnemy { get => listEnemy;}
    public List<GameObject> ListObstacle { get => listObstacle; }
    //private enum GameState { gameStarted, gameComplete, gameOver}
    //**********************************************************************
    [SerializeField] private Transform NearestEnemyFromPlayerTrans;
    private bool hasNearestEnemyOfPlayer;
    //Canvas Game Over
    [SerializeField] private GameObject footTarget;
    private bool isPlay;
    private int killedCount;
    public bool IsPlay { get => isPlay; set => isPlay = value; }
    public GameObject FootTarget { get => footTarget; set => footTarget = value; }
    public bool GameStarted { get => gameStarted; set => gameStarted = value; }
    public bool LevelCompleted { get => levelCompleted; set => levelCompleted = value; }
    public bool GameOver { get => gameOver; set => gameOver = value; }
    public int KilledCount { get => killedCount; set => killedCount = value; }
    private void Awake()
    {
        listEnemy = new List<GameObject>();
        SubscribeEvent();
        //InitializeVariables();//
    }
    private void OnEnable()
    {
        
    }
    void Start()
    {
        InitializeVariables();//
        
    }

    // Update is called once per frame
    void Update()
    {
        GameState();
    }
    private void GameState()
    {
        if (gameStarted)
        {
            gameStarted = false;
            OnGameStarted?.Invoke(currentLevel);
        }

        if (levelCompleted)
        {
            levelCompleted = false;
            OnLevelCompleted?.Invoke();
        }

        if (gameOver || listEnemy.Count == 0)
        {
            gameOver = false;
            footTarget.SetActive(false);
            OnGameOver?.Invoke();
        }
    }
    public void NextLevel()
    {
        sceneIndex++;
        if (sceneIndex > SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }
    public void InitializeVariables()
    {
        Application.targetFrameRate = 60;
        playerMain = PlayerMain.Instance;
        NearestEnemyFromPlayerTrans = playerMain.NearestEnemyFromPlayerTrans;//
        hasNearestEnemyOfPlayer = false;
        // canvas
        isPlay = false;
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        gameStarted = false;
        levelCompleted = false;
        gameOver = false;
        killedCount = 0;
        //listEnemy = new List<GameObject>();
    }

    public void SubscribeEvent()
    {
        OnLevelCompleted += NextLevel;
    }

    public void UnSubscribeEvent()
    {
        OnLevelCompleted -= NextLevel;
    }
}
