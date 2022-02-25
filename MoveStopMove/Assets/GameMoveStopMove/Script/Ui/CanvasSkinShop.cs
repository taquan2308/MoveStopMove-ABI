using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSkinShop : MonoBehaviour
{
    //UI
    [HideInInspector] public UiManager uiManager;
    //Button
    public Button exitBtn;
    public Button hornBtn;
    public Button shortBtn;
    public Button armtBtn;
    public Button skinBtn;
    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UiManager").GetComponent<UiManager>();
        exitBtn.onClick.AddListener(Exit);
        hornBtn.onClick.AddListener(Horn);
        shortBtn.onClick.AddListener(Short);
        shortBtn.onClick.AddListener(Arm);
        skinBtn.onClick.AddListener(Skin);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        uiManager = GameObject.FindGameObjectWithTag("UiManager").GetComponent<UiManager>();
        exitBtn.onClick.AddListener(Exit);
        hornBtn.onClick.AddListener(Horn);
        shortBtn.onClick.AddListener(Short);
        shortBtn.onClick.AddListener(Arm);
        skinBtn.onClick.AddListener(Skin);
    }
    public void Exit()
    {
        uiManager.canvasCenterBoot.SetActive(true);
        gameObject.SetActive(false);
        uiManager.mainCamera.SetActive(true);
        uiManager.subCamera01.SetActive(false);
    }
    public void Horn()
    {
        
    }
    public void Short()
    {
        
    }
    public void Arm()
    {
        
    }
    public void Skin()
    {
       
    }
}
