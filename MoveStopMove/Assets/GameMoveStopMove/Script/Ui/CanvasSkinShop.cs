using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasSkinShop : UICanvas, IInitializeVariables
{
    //Button
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button hornBtn;
    [SerializeField] private Button shortBtn;
    [SerializeField] private Button armBtn;
    [SerializeField] private Button skinBtn;
    //
    [SerializeField] private HornSO[] arrayHornSO;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject itemOfContentPrefab;
    [SerializeField] private Button purchaseBtn;
    [SerializeField] private Button adsBtn;
    //Price
    [SerializeField] private TextMeshProUGUI priceTxt;
    [SerializeField] private Image goldImage;
    [SerializeField] private TextMeshProUGUI adsCountTxt;
    private bool isSeenAds;
    //
    [SerializeField] private TextMeshProUGUI equipedTxt;
    [SerializeField] private TextMeshProUGUI selectTxt;
    [SerializeField] private GameObject lockImage;
    private bool isAdsClick;
    //UI
    [HideInInspector] public PlayerMain playerMain;
    // Load weapon to UI
    private List<int> indexEquipedList;
    private int indexHorn;
    [HideInInspector] public int indexEquiped;
    [HideInInspector] public int priceHorn;
    [HideInInspector] public HornSO hornScriptableObjectChosen;
    // State Purcchase button
    private enum StateEquipment { onlyPurchase, equiped, notYet }
    private StateEquipment[] stateIndext;
    //Color Button
    private Color choseColor;
    private Color normalColor;
    private Button[] arrButtonGroup;
    private List<Button> listBtnItem;
    //
    public List<Button> ListBtnItem { get => listBtnItem; set => listBtnItem = value; }
    public int IndexHorn { get => indexHorn; set => indexHorn = value; }
    public GameObject LockImage { get => lockImage; set => lockImage = value; }
    void OnEnable()
    {
        InitializeVariables();
        Horn();
    }
    public void Exit()
    {
        UIManager.Instance.OpenUI(UIName.CenterBoot);
        gameObject.SetActive(false);
        CameraManager.Instance.MainCameraTrans.gameObject.SetActive(true);
        CameraManager.Instance.Sub01CameraTrans.gameObject.SetActive(false);
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
            item.GetComponent<Item>().HornSOThisItem = arrayHornSO[i];
            item.GetComponent<Item>().IndexHorn = i;
            //check state because order process start and OnEnable,
            //unlock
            if (stateIndext.Length > 0)
            {
                if (stateIndext[i] == StateEquipment.equiped || stateIndext[i] == StateEquipment.onlyPurchase)
                {
                    item.GetComponent<Item>().LockImage.SetActive(false);
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
        if (playerMain.Gold >= hornScriptableObjectChosen.priceHorn)
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
        PlayerPrefs.SetInt(hornScriptableObjectChosen.nameItem, 2);// 0 = notYet, 1 = onlyPurchase, 2 = equiped
        SetEquipedVisible();
    }
  

    public void SpawnSetInforHorn()
    {
        PlayerPrefs.SetInt(hornScriptableObjectChosen.nameItem, 1);
        //Horn
        if(hornScriptableObjectChosen.nameGroup == "Horn")
        {
            DeleteChildOfParent(playerMain.HeadTras);
            DeleteChildOfParent(playerMain.BladeWearTras);
            GameObject item = Instantiate(hornScriptableObjectChosen.prefabsHorn, playerMain.HeadTras, false);
            if (stateIndext[indexHorn] == StateEquipment.notYet && !isAdsClick)
            {
                playerMain.Gold -= hornScriptableObjectChosen.priceHorn;
            }
        }
        //Short
        if(hornScriptableObjectChosen.nameGroup == "Short")
        {
            DeleteChildOfParent(playerMain.HeadTras);
            DeleteChildOfParent(playerMain.BladeWearTras);
            //set material
            var mats = new Material[playerMain.MaterialWears.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = playerMain.MaterialWears[i];
            }
            mats[0] = hornScriptableObjectChosen.materialPan;
            //Must call player.materialGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats, ( do not change if set : player.materialWears = mats;)
            playerMain.MaterialGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats;
            //
            if (stateIndext[indexHorn] == StateEquipment.notYet && !isAdsClick)
            {
                playerMain.Gold -= hornScriptableObjectChosen.priceHorn;
            }
        }
        //Arm
        if(hornScriptableObjectChosen.nameGroup == "Arm")
        {
            DeleteChildOfParent(playerMain.ShieldWearTras);
            DeleteChildOfParent(playerMain.HeadTras);
            DeleteChildOfParent(playerMain.BladeWearTras);
            GameObject item = Instantiate(hornScriptableObjectChosen.prefabsShield, playerMain.ShieldWearTras, false);
            if (stateIndext[indexHorn] == StateEquipment.notYet && !isAdsClick)
            {
                playerMain.Gold -= hornScriptableObjectChosen.priceHorn;
            }
        }
        //Skin
        if(hornScriptableObjectChosen.nameGroup == "Skin")
        {
            var mats = new Material[playerMain.MaterialWears.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = playerMain.MaterialWears[i];
            }
            mats[0] = hornScriptableObjectChosen.materialFullSet;
            //Must call player.materialGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats, ( do not change if set : player.materialWears = mats;)
            playerMain.MaterialGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats;
            if(hornScriptableObjectChosen.prefabsWing != null)
            {
                if(hornScriptableObjectChosen.prefabsWing.name == "Blade")
                {
                    DeleteChildOfParent(playerMain.HeadTras);
                    DeleteChildOfParent(playerMain.BladeWearTras);
                    GameObject item = Instantiate(hornScriptableObjectChosen.prefabsWing, playerMain.BladeWearTras, false);
                }
            }
            if (hornScriptableObjectChosen.prefabsHorn != null)
            {
                if (hornScriptableObjectChosen.prefabsHorn.name == "Hat_Thor")
                {
                    DeleteChildOfParent(playerMain.BladeWearTras);
                    DeleteChildOfParent(playerMain.HeadTras);
                    GameObject item = Instantiate(hornScriptableObjectChosen.prefabsHorn, playerMain.HeadTras, false);
                }
            }
            //
            if (stateIndext[indexHorn] == StateEquipment.notYet && !isAdsClick)
            {
                playerMain.Gold -= hornScriptableObjectChosen.priceHorn;
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
                foreach (var item in arrayHornSO)
                {
                    if (item.nameItem != hornScriptableObjectChosen.nameItem && PlayerPrefs.GetInt(item.nameItem) == 2)
                    {
                        PlayerPrefs.SetInt(item.nameItem, 1);// 0 = notYet, 1 = onlyPurchase, 2 = equiped
                    }
                }
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
    //
    public void DeleteChildOfParent(Transform _transformParentToFormat)
    {
        if (_transformParentToFormat.childCount > 0)
        {
            Transform[] items = _transformParentToFormat.gameObject.GetComponentsInChildren<Transform>();
            //Ignor first component of Content
            for (int i = 1; i < items.Length; i++)
            {
                Destroy(items[i].gameObject);
            }
        }
    }
    public void InitializeVariables()
    {
        stateIndext = new StateEquipment[arrayHornSO.Length];
        for (int i = 0; i < arrayHornSO.Length; i++)
        {
            if (PlayerPrefs.GetInt(arrayHornSO[i].nameItem) == 1)// 0 = notYet, 1 = onlyPurchase, 2 = equiped
            {
                stateIndext[i] = StateEquipment.onlyPurchase;
            }
            else if (PlayerPrefs.GetInt(arrayHornSO[i].nameItem) == 2)// 0 = notYet, 1 = onlyPurchase, 2 = equiped
            {
                stateIndext[i] = StateEquipment.equiped;
            }
            else
            {
                stateIndext[i] = StateEquipment.notYet;
            }
        }
        listBtnItem = new List<Button>();
        choseColor = new Color(1f, 1f, 1f);
        normalColor = new Color(0.8f, 0.8f, 0.8f);
        //
        playerMain = PlayerMain.Instance;
        exitBtn.onClick.AddListener(Exit);
        hornBtn.onClick.AddListener(Horn);
        shortBtn.onClick.AddListener(Short);
        armBtn.onClick.AddListener(Arm);
        skinBtn.onClick.AddListener(Skin);
        purchaseBtn.onClick.AddListener(Purchase);
        adsBtn.onClick.AddListener(AdsCount);
        //
        isAdsClick = false;
        //
        hornScriptableObjectChosen = arrayHornSO[0];
        //
        priceHorn = arrayHornSO[0].priceHorn;
        priceTxt.text = arrayHornSO[0].priceHorn.ToString();
        //
        isSeenAds = false;
        arrButtonGroup = new Button[] { hornBtn, shortBtn, armBtn, skinBtn };
        //
        nameUI = UIName.SkinShop;
    }
}
