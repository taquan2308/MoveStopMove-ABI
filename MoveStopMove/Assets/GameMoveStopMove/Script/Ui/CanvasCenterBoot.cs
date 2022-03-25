using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCenterBoot : UICanvas, IInitializeVariables
{
    private PlayerMain playerMain;
    [SerializeField] private Button weaponBtn;
    [SerializeField] private Button skinBtn;
    [SerializeField] private Button playBtn;
    private Vector3 offsetSubCamera;
    private Animator animCanvsCenterBoot;
    private void OnEnable()
    {
        InitializeVariables();
    }
    public void Play()
    {
        StartCoroutine(DelayPlay());
    }
    public void Weapon()
    {
        UIManager.Instance.OpenUI(UIName.WeaponChose);
        gameObject.SetActive(false);
    }
    public void Skin()
    {
        UIManager.Instance.OpenUI(UIName.SkinShop);
        gameObject.SetActive(false);
        PlayerMain.Instance.PlayerAnimationGetterSetter.PlayDanceAnim();
        CameraManager.Instance.Sub01CameraGameObj.SetActive(true);
        CameraManager.Instance.Sub01CameraTrans.position = playerMain.transform.position + offsetSubCamera;
        CameraManager.Instance.MainCameraGameObj.SetActive(false);
    }

    IEnumerator DelayPlay()
    {
        animCanvsCenterBoot.SetTrigger("Out");
        yield return new WaitForSeconds(1);
        if (playerMain == null)
        {
            playerMain = PlayerMain.Instance;
        }
        playerMain.IsPlay = true;
        gameObject.SetActive(false);
        GameManager.Instance.GameStarted = true;
        UIManager.Instance.OpenUI(UIName.Joystick);
        UIManager.Instance.CloseUI(UIName.RightTop);
        foreach (var enemy in GameManager.Instance.ListEnemy)
        {
            if (enemy != null)
            {
                enemy.GetComponent<EnemyMain>().IsPlay = true;
            }
        }
        UIManager.Instance.OpenUI(UIName.Joystick);
    }
     public void SetAnimationOut()
    {
        animCanvsCenterBoot.SetTrigger("Out");
    }
    public void InitializeVariables()
    {
        playerMain = PlayerMain.Instance;
        offsetSubCamera = new Vector3(16.52f, 3.55f, 0);
        playBtn.onClick.AddListener(Play);
        weaponBtn.onClick.AddListener(Weapon);
        skinBtn.onClick.AddListener(Skin);
        nameUI = UIName.CenterBoot;
        animCanvsCenterBoot = GetComponent<Animator>();
        animCanvsCenterBoot.SetTrigger("In");
    }
}
