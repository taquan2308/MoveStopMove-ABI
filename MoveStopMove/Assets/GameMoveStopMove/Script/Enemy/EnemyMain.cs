using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;

public class EnemyMain : Character, IInitializeVariables
{
    #region Parameter
    private PlayerMain playerMain;
    private EnemyAnimation enemyAnimation;
    public event Action OnAttack = delegate { };
    public event Action OnDance = delegate { };
    public event Action OnDead = delegate { };
    public event Action OnIdle = delegate { };
    public event Action OnRun = delegate { };
    public event Action OnUlti = delegate { };
    public event Action OnWin = delegate { };
    //*********************************************************************
    private NavMeshAgent agent;
    private Rigidbody enemyRb;
    [HideInInspector] public Vector3 enemyOldPos;
    private GameObject[] enemys;
    [SerializeField] private float turnSpeed;
    [SerializeField] private bool isRun;
    [SerializeField] private Vector2 direction;
    [SerializeField] private Vector3 directionMove;
    [SerializeField] private Vector3 dir;
    //Attack
    private Transform nearestEnemyOtherFromThisEnemyTrans;
    [SerializeField] private float rangeAttack;
    [SerializeField] private int experience;
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private Transform pointFire;
    [SerializeField] private float timeStartAttack;
    private float timeCountdowntAttack;
    private float timeStartIdle;
    private float timeCountdowntIdle;

    // get radompoint
    private RandomPoints randomPoints;
    private Vector3 pointToGo;
    //Exp Canvas
    private TextMeshProUGUI txtExp;
    private Transform canvasExpTrans;
    private bool isAddExp;
    [SerializeField] private GameObject expPrefabs;
    //Animation Player
    [SerializeField] private Animator playerAni;
    //Check State Player
    
    //Check first attack each time idle
    private bool isFirstAttackEveryTimeIdle;
    //Effect
    [SerializeField] private GameObject effectPrefabs;
    [SerializeField] private bool isEffect;
    //Audio
    [SerializeField] private AudioClip attackAudio;
    [SerializeField] private AudioClip dieAudio;
    private AudioSource audioSource;
    //Has point random
    [SerializeField] private bool hasPointRandom;
    //Play Game
    [SerializeField] private bool isPlay;
    [SerializeField] private GameObject arrowObject;
    
    //
    public bool IsPlay { get => isPlay; set => isPlay = value; }
    public float RangeAttack { get => rangeAttack; set => rangeAttack = value; }
    public int Experience { get => experience; set => experience = value; }
    public bool IsAddExp { get => isAddExp; set => isAddExp = value; }
    public bool IsEffect { get => isEffect; set => isEffect = value; }
    public EnemySO EnemySO { get => enemySO; set => enemySO = value; }
    public Transform PointFire { get => pointFire; set => pointFire = value; }
    public Transform NearestEnemyOtherFromThisEnemyTrans { get => nearestEnemyOtherFromThisEnemyTrans; set => nearestEnemyOtherFromThisEnemyTrans = value; }
    public EnemyAnimation EnemyAnimation { get => enemyAnimation; set => enemyAnimation = value; }
    public GameObject ArrowObject { get => arrowObject; set => arrowObject = value; }
    //
    
    // Controll oder execute on Update
    private bool turn01;
    private bool turn02;
    private bool turn03;
    private bool turn04;
    
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();//
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlay)
        {
            if (!enemyAnimation.IsDead)
            {
                #region grow,exp
                //Grow
                Grow();
                //
                if (isEffect)
                {
                    isEffect = false;
                    GameObject effectAdd = SpawnArrow.Instance.Spawns(effectPrefabs);
                    effectAdd.transform.position = transform.position;
                    effectAdd.transform.rotation = expPrefabs.transform.rotation;
                }
                //Exp
                txtExp.text = experience.ToString();
                canvasExpTrans.eulerAngles = new Vector3(0, 90, 0);
                if (isAddExp)
                {
                    isAddExp = false;
                    isEffect = false;
                    GameObject expAdd = SpawnArrow.Instance.Spawns(expPrefabs);
                    expAdd.transform.position = transform.position;
                    expAdd.transform.rotation = expPrefabs.transform.rotation;
                }
                #endregion
                // Check Move
                FindNearestEnemy();
                if (!agent.hasPath)
                {
                    LockOntarget();
                    if (nearestEnemyOtherFromThisEnemyTrans != null)
                    {
                        if ((transform.position - nearestEnemyOtherFromThisEnemyTrans.position).magnitude <= rangeAttack)
                        {
                        }
                    }
                            
                    if (!enemyAnimation.IsAttack)
                    {
                        OnIdle?.Invoke();
                        if(nearestEnemyOtherFromThisEnemyTrans != null)
                        {
                            bool isOnrange = (transform.position - nearestEnemyOtherFromThisEnemyTrans.position).magnitude 
                                <= rangeAttack;
                            if (timeCountdowntAttack <= 0 && isOnrange && isFirstAttackEveryTimeIdle)
                            {
                                isFirstAttackEveryTimeIdle = false;
                                OnAttack?.Invoke();
                                HideArrow();
                                timeCountdowntAttack = timeStartAttack;
                            }
                            else
                            {
                                timeCountdowntIdle = timeStartIdle;
                                isFirstAttackEveryTimeIdle = true;
                                OnRun?.Invoke();
                                AsignDestination();
                            }
                        }
                    }
                    else
                    {
                        if(timeCountdowntIdle <= 0)
                        {
                            timeCountdowntIdle = timeStartIdle;
                            isFirstAttackEveryTimeIdle = true;
                            OnRun?.Invoke();
                            AsignDestination();
                        }
                        else
                        {
                            timeCountdowntIdle -= Time.deltaTime;
                            //OnIdle?.Invoke();
                        }
                        timeCountdowntAttack -= Time.deltaTime;
                        timeCountdowntAttack = Mathf.Clamp(timeCountdowntAttack, 0, Mathf.Infinity);
                    }
                    // check countdown idle
                }
                else
                {
                    OnRun?.Invoke();
                    timeCountdowntAttack -= Time.deltaTime;
                    timeCountdowntAttack = Mathf.Clamp(timeCountdowntAttack, 0, Mathf.Infinity);
                }
            }
        }
    }
    public void ShowArrow()
    {
        if (arrowObject != null)
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
    public void FindNearestEnemy()
    {
        // Must Exclude itself
        GameObject[] enemies = new GameObject[GameManager.Instance.ListEnemy.Count];
        int count = 0;
        foreach (var enemie in GameManager.Instance.ListEnemy)
        {
            enemies[count] = enemie;
            count++;
        }
        GameObject[] enemiesAll = new GameObject[enemies.Length + 1];
        for (int i = 0; i < enemies.Length; i++)
        {
            if(gameObject.GetInstanceID() != enemies[i].GetInstanceID())
            {
                enemiesAll[i] = enemies[i];
            }
        }
        if(playerMain != null)
        {
            enemiesAll[enemies.Length] = playerMain.gameObject;
        }
        
        float shortestDistance = Mathf.Infinity;
        nearestEnemyOtherFromThisEnemyTrans = null;

        //
        foreach (GameObject enemy in enemiesAll)
        {
            
            if (enemy != null)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemyOtherFromThisEnemyTrans = enemy.transform;
                }
            }
        }
    }
    //
    public override void Attack()
    {
        //Play Audio
        PlayAttackAudio();
        GameObject arrow2 = SpawnArrow.Instance.Spawns(enemySO.arrowPrefabs);
        arrow2.transform.position = pointFire.position;
        arrow2.transform.rotation = enemySO.arrowPrefabs.transform.rotation;
        arrow2.GetComponent<Arrow>().SetTaget(nearestEnemyOtherFromThisEnemyTrans.position, rangeAttack, gameObject.GetInstanceID());
    }
    public void LockOntarget()
    {
        // check if have enemyNearest
        if (nearestEnemyOtherFromThisEnemyTrans != null)
        {
            Vector3 dirPlayerToEnemy = nearestEnemyOtherFromThisEnemyTrans.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dirPlayerToEnemy);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }
    public void Grow()
    {
        if (experience > 2)
        {
            transform.localScale = new Vector3(1 + experience / 2 * 0.1f, 1 + experience / 2 * 0.1f, 1 + experience / 2 * 0.1f);// buff 10% Scale
        }
    }
    //
    public void AsignDestination()
    {
        //check if in range no Enemmy
        if (nearestEnemyOtherFromThisEnemyTrans != null)//!isMove && 
        {
            pointToGo = nearestEnemyOtherFromThisEnemyTrans.GetComponent<RandomPoints>().GetPointRandomAroundThisObject();
            //
            directionMove = pointToGo - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(directionMove);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;//Time.deltaTime * turnSpeed

            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            Debug.DrawRay(pointToGo, Vector3.up, Color.black, 2);
            agent.destination = pointToGo;
        }
    }
    
    //End Set state function
    //Set Dead from out of Class
    
    //
    public void PlayAttackAudio()
    {
        //audioSource.clip = attackAudio;
        audioSource.PlayOneShot(attackAudio);
    }
    public void PlayDieAudio()
    {
        //audioSource.clip = attackAudio;
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
        gameObject.tag = "Untagged";
        OnDead?.Invoke();
        StartCoroutine("DelayDie");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")|| collision.gameObject.CompareTag("Player"))
        {
            AsignDestination();
        }
    }
    
    public void InitializeVariables()
    {
        playerMain = PlayerMain.Instance;
        isEffect = false;
        enemyAnimation = GetComponent<EnemyAnimation>();
        agent = GetComponent<NavMeshAgent>();
        enemyRb = GetComponent<Rigidbody>();
        enemyOldPos = transform.position;
        //attack
        rangeAttack = enemySO.rangeAttack;
        //Countdownt attack
        timeStartAttack = enemySO.speedAttack;
        timeCountdowntAttack = 0;
        timeStartIdle = enemySO.timeIdleStart;
        timeCountdowntIdle = timeStartIdle;
        // initialization 
        turnSpeed = enemySO.turnSpeed;
        // get radompoint and going this
        randomPoints = GetComponent<RandomPoints>();
        //canvas
        txtExp = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        canvasExpTrans = gameObject.GetComponentsInChildren<Transform>()[1];
        isAddExp = false;
        // Set State
        isFirstAttackEveryTimeIdle = true;
        isEffect = false;
        //Audio
        audioSource = GetComponent<AudioSource>();
        hasPointRandom = false;
        isPlay = GameManager.Instance.IsPlay;
        GameManager.Instance.ListEnemy.Add(this.gameObject);//
        isRun = false;
        //
        turn01 = true;
        turn02 = true;
        turn03 = true;
        turn04 = true;
        GameObject showPositionOnUi = (GameObject)Instantiate(UiManager.Instance.ShowPositionOnUi, UiManager.Instance.CanvasLive.transform);
        showPositionOnUi.GetComponent<ShowPositionOnUi>().GetTransformEnemy(this.gameObject.transform);
    }
}

//public void SetPositionGoUp(Vector3 _posGoUp)
//{
//    posGoUp = _posGoUp;
//}
//private Vector3 posGoUp;
//private bool isFirstSpawn;
//GoUp
//private float speedGoUp;
//private float startTime;
//private float journeyLength;
//private bool isFirstTime01;
//isFirstTime01 = false;
//journeyLength = 2.5f;
//if (isFirstSpawn)
//{
//    if (!isFirstTime01)
//    {
//        isFirstTime01 = true;
//        startTime = Time.time;
//    }
//    float distCovered = (Time.time - startTime) * speedGoUp;
//    float fractionOfJourney = distCovered / journeyLength;
//    //isFirstSpawn = false;
//    transform.position = Vector3.Lerp(posGoUp, posGoUp + new Vector3(0, 2.5f, 0), fractionOfJourney);
//    Debug.Log("ssfsd");
//}