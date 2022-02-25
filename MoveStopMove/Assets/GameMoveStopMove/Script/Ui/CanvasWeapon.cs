using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasWeapon : MonoBehaviour
{
    //Button
    public Button exitBtn;
    //UI
    [HideInInspector] public UiManager uiManager;
    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UiManager").GetComponent<UiManager>();
        exitBtn.onClick.AddListener(Exit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        uiManager = GameObject.FindGameObjectWithTag("UiManager").GetComponent<UiManager>();
        exitBtn.onClick.AddListener(Exit);
    }
    public void Exit()
    {
        gameObject.SetActive(false);
        uiManager.canvasCenterBoot.SetActive(true);
    }
}
