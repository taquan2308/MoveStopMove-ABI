using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.AI;
public class Player : Character
{
    private NavMeshAgent agent;
    private Rigidbody playerRb;
    public Transform cameraPos;
    [HideInInspector] public Vector3 playerOldPos;
    [HideInInspector] public Vector3 cameraPosOffset;
    private GameObject[] enemys;
    [SerializeField] private Joystick Joystick;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private bool isRun;
    [SerializeField] private Vector2 direction;
    [SerializeField] private Vector3 directionMove;
    private float horizontalInput;
    private float forwardInput;
    Animator animator;
    //check if Move
    private bool isMove;
    //Attack
    //[HideInInspector] 
    [HideInInspector] public Transform NearestEnemyFromPlayerTrans;
    public float rangeAttack;
    public int experience;
    public PlayerSO playerso;
    public Transform pointFire;
    public float timeStart;
    public float timeCountdownt;
    // show Circle around Player
    [HideInInspector] public DrawCircle drawCircle;
    public GameObject cirleObjectCanvas;
    public GameManager2 gameManager2;
    //Exp Canvas
    [HideInInspector] public TextMeshProUGUI txtExp;
    [HideInInspector] public Transform canvasExpTrans;
    [HideInInspector] public bool isAddExp;
    public GameObject expPrefabs;
    [HideInInspector] public Vector3 offsetPosAddExp;
    //Animation Player
    public Animator playerAni;
    //Check State Player
    [HideInInspector]
    public enum StatePlayer
    {
        isAttack,
        isDance,
        isDead,
        isIdle,
        isRun,
        isUlti,
        isWin
    }
    //
    [HideInInspector] public bool isAttack;
    [HideInInspector] public bool isDance;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isIdle;
    [HideInInspector] public bool isRunAnim;
    [HideInInspector] public bool isUlti;
    [HideInInspector] public bool isWin;
    //
    //Check first attack each time idle
    [HideInInspector] public bool isFirstAttackEveryTimeIdle;
    //Effect
    public GameObject effectPrefabs;
    public bool isEffect;
    //Audio
    public AudioClip attackAudio;
    public AudioClip dieAudio;
    [HideInInspector] public AudioSource audioSource;
    //Play Game
    [HideInInspector] public bool play;
    [HideInInspector] public UiManager uiManager;
    //Gold
     public int gold;
    [HideInInspector] public int indexArrow;
    //index arrow to check if player have?
    [HideInInspector] public List<int> indexArrowList;
    //Position clothes
    public Transform headTras;
    public Transform beardTras;
    public Transform underWearTras;
    public Transform bladeWearTras;
    public Transform shieldWearTras;
    public GameObject materialGameObject;
    [HideInInspector] public Material[] materialWears;
    //
    public GameObject arrowObject;
    private void Awake()
    {
        drawCircle = GetComponent<DrawCircle>();
        agent = GetComponent<NavMeshAgent>();
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cameraPosOffset = new Vector3(15f, 15f, 0);
        offsetPosAddExp = new Vector3(0, 4, 0);
        cameraPos.position = transform.position + cameraPosOffset;
        playerOldPos = transform.position;
        //attack
        rangeAttack = playerso.rangeAttack;
        experience = playerso.experience;
        //Countdownt attack
        timeStart = playerso.speedAttack;
        timeCountdownt = 0;
        // initialization 
        isMove = false;
        //
        turnSpeed = playerso.turnSpeed;
        // circle on foot
        cirleObjectCanvas.SetActive( false );
        //Canvas Exp
        txtExp = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        canvasExpTrans = gameObject.GetComponentsInChildren<Transform>()[1];// Because know order of Transform Child
        isAddExp = false;
        // Set State
        SetState(StatePlayer.isIdle);
        //
        isAttack = false;
        isDance = false;
        isDead = false;
        isIdle = true;
        isRunAnim = false;
        isUlti = false;
        isWin = false;
        //Check first attack each time idle
        isFirstAttackEveryTimeIdle  = true;
        //
        isEffect = false;
        //Audio
        audioSource = GetComponent<AudioSource>();
        //
        gold = playerso.gold;
        //Set no arrow at start game
        indexArrow = -1;
        speed = 3;
    }
    // Start is called before the first frame update
    void Start()
    {
        cameraPos.position = transform.position + cameraPosOffset;
        play = false;
        uiManager = GameObject.FindGameObjectWithTag("UiManager").GetComponent<UiManager>();
        //get material
        materialWears = materialGameObject.GetComponent<SkinnedMeshRenderer>().materials;
    }
    private void FixedUpdate()
    {
        cameraPos.position = transform.position + cameraPosOffset;
    }
    // Update is called once per frame
    void Update()
    {
        play = uiManager.play;
        if (play)
        {
            if (!isDead)
            {
                //Exp
                txtExp.text = experience.ToString();
                canvasExpTrans.eulerAngles = new Vector3(0, 90, 0);//.Rotate(0, -90, 0,Space.World);
                if (isAddExp)
                {
                    isAddExp = false;
                    GameObject expAdd = GameObject.FindGameObjectWithTag("SpawArrow").GetComponent<SpawnArrow>().Spawns(expPrefabs);
                    expAdd.transform.position = transform.position + offsetPosAddExp;
                    expAdd.transform.rotation = expPrefabs.transform.rotation;
                    expAdd.GetComponent<AddExp>().txtExp.text = "+ 2";
                }
                //Effect grow
                if (isEffect)
                {
                    isEffect = false;
                    GameObject effectAdd = GameObject.FindGameObjectWithTag("SpawArrow").GetComponent<SpawnArrow>().Spawns(effectPrefabs);
                    effectAdd.transform.position = transform.position;
                    effectAdd.transform.rotation = expPrefabs.transform.rotation;
                }
                // Grow
                Grow();
                //DrawCircle
                drawCircle.DrawCircleMethod(gameObject, rangeAttack, 1);
                // Find Enemy array
                enemys = GameObject.FindGameObjectsWithTag("Enemy");
                // Move Player
                if (Joystick.Direction == Vector2.zero || Input.GetMouseButtonUp(0))
                {
                    // flag isMove
                    isMove = false;
                    SetState(StatePlayer.isIdle);
                    // Check Ranger Attack
                }
                else
                {
                    // flag isMove
                    isMove = true;
                    MoveCharater();
                    // Reset isFirstAttackEveryTimeIdle
                    isFirstAttackEveryTimeIdle = true;
                }
                //attack
                FindNearestEnemy();
                // idle wil lock on EnemyNearest
                if (!isMove)
                {
                    //SetState(StatePlayer.isIdle);
                    LockOntarget();
                }

                //Ckeck ------ timecoundownt and have enemyNearest, isMove?
                if (timeCountdownt <= 0 && NearestEnemyFromPlayerTrans != null && isMove == false && isFirstAttackEveryTimeIdle)// && isFirstAttackEveryTimeIdle
                {
                    if (((NearestEnemyFromPlayerTrans.transform.position - transform.position).magnitude - rangeAttack) <= 0)
                    {
                        isAttack = true;
                        isFirstAttackEveryTimeIdle = false;
                        SetState(StatePlayer.isAttack);
                        timeCountdownt = timeStart;
                    }
                }
                else
                {
                    timeCountdownt -= Time.deltaTime;
                    ShowArrow();
                }
                //Circle foot
                if (NearestEnemyFromPlayerTrans != null)
                {
                    cirleObjectCanvas.transform.position = NearestEnemyFromPlayerTrans.position;
                    cirleObjectCanvas.SetActive(true);
                }
                else
                {
                    cirleObjectCanvas.SetActive(false);
                }
            }
        }
        
    }
    public override void MoveCharater()
    {
        SetState(StatePlayer.isRun);
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
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if (nearestEnemy != null && shortestDistance <= rangeAttack)
        {
            NearestEnemyFromPlayerTrans = nearestEnemy.transform;
        }
        else
        {
            NearestEnemyFromPlayerTrans = null;
        }
    }
    //
    public override void AttackCharater()
    {
        //Play Audio
        PlayAttackAudio();
        GameObject arrow2 = GameObject.FindGameObjectWithTag("SpawArrow").GetComponent<SpawnArrow>().Spawns(playerso.arrowPrefabs);
        arrow2.transform.position = pointFire.position;
        arrow2.transform.rotation = playerso.arrowPrefabs.transform.rotation;
        if(NearestEnemyFromPlayerTrans != null)
        {
            arrow2.GetComponent<Arrow2>().SetTaget(NearestEnemyFromPlayerTrans.position, rangeAttack, gameObject.GetInstanceID());
        }
        //Check if Arrow fire 3 direction
        if (playerso.arrowPrefabs.GetComponent<Arrow2>().arrowSO2.isThreeDirection && NearestEnemyFromPlayerTrans != null)
        {
            // pass 2 Target and fire 2 times
            // fire 2
            GameObject arrow3 = GameObject.FindGameObjectWithTag("SpawArrow").GetComponent<SpawnArrow>().Spawns(playerso.arrowPrefabs);
            arrow3.transform.position = pointFire.position;
            arrow3.transform.rotation = playerso.arrowPrefabs.transform.rotation;
            Vector3 target3Position = transform.position + Quaternion.Euler(0, 45, 0) * (NearestEnemyFromPlayerTrans.position - transform.position);
            arrow3.GetComponent<Arrow2>().SetTaget(target3Position, rangeAttack, gameObject.GetInstanceID());
            //
            // fire 3
            GameObject arrow4 = GameObject.FindGameObjectWithTag("SpawArrow").GetComponent<SpawnArrow>().Spawns(playerso.arrowPrefabs);
            arrow4.transform.position = pointFire.position;
            arrow4.transform.rotation = playerso.arrowPrefabs.transform.rotation;
            Vector3 target4Position = transform.position + Quaternion.Euler(0, -45, 0) * (NearestEnemyFromPlayerTrans.position - transform.position);
            arrow4.GetComponent<Arrow2>().SetTaget(target4Position, rangeAttack, gameObject.GetInstanceID());
            //
        }
    }
    
    public void ShowArrow()
    {
        if(arrowObject != null)
        {
            arrowObject.SetActive(true);
        }
    }
    public void LockOntarget()
    {
        // check if have enemyNearest
        if (NearestEnemyFromPlayerTrans != null)
        {
            Vector3 dirPlayerToEnemy = NearestEnemyFromPlayerTrans.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dirPlayerToEnemy);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;//Time.deltaTime * turnSpeed
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }
    //Grow
    public void Grow()
    {
        if(experience > 2)
        {
            transform.localScale = new Vector3(1 + experience / 2 * 0.1f, 1 + experience / 2 * 0.1f, 1 + experience / 2 * 0.1f);// buff 10% Scale
        }
    }
    IEnumerator DelaySomeSencon()
    {
        yield return new WaitForSeconds(1);
    }
    // Set State Player Function
    public void SetState(StatePlayer _statePlayer)
    {
        if(_statePlayer == StatePlayer.isAttack)
        {
            playerAni.SetTrigger("at");
        }
        else if (_statePlayer == StatePlayer.isDance)
        {
            playerAni.SetBool("isAttack", false);
            playerAni.SetBool("isDance", true);
            playerAni.SetBool("isDead", false);
            playerAni.SetBool("isIdle", false);
            playerAni.SetBool("isRun", false);
            playerAni.SetBool("isUlti", false);
            playerAni.SetBool("isWin", false);
        }
        else if (_statePlayer == StatePlayer.isDead)
        {
            playerAni.SetBool("isAttack", false);
            playerAni.SetBool("isDance", false);
            playerAni.SetBool("isDead", true);
            playerAni.SetBool("isIdle", false);
            playerAni.SetBool("isRun", false);
            playerAni.SetBool("isUlti", false);
            playerAni.SetBool("isWin", false);
        }
        else if (_statePlayer == StatePlayer.isIdle)
        {
            playerAni.SetBool("isAttack", false);
            playerAni.SetBool("isDance", false);
            playerAni.SetBool("isDead", false);
            playerAni.SetBool("isIdle", true);
            playerAni.SetBool("isRun", false);
            playerAni.SetBool("isUlti", false);
            playerAni.SetBool("isWin", false);
        }
        else if (_statePlayer == StatePlayer.isRun)
        {
            playerAni.SetBool("isAttack", false);
            playerAni.SetBool("isDance", false);
            playerAni.SetBool("isDead", false);
            playerAni.SetBool("isIdle", false);
            playerAni.SetBool("isRun", true);
            playerAni.SetBool("isUlti", false);
            playerAni.SetBool("isWin", false);
        }
        else if (_statePlayer == StatePlayer.isUlti)
        {
            playerAni.SetBool("isAttack", false);
            playerAni.SetBool("isDance", false);
            playerAni.SetBool("isDead", false);
            playerAni.SetBool("isIdle", false);
            playerAni.SetBool("isRun", false);
            playerAni.SetBool("isUlti", true);
            playerAni.SetBool("isWin", false);
        }
        else if (_statePlayer == StatePlayer.isWin)
        {
            playerAni.SetBool("isAttack", false);
            playerAni.SetBool("isDance", false);
            playerAni.SetBool("isDead", false);
            playerAni.SetBool("isIdle", false);
            playerAni.SetBool("isRun", false);
            playerAni.SetBool("isUlti", false);
            playerAni.SetBool("isWin", true);
        }

    }
    //End Set state function
    //Set Dead from out of Class
    public void SetDead()
    {
        playerAni.SetBool("isAttack", false);
        playerAni.SetBool("isDance", false);
        playerAni.SetBool("isDead", true);
        playerAni.SetBool("isIdle", false);
        playerAni.SetBool("isRun", false);
        playerAni.SetBool("isUlti", false);
        playerAni.SetBool("isWin", false);
    }
    //
    
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
        gameManager2.footTarget.gameObject.SetActive(false);
        gameObject.tag = "Untagged";
        isDead = true;
        SetState(StatePlayer.isDead);
        StartCoroutine("DelayDie");
    }
}
