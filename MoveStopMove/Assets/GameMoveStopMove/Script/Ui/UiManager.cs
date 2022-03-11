using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : Singleton<UiManager>, IInitializeVariables, ISubcribers
{
    // Canvas Object
    [SerializeField] private GameObject canvasRightTop;
    [SerializeField] private GameObject canvasCenterBoot;
    [SerializeField] private GameObject canvasWeapon;
    [SerializeField] private GameObject canvasSkinShop;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject subCamera01;
    [SerializeField] private TextMeshProUGUI txtLive;
    [SerializeField] private TextMeshProUGUI txtLevel;
    [SerializeField] private GameObject canvasGameOver;
    public GameObject CanvasRightTop { get => canvasRightTop; set => canvasRightTop = value;}
    public GameObject CanvasCenterBoot { get => canvasCenterBoot; set => canvasCenterBoot = value;}
    public GameObject CanvasWeapon { get => canvasWeapon; set => canvasWeapon = value;}
    public GameObject CanvasSkinShop { get => canvasSkinShop; set => canvasSkinShop = value;}
    public GameObject MainCamera { get => mainCamera; set => mainCamera = value;}
    public GameObject SubCamera01 { get => subCamera01; set => subCamera01 = value;}
    public TextMeshProUGUI TxtLive { get => txtLive; set => txtLive = value;}
    public GameObject CanvasGameOver { get => canvasGameOver; set => canvasGameOver = value; }
    //Show count enemy
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        SubscribeEvent();
    }
    private void Update()
    {
        
    }
    private void OnEnable()
    {
        InitializeVariables();
    }

    public void InitializeVariables()
    {
        canvasRightTop.SetActive(true);
        canvasCenterBoot.SetActive(true);
        canvasWeapon.SetActive(false);
        canvasSkinShop.SetActive(false);
        canvasSkinShop.SetActive(false);
        canvasGameOver.SetActive(false);
    }
    public void UpdateAlives()
    {
        txtLive.text = "Alive: " + GameManager.Instance.ListEnemy.Count.ToString();
    }
    public void ShowCanvasGameOver()
    {
        canvasGameOver.SetActive(true);
    }
    public void GameStarted(int currentLevel)
    {
        txtLevel.text = "LEVEL " + currentLevel.ToString();
    }
    public void SubscribeEvent()
    {
        GameManager.Instance.OnGameStarted += GameStarted;
        GameManager.Instance.OnGameOver += ShowCanvasGameOver;
    }

    public void UnSubscribeEvent()
    {
        GameManager.Instance.OnGameStarted -= GameStarted;
        GameManager.Instance.OnGameOver -= ShowCanvasGameOver;
    }
}
