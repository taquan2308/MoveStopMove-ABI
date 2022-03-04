using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasSkinShop : MonoBehaviour
{
    //UI
    [HideInInspector] public UiManager uiManager;
    //Button
    public Button exitBtn;
    public Button hornBtn;
    public Button shortBtn;
    public Button armBtn;
    public Button skinBtn;
    //
    public HornSO[] arrayHornSO;
    public Transform content;
    public GameObject itemOfContentPrefab;
    public Button purchaseBtn;
    public Button adsBtn;
    //Price
    public TextMeshProUGUI priceTxt;
    public Image goldImage;
    public TextMeshProUGUI adsCountTxt;
    [HideInInspector] public bool isSeenAds;
    //
    public TextMeshProUGUI equipedTxt;
    public TextMeshProUGUI selectTxt;
    public GameObject lockImage;
    public bool isAdsClick;
    //UI
    [HideInInspector] public Player player;
    // Load weapon to UI
    public List<int> indexEquipedList;
    [HideInInspector] public int indexHorn;
    [HideInInspector] public int indexEquiped;
    [HideInInspector] public int priceHorn;
    [HideInInspector] public HornSO hornScriptableObjectChosen;
    // State Purcchase button
    public enum StateEquipment { onlyPurchase, equiped, notYet }
    public StateEquipment[] stateIndext;
    //Color Button
    public Color choseColor;
    public Color normalColor;
    public Button[] arrButtonGroup;
    public List<Button> listBtnItem;
    private void Awake()
    {
        stateIndext = new StateEquipment[arrayHornSO.Length];
        for (int i = 0; i < arrayHornSO.Length; i++)
        {
            stateIndext[i] = StateEquipment.notYet;
        }
        listBtnItem = new List<Button>();
        choseColor = new Color(1f, 1f, 1f);
        normalColor = new Color(0.8f, 0.8f, 0.8f);
    }
    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UiManager").GetComponent<UiManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        exitBtn.onClick.AddListener(Exit);
        hornBtn.onClick.AddListener(Horn);
        shortBtn.onClick.AddListener(Short);
        armBtn.onClick.AddListener(Arm);
        skinBtn.onClick.AddListener(Skin);
        purchaseBtn.onClick.AddListener(Purchase);
        adsBtn.onClick.AddListener(AdsCount);
        //
        stateIndext = new StateEquipment[arrayHornSO.Length];
        isAdsClick = false;
        //
        hornScriptableObjectChosen = arrayHornSO[0];
        //
        priceHorn = arrayHornSO[0].priceHorn;
        priceTxt.text = arrayHornSO[0].priceHorn.ToString();
        //
        isSeenAds = false;
        //Nemed Indext
        stateIndext = new StateEquipment[arrayHornSO.Length];
        for (int i = 0; i < arrayHornSO.Length; i++)
        {
            stateIndext[i] = StateEquipment.notYet;
        }
        //
        arrButtonGroup = new Button[] { hornBtn,shortBtn,armBtn,skinBtn};
        choseColor = new Color(1f, 1f, 1f);
        normalColor = new Color(0.8f, 0.8f, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        Horn();
    }
    public void Exit()
    {
        uiManager.canvasCenterBoot.SetActive(true);
        gameObject.SetActive(false);
        uiManager.mainCamera.SetActive(true);
        uiManager.subCamera01.SetActive(false);
    }
    public void SetColorBtn(Button _nameBtn)
    {
        foreach (Button btn in arrButtonGroup)
        {
            if(btn != _nameBtn)
            {
                var colors1 = btn.colors;
                colors1.normalColor = normalColor;
                colors1.selectedColor = normalColor;
                btn.colors = colors1;
            }
            else
            {
                var colors1 = btn.colors;
                colors1.normalColor = choseColor;
                colors1.selectedColor = choseColor;
                btn.colors = colors1;
            }
        }
    }
    public void Horn()
    {
        SetColorBtn(hornBtn);
        LoadItemOnGroup("Horn");
    }
    public void Short()
    {
        SetColorBtn(shortBtn);
        LoadItemOnGroup("Short");
    }
    public void Arm()
    {
        SetColorBtn(armBtn);
        LoadItemOnGroup("Arm");
    }
    public void Skin()
    {
        SetColorBtn(skinBtn);
        LoadItemOnGroup("Skin");
    }
    public void LoadItemOnGroup(string _nameGroup)
    {
        listBtnItem.Clear();
        if (content.childCount > 0)
        {
            RectTransform[] items = content.gameObject.GetComponentsInChildren<RectTransform>();
            //Ignor first component of Content
            for (int i = 1; i < items.Length; i++)
            {
                Destroy(items[i].gameObject);
            }
        }
        bool isFirstItemInGroup = true;
        
        for (int i = 0; i < arrayHornSO.Length; i++)
        {
            
            GameObject item = Instantiate(itemOfContentPrefab, content, false);
            item.GetComponent<Image>().sprite = arrayHornSO[i].iconHorn;
            item.GetComponent<Item>().hornSOThisItem = arrayHornSO[i];
            item.GetComponent<Item>().indexHorn = i;
            //check state because order process start and OnEnable,
            //unlock
            if (stateIndext.Length > 0)
            {
                if (stateIndext[i] == StateEquipment.equiped || stateIndext[i] == StateEquipment.onlyPurchase)
                {
                    item.GetComponent<Item>().lockImage.SetActive(false);
                }
            }
            
            //destroy button of other group
            listBtnItem.Add(item.GetComponent<Button>());
            if (arrayHornSO[i].nameGroup != _nameGroup)
            {
                Destroy(item);
            }else if (isFirstItemInGroup)
            {
                isFirstItemInGroup = false;
                item.GetComponent<Item>().ClickBtn();

                var colors1 = item.GetComponent<Button>().colors;
                colors1.normalColor = choseColor;
                item.GetComponent<Button>().colors = colors1;
            }

        }
    }
    public void SetPriceTxt(int _price)
    {
        priceHorn = _price;
        priceTxt.text = priceHorn.ToString();
    }
    
    public void CheckStateShowUi()
    {
        if (hornScriptableObjectChosen != null)
        {
            priceHorn = hornScriptableObjectChosen.priceHorn;
            priceTxt.text = priceHorn.ToString();
            switch (stateIndext[indexHorn])
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
    }
    public void AdsCount()
    {
        if (stateIndext[indexHorn] == StateEquipment.notYet && !isAdsClick)
        {
            DestroySpawnEqip();
            adsCountTxt.text = "1/1";
            lockImage.SetActive(false);
        }
        isAdsClick = true;
    }
    public void Purchase()
    {
        if (player.gold >= hornScriptableObjectChosen.priceHorn)
        {
            DestroySpawnEqip();
            lockImage.SetActive(false);
        }
    }
    public void DestroySpawnEqip()
    {
        SpawnSetInforHorn();
        stateIndext[indexHorn] = StateEquipment.equiped;
        DeEquiped();
        SetEquipedVisible();
    }
  

    public void SpawnSetInforHorn()
    {
        //Horn
        if(hornScriptableObjectChosen.nameGroup == "Horn")
        {
            if (player.headTras.childCount > 0)
            {
                Transform[] items = player.headTras.gameObject.GetComponentsInChildren<Transform>();
                //Ignor first component of Content
                for (int i = 1; i < items.Length; i++)
                {
                    Destroy(items[i].gameObject);
                }
            }
            GameObject item = Instantiate(hornScriptableObjectChosen.prefabsHorn, player.headTras, false);
            if (stateIndext[indexHorn] == StateEquipment.notYet && !isAdsClick)
            {
                player.gold -= hornScriptableObjectChosen.priceHorn;
            }
        }
        //Short
        if(hornScriptableObjectChosen.nameGroup == "Short")
        {
            //set material
            var mats = new Material[player.materialWears.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = player.materialWears[i];
            }
            mats[0] = hornScriptableObjectChosen.materialPan;
            //Must call player.materialGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats, ( do not change if set : player.materialWears = mats;)
            player.materialGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats;
            //
            if (stateIndext[indexHorn] == StateEquipment.notYet && !isAdsClick)
            {
                player.gold -= hornScriptableObjectChosen.priceHorn;
            }
        }
        //Arm
        if(hornScriptableObjectChosen.nameGroup == "Arm")
        {
            if (player.headTras.childCount > 0)
            {
                Transform[] items = player.shieldWearTras.gameObject.GetComponentsInChildren<Transform>();
                //Ignor first component of Content
                for (int i = 1; i < items.Length; i++)
                {
                    Destroy(items[i].gameObject);
                }
            }
            GameObject item = Instantiate(hornScriptableObjectChosen.prefabsShield, player.shieldWearTras, false);
            if (stateIndext[indexHorn] == StateEquipment.notYet && !isAdsClick)
            {
                player.gold -= hornScriptableObjectChosen.priceHorn;
            }
        }
        //Skin
        if(hornScriptableObjectChosen.nameGroup == "Skin")
        {
            var mats = new Material[player.materialWears.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = player.materialWears[i];
            }
            mats[0] = hornScriptableObjectChosen.materialFullSet;
            //Must call player.materialGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats, ( do not change if set : player.materialWears = mats;)
            player.materialGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats;
            if(hornScriptableObjectChosen.prefabsWing != null)
            {
                if(hornScriptableObjectChosen.prefabsWing.name == "Blade")
                {
                    if (player.bladeWearTras.childCount > 0)
                    {
                        Transform[] items = player.shieldWearTras.gameObject.GetComponentsInChildren<Transform>();
                        //Ignor first component of Content
                        for (int i = 1; i < items.Length; i++)
                        {
                            Destroy(items[i].gameObject);
                        }
                    }
                    GameObject item = Instantiate(hornScriptableObjectChosen.prefabsWing, player.bladeWearTras, false);
                }
            }
            if (hornScriptableObjectChosen.prefabsHorn != null)
            {
                if (hornScriptableObjectChosen.prefabsHorn.name == "Hat_Thor")
                {
                    if (player.bladeWearTras.childCount > 0)
                    {
                        Transform[] items = player.bladeWearTras.gameObject.GetComponentsInChildren<Transform>();
                        //Ignor first component of Content
                        for (int i = 1; i < items.Length; i++)
                        {
                            Destroy(items[i].gameObject);
                        }
                    }
                    if (player.headTras.childCount > 0)
                    {
                        Transform[] items = player.headTras.gameObject.GetComponentsInChildren<Transform>();
                        //Ignor first component of Content
                        for (int i = 1; i < items.Length; i++)
                        {
                            Destroy(items[i].gameObject);
                        }
                    }
                    GameObject item = Instantiate(hornScriptableObjectChosen.prefabsHorn, player.headTras, false);
                }
            }
            //
            if (stateIndext[indexHorn] == StateEquipment.notYet && !isAdsClick)
            {
                player.gold -= hornScriptableObjectChosen.priceHorn;
            }
        }
    }
    public void DeEquiped()
    {
        for (int i = 0; i < stateIndext.Length; i++)
        {
            if (i != indexHorn && stateIndext[i] == StateEquipment.equiped)
            {
                stateIndext[i] = StateEquipment.onlyPurchase;
            }
        }
    }
    
    //Set type UI visible
    public void SetPriceVisible()
    {
        priceTxt.gameObject.SetActive(true);
        goldImage.gameObject.SetActive(true);
        equipedTxt.gameObject.SetActive(false);
        selectTxt.gameObject.SetActive(false);
    }
    public void SetEquipedVisible()
    {
        priceTxt.gameObject.SetActive(false);
        goldImage.gameObject.SetActive(false);
        equipedTxt.gameObject.SetActive(true);
        selectTxt.gameObject.SetActive(false);
    }
    public void SetSelectVisible()
    {
        priceTxt.gameObject.SetActive(false);
        goldImage.gameObject.SetActive(false);
        equipedTxt.gameObject.SetActive(false);
        selectTxt.gameObject.SetActive(true);
    }
}
