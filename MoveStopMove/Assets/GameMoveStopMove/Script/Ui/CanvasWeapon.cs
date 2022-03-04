using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasWeapon : MonoBehaviour
{
    //Button
    public Button exitBtn;
    public Button beforeBtn;
    public Button nextBtn;
    public Button purchaseBtn;
    public Button adsBtn;
    public Image iconArrow;
    //Price
    public TextMeshProUGUI lockTxt;
    public TextMeshProUGUI priceTxt;
    public Image goldImage;
    public TextMeshProUGUI adsCountTxt;
    [HideInInspector] public bool isSeenAds;
    //
    public TextMeshProUGUI equipedTxt;
    public TextMeshProUGUI selectTxt;
    public bool isAdsClick;
    //UI
    [HideInInspector] public UiManager uiManager;
    [HideInInspector] public Player player;
    [HideInInspector] public Vector3 offsetArrow;
    // Load weapon to UI
    public ArrowSO2[] arrowList;
    public List <int> indexEquipedList;
    [HideInInspector] public int indexArrow;
    [HideInInspector] public int indexEquiped;
    [HideInInspector] public int priceArrow;
    [HideInInspector] public ArrowSO2 ArrowScriptableObjectChosen;
    // State Purcchase button
    public enum StateEquipment { onlyPurchase, equiped, notYet}
    public StateEquipment[] stateIndext;
    // Start is called before the first frame update
    void Start()
    {
        indexArrow = 0;
        uiManager = GameObject.FindGameObjectWithTag("UiManager").GetComponent<UiManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        exitBtn.onClick.AddListener(Exit);
        beforeBtn.onClick.AddListener(Before);
        nextBtn.onClick.AddListener(Next);
        purchaseBtn.onClick.AddListener(Purchase);
        adsBtn.onClick.AddListener(AdsCount);
        ArrowScriptableObjectChosen = arrowList[0];
        // offset position weapon
        offsetArrow = new Vector3(-0.23f, 0, 0);
        //
        priceArrow = arrowList[0].priceArrow;
        priceTxt.text = arrowList[0].priceArrow.ToString();
        iconArrow.sprite = arrowList[0].iconArrow;
        //
        isSeenAds = false;
        //Nemed Indext
        stateIndext = new StateEquipment[arrowList.Length];
        for (int i = 0; i < arrowList.Length; i++)
        {
            stateIndext[i] = StateEquipment.notYet;
        }
        //
        isAdsClick = false;
    }
    public void Exit()
    {
        gameObject.SetActive(false);
        uiManager.canvasCenterBoot.SetActive(true);
    }
    public void Before()
    {
        indexArrow -= 1;
        CheckStateShowUi();
    }
    public void Next()
    {
        indexArrow += 1;
        CheckStateShowUi();
    }
    
    public void CheckStateShowUi()
    {
        indexArrow = Mathf.Clamp(indexArrow, 0, arrowList.Length - 1);
        priceArrow = arrowList[indexArrow].priceArrow;
        iconArrow.sprite = arrowList[indexArrow].iconArrow;
        priceTxt.text = arrowList[indexArrow].priceArrow.ToString();
        //++
        switch (stateIndext[indexArrow])
        {
            case StateEquipment.notYet:
                SetPriceVisible();
                break;
            case StateEquipment.onlyPurchase:
                SetSelectVisible();
                break;
            case StateEquipment.equiped:
                SetEquipedVisible();
                break;
        }

    }
    //Set type UI visible
    public void SetPriceVisible()
    {
        priceTxt.gameObject.SetActive(true);
        goldImage.gameObject.SetActive(true);
        equipedTxt.gameObject.SetActive(false);
        selectTxt.gameObject.SetActive(false);
        lockTxt.gameObject.SetActive(true);
    }
    public void SetEquipedVisible()
    {
        priceTxt.gameObject.SetActive(false);
        goldImage.gameObject.SetActive(false);
        equipedTxt.gameObject.SetActive(true);
        selectTxt.gameObject.SetActive(false);
        lockTxt.gameObject.SetActive(false);
    }
    public void SetSelectVisible()
    {
        priceTxt.gameObject.SetActive(false);
        goldImage.gameObject.SetActive(false);
        equipedTxt.gameObject.SetActive(false);
        selectTxt.gameObject.SetActive(true);
        lockTxt.gameObject.SetActive(false);
    }
    public void SpawnSetInforArrow()
    {
        GameObject arrow = (GameObject)Instantiate(ArrowScriptableObjectChosen.arrowPrefabs, player.pointFire.position, player.pointFire.rotation);
        arrow.GetComponent<Arrow2>().enabled = false;
        arrow.GetComponent<RoteItself>().enabled = false;
        arrow.transform.SetParent(player.pointFire);
        arrow.transform.localPosition += offsetArrow;
        arrow.transform.transform.Rotate(0, -90, 0, Space.Self);
        if(stateIndext[indexArrow] == StateEquipment.notYet && !isAdsClick)
        {
            player.gold -= ArrowScriptableObjectChosen.priceArrow;
        }
        player.playerso.arrowPrefabs = ArrowScriptableObjectChosen.arrowPrefabs;
    }
    public void Purchase()
    {
        if (player.gold >= ArrowScriptableObjectChosen.priceArrow)
        {
            DestroySpawnEqip();
        }
    }
    public void AdsCount()
    {
        isAdsClick = true;
        if (stateIndext[indexArrow] == StateEquipment.notYet)
        {
            DestroySpawnEqip();
            adsCountTxt.text = "1/1";
        }
        isAdsClick = false;
    }
    public void DeEquiped()
    {
        for (int i = 0; i < stateIndext.Length; i++)
        {
            if(i != indexArrow && stateIndext[i] == StateEquipment.equiped)
            {
                stateIndext[i] = StateEquipment.onlyPurchase;
            }
        }
    }
    public void DestroySpawnEqip()
    {
        if (player.pointFire.childCount > 0)
        {
            Destroy(player.pointFire.GetChild(0).gameObject);
        }
        ArrowScriptableObjectChosen = arrowList[indexArrow];
        SpawnSetInforArrow();
        stateIndext[indexArrow] = StateEquipment.equiped;
        DeEquiped();
        SetEquipedVisible();
    }
}
