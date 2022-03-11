using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCenterBoot : MonoBehaviour, IInitializeVariables
{
    private PlayerMain playerMain;
    [SerializeField] private Button weaponBtn;
    [SerializeField] private Button skinBtn;
    [SerializeField] private Button playBtn;
    private Vector3 offsetSubCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        InitializeVariables();
    }
    public void Play()
    {
        if(playerMain == null)
        {
            playerMain = PlayerMain.Instance;
        }
        playerMain.IsPlay = true;
        gameObject.SetActive(false);
        GameManager.Instance.GameStarted = true;
        foreach (var enemy in GameManager.Instance.ListEnemy)
        {
            if (enemy != null)
            {
                enemy.GetComponent<EnemyMain>().IsPlay = true;
            }
        }
    }
    public void Weapon()
    {
        UiManager.Instance.CanvasWeapon.SetActive(true);
        gameObject.SetActive(false);
    }
    public void Skin()
    {
        UiManager.Instance.CanvasSkinShop.SetActive(true);
        gameObject.SetActive(false);
        UiManager.Instance.SubCamera01.SetActive(true);
        UiManager.Instance.SubCamera01.transform.position = playerMain.transform.position + offsetSubCamera;
        UiManager.Instance.MainCamera.SetActive(false);
    }

    public void InitializeVariables()
    {
        playerMain = PlayerMain.Instance;
        offsetSubCamera = new Vector3(16.52f, 3.55f, 0);
        playBtn.onClick.AddListener(Play);
        weaponBtn.onClick.AddListener(Weapon);
        skinBtn.onClick.AddListener(Skin);
    }
}
