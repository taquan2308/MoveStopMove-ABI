using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Button button;
    public HornSO hornSOThisItem;
    [HideInInspector] public CanvasSkinShop canvasSkinShop;
    [HideInInspector] public Player player;
    [HideInInspector] public int indexHorn;
    public GameObject lockImage;
    //
    public Color choseColor;
    public Color normalColor;
    private void Awake()
    {
        choseColor = new Color(1f, 1f, 1f);
        normalColor = new Color(0.8f, 0.8f, 0.8f);
    }
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ClickBtn);
        canvasSkinShop = GameObject.FindGameObjectWithTag("CanvasSkinShop").GetComponent<CanvasSkinShop>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        lockImage = gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ClickBtn()
    {
        canvasSkinShop = GameObject.FindGameObjectWithTag("CanvasSkinShop").GetComponent<CanvasSkinShop>();
        foreach (Button btn in canvasSkinShop.listBtnItem)
        {
            if (btn != null)
            {
                if (btn != this.button)
                {
                    var colors1 = btn.colors;
                    colors1.normalColor = normalColor;
                    btn.colors = colors1;
                }
                else
                {
                    var colors1 = btn.colors;
                    colors1.normalColor = choseColor;
                    btn.colors = colors1;
                }
            }
        }
        canvasSkinShop.SetPriceTxt(hornSOThisItem.priceHorn);
        canvasSkinShop.hornScriptableObjectChosen = hornSOThisItem;
        canvasSkinShop.indexHorn = indexHorn;
        canvasSkinShop.CheckStateShowUi();
        canvasSkinShop.lockImage = lockImage;
    }
}
