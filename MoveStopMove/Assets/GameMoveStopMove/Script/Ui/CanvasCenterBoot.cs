using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCenterBoot : MonoBehaviour
{
    //Button
    public Button weaponBtn;
    public Button skinBtn;
    public Button playBtn;
    //UI
    [HideInInspector] public UiManager uiManager;
    //Offset pos camera
    private Vector3 offsetSubCamera;
    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UiManager").GetComponent<UiManager>();
        offsetSubCamera = new Vector3(16.52f, 3.55f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        uiManager = GameObject.FindGameObjectWithTag("UiManager").GetComponent<UiManager>();
        playBtn.onClick.AddListener(Play);
        weaponBtn.onClick.AddListener(Weapon);
        skinBtn.onClick.AddListener(Skin);
    }
    public void Play()
    {
        gameObject.SetActive(false);
        uiManager.play = true;
    }
    public void Weapon()
    {
        uiManager.canvasWeapon.SetActive(true);
        gameObject.SetActive(false);
    }
    public void Skin()
    {
        uiManager.canvasSkinShop.SetActive(true);
        gameObject.SetActive(false);
        uiManager.subCamera01.SetActive(true);
        uiManager.subCamera01.transform.position = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position + offsetSubCamera;
        uiManager.mainCamera.SetActive(false);
    }
}
