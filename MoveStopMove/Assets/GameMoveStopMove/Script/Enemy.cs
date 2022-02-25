using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;

public class Enemy : Character
{
    private NavMeshAgent agent;
    private Rigidbody enemyRb;
    [HideInInspector] public Vector3 enemyOldPos;
    private GameObject[] enemys;
    [SerializeField] private float turnSpeed;
    [SerializeField] private bool isRun;
    [SerializeField] private Vector2 direction;
    [SerializeField] private Vector3 directionMove;
    [SerializeField] private Vector3 dir;

    private float horizontalInput;
    private float forwardInput;

    Animator animator;
    //check if Move
    private bool isMove;
    //Attack
    //enemy nearest in range attack
    [HideInInspector] public Transform NearestEnemyOtherFromThisEnemyTrans;
    // enemy nearest Out range attack while no enemy in range attack
    [HideInInspector] public Transform NearEnemyOutRangeAttackTrans;
    public float rangeAttack;
    public int experience;
    public EnemySO2 enemyso;
    public Transform pointFire;
    public float timeStart;
    public float timeCountdownt;
    
    // get radompoint
    [HideInInspector] RandomPoints randomPoints;
    [HideInInspector] Vector3 pointToGo;
    //Exp Canvas
    [HideInInspector] public TextMeshProUGUI txtExp;
    [HideInInspector] public Transform canvasExpTrans;
    [HideInInspector] public bool isAddExp;
    public GameObject expPrefabs;
    //Animation Player
    public Animator playerAni;
    //Check State Player
    [HideInInspector]
    public enum StateEnemy
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
    //public StatePlayer ActiveState = StatePlayer.isIdle;
    //Check first attack each time idle
    [HideInInspector] public bool isFirstAttackEveryTimeIdle;
    //Effect
    public GameObject effectPrefabs;
    public bool isEffect;
    //Audio
    public AudioClip attackAudio;
    public AudioClip dieAudio;
    [HideInInspector] public AudioSource audioSource;
    //Has point random
    public bool hasPointRandom;
    //Play Game
    [HideInInspector] public bool play;
    [HideInInspector] public UiManager uiManager;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        enemyOldPos = transform.position;
        //attack
        rangeAttack = enemyso.rangeAttack;
        //Countdownt attack
        timeStart = enemyso.speedAttack;
        timeCountdownt = 0;
        // initialization 
        isMove = false;
        //
        turnSpeed = enemyso.turnSpeed;
        // get radompoint and going this
        randomPoints = GetComponent<RandomPoints>();
        //canvas
        txtExp = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        canvasExpTrans = gameObject.GetComponentsInChildren<Transform>()[1];
        isAddExp = false;
        // Set State
        SetState(StateEnemy.isIdle);
        isFirstAttackEveryTimeIdle = true;
        isEffect = false;
        //Audio
        audioSource = GetComponent<AudioSource>();
        hasPointRandom = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        //
        isEffect = false;
        uiManager = GameObject.FindGameObjectWithTag("UiManager").GetComponent<UiManager>();
    }

    // Update is called once per frame
    void Update()
    {
        play = uiManager.play;
        if (play)
        {
            if (!isDead)
            {
                //
                //SetState(StateEnemy.isDance);
                //Grow
                Grow();
                //
                if (isEffect)
                {
                    isEffect = false;
                    GameObject effect = (GameObject)Instantiate(effectPrefabs, transform.position, transform.rotation);
                    Destroy(effect, 0.2f);
                }
                //Exp
                txtExp.text = experience.ToString();
                canvasExpTrans.eulerAngles = new Vector3(0, 90, 0);//.Rotate(0, -90, 0,Space.World);
                if (isAddExp)
                {
                    isAddExp = false;
                    //GameObject exp = (GameObject)Instantiate(expPrefabs, transform.position, expPrefabs.transform.rotation);
                    //
                    GameObject expAdd = GameObject.FindGameObjectWithTag("SpawArrow").GetComponent<SpawnArrow>().Spawns(expPrefabs);
                    expAdd.transform.position = transform.position;
                    expAdd.transform.rotation = expPrefabs.transform.rotation;
                    expAdd.GetComponent<AddExp>().t = Time.time;
                    //
                    //expAdd.GetComponent<AddExp>().txtExp.text = "'+ 2";
                    //expAdd.SetActive(false);
                }
                FindNearestEnemy();
                //Debug.Log("---");

                // idle wil lock on EnemyNearest
                //if (!isMove)
                //{
                //    LockOntarget();
                //}
                if (!agent.hasPath && !hasPointRandom)
                {
                    isMove = false;
                    SetState(StateEnemy.isIdle);
                    LockOntarget();
                    StartCoroutine("DelayFindRandomPointAndRun");
                    //MoveToPointRandom();
                }
                else
                {
                    isMove = true;
                    hasPointRandom = false;
                    SetState(StateEnemy.isRun);
                    // Reset isFirstAttackEveryTimeIdle
                    isFirstAttackEveryTimeIdle = true;
                }
                //Ckeck ------ timecoundownt and have enemyNearest, isMove?

                if (timeCountdownt <= 0 && NearestEnemyOtherFromThisEnemyTrans != null && isMove == false && isFirstAttackEveryTimeIdle)//&& isMove == false
                {
                    if ((transform.position - NearestEnemyOtherFromThisEnemyTrans.position).magnitude <= rangeAttack)
                    {
                        AttackCharater();
                        //SetState(StateEnemy.isAttack);
                        timeCountdownt = timeStart;
                        // 
                        isFirstAttackEveryTimeIdle = false;
                    }
                }
                else
                {
                    timeCountdownt -= Time.deltaTime;
                    timeCountdownt = Mathf.Clamp(timeCountdownt, 0, Mathf.Infinity);
                }
            }
        }
    }
    public void FindNearestEnemy()
    {
        // Must Exclude itself
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] enemiesAll = new GameObject[enemies.Length + 1];
        //Debug.Log("enemies.Length    "+enemies.Length);
        for (int i = 0; i < enemies.Length; i++)
        {
            if(gameObject.GetInstanceID() != enemies[i].GetInstanceID())
            {
                enemiesAll[i] = enemies[i];
            }
        }
        if(player != null)
        {
            enemiesAll[enemies.Length] = player;
            //Debug.Log("Player here =======================");
        }
        
        float shortestDistance = Mathf.Infinity;
        NearEnemyOutRangeAttackTrans = null;
        //
        foreach (GameObject enemy in enemiesAll)
        {
            
            if (enemy != null)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    NearestEnemyOtherFromThisEnemyTrans = enemy.transform;
                }
            }
        }
    }
    //
    public override void AttackCharater()
    {
        //Play Audio
        PlayAttackAudio();
        //GameObject arrow2 = (GameObject)Instantiate(enemyso.arrowPrefabs, pointFire.position, enemyso.arrowPrefabs.transform.rotation);
        //GameObject arrow2 = GetComponent<Spawn>().Spawns(enemyso.arrowPrefabs);
        //SetState(StateEnemy.isAttack);
        GameObject arrow2 = GameObject.FindGameObjectWithTag("SpawArrow").GetComponent<SpawnArrow>().Spawns(enemyso.arrowPrefabs);
        arrow2.transform.position = pointFire.position;
        arrow2.transform.rotation = enemyso.arrowPrefabs.transform.rotation;
        arrow2.GetComponent<Arrow2>().SetTaget(NearestEnemyOtherFromThisEnemyTrans.position, rangeAttack, gameObject.GetInstanceID());
        //Destroy(arrow2, 2);
    }
    public void LockOntarget()
    {
        //Debug.Log(NearestEnemyOtherFromThisEnemyTrans.name);
        // check if have enemyNearest
        if (NearestEnemyOtherFromThisEnemyTrans != null)
        {
            Vector3 dirPlayerToEnemy = NearestEnemyOtherFromThisEnemyTrans.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dirPlayerToEnemy);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;//Time.deltaTime * turnSpeed
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }
    //public void MoveToPointRandom()
    //{
    //    StartCoroutine("DelayAndRun");// error Must place StartCoroutine in Update
    //}
    //
    public void Grow()
    {
        if (experience > 2)
        {
            transform.localScale = new Vector3(1 + experience / 2 * 0.1f, 1 + experience / 2 * 0.1f, 1 + experience / 2 * 0.1f);// buff 10% Scale
        }
    }
    // coroutine delay
    IEnumerator DelayFindRandomPointAndRun()
    {
        yield return new WaitForSeconds(1);
        //check if in range no Enemmy
        if (NearEnemyOutRangeAttackTrans != null)//!isMove && 
        {
            //Debug.Log(NearestEnemyOtherFromThisEnemyTrans.GetComponent<RandomPoints>().pointRandomAroundThisObject);
            //
            if (!hasPointRandom)
            {
                pointToGo = NearEnemyOutRangeAttackTrans.GetComponent<RandomPoints>().GetPointRandomAroundThisObject();
                hasPointRandom = true;
            }
            //
            directionMove = pointToGo - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(directionMove);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;//Time.deltaTime * turnSpeed

            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            //
            //SetState(StateEnemy.isRun);
            //Draw point wil go
            Debug.DrawRay(pointToGo, Vector3.up, Color.black, 2);
            agent.destination = pointToGo;
        }
        //check if in range has Enemmy
        if (NearestEnemyOtherFromThisEnemyTrans != null)//!isMove && 
        {
            //Debug.Log(NearestEnemyOtherFromThisEnemyTrans.GetComponent<RandomPoints>().pointRandomAroundThisObject);
            //
            if (!hasPointRandom)
            {
                pointToGo = NearestEnemyOtherFromThisEnemyTrans.GetComponent<RandomPoints>().GetPointRandomAroundThisObject();
                hasPointRandom = true;
            }
            //
            //Draw point wil go
            Debug.DrawRay(pointToGo, Vector3.up, Color.black, 2);
            directionMove = pointToGo - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(directionMove);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;//Time.deltaTime * turnSpeed
            //Debug.Log(rotation);
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            //
            agent.destination = pointToGo;
        }
    }
    // Set State Player Function
    public void SetState(StateEnemy _statePlayer)
    {
        if (_statePlayer == StateEnemy.isAttack)
        {
            playerAni.SetBool("isAttack", true);
            playerAni.SetBool("isDance", false);
            playerAni.SetBool("isDead", false);
            playerAni.SetBool("isIdle", false);
            playerAni.SetBool("isRun", false);
            playerAni.SetBool("isUlti", false);
            playerAni.SetBool("isWin", false);
        }
        else if (_statePlayer == StateEnemy.isDance)
        {
            playerAni.SetBool("isAttack", false);
            playerAni.SetBool("isDance", true);
            playerAni.SetBool("isDead", false);
            playerAni.SetBool("isIdle", false);
            playerAni.SetBool("isRun", false);
            playerAni.SetBool("isUlti", false);
            playerAni.SetBool("isWin", false);
        }
        else if (_statePlayer == StateEnemy.isDead)
        {
            playerAni.SetBool("isAttack", false);
            playerAni.SetBool("isDance", false);
            playerAni.SetBool("isDead", true);
            playerAni.SetBool("isIdle", false);
            playerAni.SetBool("isRun", false);
            playerAni.SetBool("isUlti", false);
            playerAni.SetBool("isWin", false);
        }
        else if (_statePlayer == StateEnemy.isIdle)
        {
            playerAni.SetBool("isAttack", false);
            playerAni.SetBool("isDance", false);
            playerAni.SetBool("isDead", false);
            playerAni.SetBool("isIdle", true);
            playerAni.SetBool("isRun", false);
            playerAni.SetBool("isUlti", false);
            playerAni.SetBool("isWin", false);
        }
        else if (_statePlayer == StateEnemy.isRun)
        {
            playerAni.SetBool("isAttack", false);
            playerAni.SetBool("isDance", false);
            playerAni.SetBool("isDead", false);
            playerAni.SetBool("isIdle", false);
            playerAni.SetBool("isRun", true);
            playerAni.SetBool("isUlti", false);
            playerAni.SetBool("isWin", false);
        }
        else if (_statePlayer == StateEnemy.isUlti)
        {
            playerAni.SetBool("isAttack", false);
            playerAni.SetBool("isDance", false);
            playerAni.SetBool("isDead", false);
            playerAni.SetBool("isIdle", false);
            playerAni.SetBool("isRun", false);
            playerAni.SetBool("isUlti", true);
            playerAni.SetBool("isWin", false);
        }
        else if (_statePlayer == StateEnemy.isWin)
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
        isDead = true;
        SetState(StateEnemy.isDead);
        StartCoroutine("DelayDie");
    }
    //DrawWireSphere
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, 4);
        //Gizmos.DrawWireSphere(transform.position, 10);
        //Gizmos.DrawWireSphere(transform.position, rangeAttack);
    }

#endif
}
