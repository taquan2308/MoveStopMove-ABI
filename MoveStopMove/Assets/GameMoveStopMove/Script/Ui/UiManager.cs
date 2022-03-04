using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    // Canvas Object
    public GameObject canvasRightTop;
    public GameObject canvasCenterBoot;
    public GameObject canvasWeapon;
    public GameObject canvasSkinShop;
    public GameObject mainCamera;
    public GameObject subCamera01;
    [HideInInspector]public bool play;
    // Start is called before the first frame update
    void Start()
    {
        canvasRightTop.SetActive(true);
        canvasCenterBoot.SetActive(true);
        canvasWeapon.SetActive(false);
        canvasSkinShop.SetActive(false);
        subCamera01.SetActive(false);
        play = false;
    }
    private void OnEnable()
    {
        canvasRightTop.SetActive(false);
        canvasCenterBoot.SetActive(false);
        canvasWeapon.SetActive(false);
        canvasSkinShop.SetActive(false);
        subCamera01.SetActive(false);
    }
}
