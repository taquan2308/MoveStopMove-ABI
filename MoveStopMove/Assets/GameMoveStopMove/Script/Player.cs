using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Player : Character
{
    private NavMeshAgent agent;
    private Rigidbody playerRb;
    public Transform goal;
    public Transform cameraPos;
    [HideInInspector] public Vector3 playerOldPos;
    [HideInInspector] public Vector3 cameraPosOffset;
    private GameObject[] enemys;
    [SerializeField] private Joystick Joystick;// must drag from Child Canvas have Prefabs 1 on 4 type Stick( float, fixed,..)
    [SerializeField] private float playerRangerAttack = 8;
    [SerializeField] private float speed = 30;
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
    //[HideInInspector] 
    [HideInInspector] public Transform NearestEnemyFromPlayerTrans;
    public float rangeAttack;
    public PlayerSO playerso;
    public Transform pointFire;
    public float timeStart;
    public float timeCountdownt;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cameraPosOffset = new Vector3(20f, 15f, 0);
        playerOldPos = transform.position;
        //attack
        rangeAttack = playerso.rangeAttack;
        //Countdownt attack
        timeStart = playerso.speedAttack;
        timeCountdownt = 0;
        // initialization 
        isMove = false;
        //
        turnSpeed = playerso.turnSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Find Enemy array
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        // Move Player
        //Debug.Log(Joystick.Direction);
        if (Joystick.Direction == Vector2.zero)
        {
            // flag isMove
            isMove = false;
            //animator.SetBool("isRun", false);
            // Check Ranger Attack
            foreach (GameObject enemy in enemys)
            {
                if (enemy != null)
                {
                    Vector3 dir = enemy.transform.position - transform.position;
                    if (dir.magnitude < playerRangerAttack)
                    {
                        //animator.SetBool("isAttack", true);
                        //return;
                    }
                    else
                    {
                        //animator.SetBool("isAttack", false);
                    }

                }
            }
        }
        else
        {
            //animator.SetBool("isRun", true);
            // flag isMove
            isMove = true;
            // Move if JoyStick change
            MoveCharater();
            // Set position Behind Player
            cameraPos.position = transform.position + cameraPosOffset;
        }
        //attack
        FindNearestEnemy();
        //Debug.Log("---");

        // idle wil lock on EnemyNearest
        if (!isMove)
        {
            LockOntarget();
        }
        
        //Ckeck ------ timecoundownt and have enemyNearest, isMove?
        if (timeCountdownt <= 0 && NearestEnemyFromPlayerTrans != null && isMove == false)
        {
            AttackCharater();
            timeCountdownt = timeStart;
        }
        else
        {
            timeCountdownt -= Time.deltaTime;
        }
        //
    }
    public override void MoveCharater()
    {
        //override the ParentClass implementation here
        direction = Joystick.Direction;
        directionMove = new Vector3( -direction.y, 0, direction.x );
        agent.destination = transform.position + directionMove * Time.deltaTime * speed;
    }
    public void FindNearestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, rangeAttack);//  colliders has on Sphere   rangeAttack
        // Set collider = null if only Player'Collider in range attack of player
            int maxColliders = 10;
            Collider[] hitColliders = new Collider[maxColliders];
            int numberColliderInRangePlayer = Physics.OverlapSphereNonAlloc(transform.position, rangeAttack, hitColliders);
            // if in Range attack Player no enemy, not assign EnemyNearest = null
            if(numberColliderInRangePlayer <= 1)
            {
                colliders = null;
            }
        //
        float distance = Mathf.Infinity;
        Vector3 directToEnemy;
        if(colliders != null)
        {
            foreach (Collider collider in colliders)
            {
                directToEnemy = transform.position - collider.gameObject.transform.position;
                if (collider.CompareTag("Enemy"))
                {
                    if(directToEnemy.magnitude < distance)
                    {
                        NearestEnemyFromPlayerTrans = collider.gameObject.transform;
                        distance = directToEnemy.magnitude;
                        //Debug.Log(distance);
                    }
                }
            }
        }
        else
        {
            NearestEnemyFromPlayerTrans = null;
        }

    }
    //
    public override void AttackCharater()
    {
        GameObject arrow2 = (GameObject) Instantiate(playerso.arrowPrefabs, pointFire.position, playerso.arrowPrefabs.transform.rotation);
        arrow2.GetComponent<Arrow2>().SetTaget(NearestEnemyFromPlayerTrans);
        Destroy(arrow2, 3);
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
    //DrawWireSphere
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 4);
        Gizmos.DrawWireSphere(transform.position, 10);
    }

#endif
}
