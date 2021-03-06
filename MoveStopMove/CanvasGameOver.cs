using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CanvasGameOver : MonoBehaviour, IInitializeVariables, ISubcribers
{
    //Button
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button retryBtn;
    [SerializeField] private Button nextLevelBtn;
    //Price
    [SerializeField] private TextMeshProUGUI killedTxt;
    //
    //UI
    private PlayerMain playerMain;
    // Load weapon to UI
    public Button ExitBtn { get => exitBtn; set => exitBtn = value; }
    public Button RetryBtn { get => retryBtn; set => retryBtn = value; }
    public Button NextLevelBtn { get => nextLevelBtn; set => nextLevelBtn = value; }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Exit()
    {
        gameObject.SetActive(false);
        Retry();
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        
    }
    
    
    private void OnEnable()
    {
        InitializeVariables();
    }
    public void InitializeVariables()
    {
        playerMain = PlayerMain.Instance;
        exitBtn.onClick.AddListener(Exit);
        retryBtn.onClick.AddListener(Retry);
        nextLevelBtn.onClick.AddListener(NextLevel);
        //killedTxt.text = arrowList[0].priceArrow.ToString();
    }
}
