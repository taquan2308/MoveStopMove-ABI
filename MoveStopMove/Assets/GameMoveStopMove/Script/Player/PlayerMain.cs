using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.AI;
public class PlayerMain : Character, IInitializeVariables
{
    #region Parameter
    public static PlayerMain Instance;
    public event Action OnAttack = delegate { };
    public event Action OnDance = delegate { };
    public event Action OnDead = delegate { };
    public event Action OnIdle = delegate { };
    public event Action OnRun = delegate { };
    public event Action OnUlti = delegate { };
    public event Action OnWin = delegate { };
    private PlayerAnimation playerAnimation;
    //-*************************
    private NavMeshAgent agent;
    private Rigidbody playerRb;
    private Vector3 playerOldPos;
    
    [SerializeField] private Joystick Joystick;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private Vector2 direction;
    [SerializeField] private Vector3 directionMove;
    //Attack
    //[HideInInspector] 
    private Transform nearestEnemyFromPlayerTrans;
    private float rangeAttack;
    private int experience;
    [SerializeField] private PlayerSO playerso;
    [SerializeField] private Transform pointFire;
    private float timeStart;
    private float timeCountdownt;
    // show Circle around Player
    private GameObject footTarget;
    //Exp Canvas
    private TextMeshProUGUI txtExp;
    private Transform canvasExpTrans;
    private bool isAddExp;
    [SerializeField] private GameObject expPrefabs;
    private Vector3 offsetPosAddExp;
    //Check first attack each time idle
    private bool isFirstAttackEveryTimeIdle;
    //Effect
    [SerializeField] private GameObject effectPrefabs;
    [SerializeField] private bool isEffect;
    //Audio
    [SerializeField] private AudioClip attackAudio;
    [SerializeField] private AudioClip dieAudio;
    private AudioSource audioSource;
    //Play Game
    [SerializeField] private bool isPlay;
    //Gold
    [SerializeField] private int gold;
    private int indexArrow;
    //index arrow to check if player have?
    private List<int> indexArrowList;
    //Position clothes
    [SerializeField] private Transform headTras;
    [SerializeField] private Transform beardTras;
    [SerializeField] private Transform underWearTras;
    [SerializeField] private Transform bladeWearTras;
    [SerializeField] private Transform shieldWearTras;
    [SerializeField] private GameObject materialGameObject;
    [SerializeField] private Material[] materialWears;
    //
    [SerializeField] private GameObject arrowObject;
    [SerializeField] private RectTransform rectCircle;
    [SerializeField] private Transform playerSubTransform;
    public int Gold { get => gold; set => gold = value;}
    public bool IsPlay { get => isPlay; set => isPlay = value;}
    public Transform HeadTras { get => headTras; set => headTras = value; }
    public Transform BeardTras { get => beardTras; set => beardTras = value; }
    public Transform UnderWearTras { get => underWearTras; set => underWearTras = value; }
    public Transform BladeWearTras { get => bladeWearTras; set => bladeWearTras = value; }
    public Transform ShieldWearTras { get => shieldWearTras; set => shieldWearTras = value; }
    public GameObject MaterialGameObject { get => materialGameObject; set => materialGameObject = value; }
    public Material[] MaterialWears { get => materialWears; set => materialWears = value; }
    public float RangeAttack { get => rangeAttack; set => rangeAttack = value; }
    public int Experience { get => experience; set => experience = value; }
    public bool IsAddExp { get => isAddExp; set => isAddExp = value; }
    public bool IsEffect { get => isEffect; set => isEffect = value; }
    public PlayerSO Playerso { get => playerso; set => playerso = value; }
    public Transform PointFire { get => pointFire; set => pointFire = value; }
    public Transform NearestEnemyFromPlayerTrans { get => nearestEnemyFromPlayerTrans; set => nearestEnemyFromPlayerTrans = value; }
    public PlayerAnimation PlayerAnimationGetterSetter { get => playerAnimation; set => playerAnimation = value; }
    public GameObject ArrowObject { get => arrowObject; set => arrowObject = value; }
    public RectTransform RectCircle { get => rectCircle; set => rectCircle = value; }
    public Transform PlayerSubTransform { get => playerSubTransform; set => playerSubTransform = value; }
    #endregion
    private void Awake()
    {
        InitializeSingleton();
    }
    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();//
    }
    private void FixedUpdate()
    {
        //cameraPos.position = transform.position + cameraPosOffset;
    }
    // Update is called once per frame
    void Update()
    {
        if (isPlay)
        {
            if (!playerAnimation.IsDead)
            {
                #region Exp,Effect,grow,DrawCircle
                //Exp
                txtExp.text = experience.ToString();
                canvasExpTrans.eulerAngles = new Vector3(0, 90, 0);
                if (isAddExp)
                {
                    isAddExp = false;
                    GameObject expAdd = (GameObject) SpawnArrow.Instance.Spawns(expPrefabs);
                    expAdd.transform.position = transform.position + offsetPosAddExp;
                    expAdd.transform.rotation = expPrefabs.transform.rotation;
                    expAdd.GetComponent<AddExp>().TxtExp.text = "+ 2";
                }
                //Effect grow
                if (isEffect)
                {
                    isEffect = false;
                    GameObject effectAdd = (GameObject) SpawnArrow.Instance.Spawns(effectPrefabs);
                    effectAdd.transform.position = transform.position;
                    effectAdd.transform.rotation = expPrefabs.transform.rotation;
                }
                // Grow
                Grow();
                //DrawCircle
                RectCircle.localScale = new Vector3(rangeAttack, rangeAttack, 1);
                #endregion
                // Move Player
                FindNearestEnemy();
                if (Joystick.Direction == Vector2.zero)
                {
                    if (!playerAnimation.IsAttack)
                    {
                        OnIdle?.Invoke();
                        LockOntarget();
                        if (timeCountdownt <= 0 && nearestEnemyFromPlayerTrans != null  && isFirstAttackEveryTimeIdle)
                        {
                            isFirstAttackEveryTimeIdle = false;
                            OnAttack?.Invoke();
                            timeCountdownt = timeStart;
                        }
                    }
                }else
                {
                    Move();
                    // Reset isFirstAttackEveryTimeIdle
                    isFirstAttackEveryTimeIdle = true;
                }
                //Debug.Log(timeCountdownt);
                #region Countdownt time to attack
                if (isFirstAttackEveryTimeIdle)
                {
                    timeCountdownt -= Time.deltaTime;
                    timeCountdownt = Mathf.Clamp(timeCountdownt, 0, Mathf.Infinity);
                    ShowArrow();
                }
                #endregion
                ShowCircleTaget();
            }
        }
        
    }
    public override void Move()
    {
        OnRun?.Invoke();
        //override the ParentClass implementation here
        direction = Joystick.Direction;
        directionMove = new Vector3( -direction.y, 0, direction.x );
        Quaternion lookRotation = Quaternion.LookRotation(directionMove);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;//Time.deltaTime * turnSpeed
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        transform.Translate(directionMove.normalized * Time.deltaTime * speed, Space.World);
    }
    public void FindNearestEnemy()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in GameManager.Instance.ListEnemy)
        {
            if (enemy != null)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
        }
        if (nearestEnemy != null && shortestDistance <= rangeAttack)
        {
            nearestEnemyFromPlayerTrans = nearestEnemy.transform;
        }
        else
        {
            nearestEnemyFromPlayerTrans = null;
        }
    }
    //
    #region Attack
    public override void Attack()
    {
        HideArrow();
        //Play Audio
        PlayAttackAudio();
        GameObject arrow2 = (GameObject) SpawnArrow.Instance.Spawns(playerso.arrowPrefabs);
        arrow2.transform.position = pointFire.position;
        arrow2.transform.rotation = playerso.arrowPrefabs.transform.rotation;
        if(nearestEnemyFromPlayerTrans != null)
        {
            arrow2.GetComponent<Arrow>().SetTaget(nearestEnemyFromPlayerTrans.position, rangeAttack, gameObject.GetInstanceID());
        }
        //Check if Arrow fire 3 direction
        if (playerso.arrowPrefabs.GetComponent<Arrow>().ArrowSO2.isThreeDirection && nearestEnemyFromPlayerTrans != null)
        {
            // pass 2 Target and fire 2 times
            // fire 2
            GameObject arrow3 = (GameObject) SpawnArrow.Instance.Spawns(playerso.arrowPrefabs);
            arrow3.transform.position = pointFire.position;
            arrow3.transform.rotation = playerso.arrowPrefabs.transform.rotation;
            Vector3 target3Position = transform.position + Quaternion.Euler(0, 45, 0) * (nearestEnemyFromPlayerTrans.position - transform.position);
            arrow3.GetComponent<Arrow>().SetTaget(target3Position, rangeAttack, gameObject.GetInstanceID());
            //
            // fire 3
            GameObject arrow4 = (GameObject) SpawnArrow.Instance.Spawns(playerso.arrowPrefabs);
            arrow4.transform.position = pointFire.position;
            arrow4.transform.rotation = playerso.arrowPrefabs.transform.rotation;
            Vector3 target4Position = transform.position + Quaternion.Euler(0, -45, 0) * (nearestEnemyFromPlayerTrans.position - transform.position);
            arrow4.GetComponent<Arrow>().SetTaget(target4Position, rangeAttack, gameObject.GetInstanceID());
            //
        }
    }
    #endregion
    public void ShowArrow()
    {
        if(arrowObject != null)
        {
            arrowObject.SetActive(true);
        }
    }
    public void HideArrow()
    {
        if (pointFire.childCount > 0)
        {
            arrowObject = pointFire.GetChild(0).gameObject;
        }
        if (arrowObject != null)
        {
            arrowObject.SetActive(false);
        }
    }
    public void LockOntarget()
    {
        // check if have enemyNearest
        if (nearestEnemyFromPlayerTrans != null)
        {
            Vector3 dirPlayerToEnemy = nearestEnemyFromPlayerTrans.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dirPlayerToEnemy);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }
    //Grow
    public void Grow()
    {
        if(experience > 2)
        {
            playerSubTransform.localScale = new Vector3(1 + experience / 2 * 0.1f, 1 + experience / 2 * 0.1f, 1 + experience / 2 * 0.1f);// buff 10% Scale
        }
    }
    
    public void PlayAttackAudio()
    {
        audioSource.PlayOneShot(attackAudio);
    }
    public void PlayDieAudio()
    {
        audioSource.PlayOneShot(dieAudio);
    }
    //
    IEnumerator DelayDie()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
    public void DieLater()
    {
        GameManager.Instance.FootTarget.gameObject.SetActive(false);
        gameObject.tag = "Untagged";
        OnDead?.Invoke();
        StartCoroutine("DelayDie");
    }
    private void InitializeSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void ShowCircleTaget()
    {
        #region Circle foot
        //Circle foot
        if (nearestEnemyFromPlayerTrans != null)
        {
            footTarget.transform.position = nearestEnemyFromPlayerTrans.position;
            footTarget.SetActive(true);
        }
        else
        {
            footTarget.SetActive(false);
        }
        #endregion
    }
    #region InitializeVariables
    public void InitializeVariables()
    {
        agent = GetComponent<NavMeshAgent>();
        playerRb = GetComponent<Rigidbody>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        offsetPosAddExp = new Vector3(0, 4, 0);
        
        playerOldPos = transform.position;
        //attack
        rangeAttack = playerso.rangeAttack;
        experience = playerso.experience;
        //Countdownt attack
        timeStart = playerso.speedAttack;
        timeCountdownt = 0;
        turnSpeed = playerso.turnSpeed;
        // circle on foot
        footTarget = GameManager.Instance.FootTarget;
        footTarget.SetActive(false);
        //Canvas Exp
        txtExp = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        canvasExpTrans = gameObject.GetComponentsInChildren<Transform>()[2];// Because know order of Transform Child
        isAddExp = false;
        //Check first attack each time idle
        isFirstAttackEveryTimeIdle = true;
        //
        isEffect = false;
        //Audio
        audioSource = GetComponent<AudioSource>();
        //
        gold = playerso.gold;
        //Set no arrow at start game
        indexArrow = -1;
        speed = 3;
        materialWears = materialGameObject.GetComponent<SkinnedMeshRenderer>().materials;
        isPlay = GameManager.Instance.IsPlay;
        if (pointFire.childCount > 0)
        {
            arrowObject = pointFire.GetChild(0).gameObject;
        }
        playerSubTransform = GetComponentsInChildren<Transform>()[1];
    }
    #endregion
}
