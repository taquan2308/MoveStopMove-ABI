using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    private NavMeshAgent agent;
    private Rigidbody enemyRb;
    [HideInInspector] public Vector3 enemyOldPos;
    private GameObject[] enemys;
    [SerializeField] private float enemyRangerAttack = 8;
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
        
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //// Find Enemy array
        //enemys = GameObject.FindGameObjectsWithTag("Enemy");
        //// Move Player
        ////Debug.Log(Joystick.Direction);
        //if (Joystick.Direction == Vector2.zero)
        //{
        //    // flag isMove
        //    isMove = false;
        //    //animator.SetBool("isRun", false);
        //    // Check Ranger Attack
        //    foreach (GameObject enemy in enemys)
        //    {
        //        if (enemy != null)
        //        {
        //            Vector3 dir = enemy.transform.position - transform.position;
        //            if (dir.magnitude < playerRangerAttack)
        //            {
        //                //animator.SetBool("isAttack", true);
        //                //return;
        //            }
        //            else
        //            {
        //                //animator.SetBool("isAttack", false);
        //            }

        //        }
        //    }
        //}
        //else
        //{
        //    //animator.SetBool("isRun", true);
        //    // flag isMove
        //    isMove = true;
        //    // Move if JoyStick change
        //    MoveCharater();
        //    // Set position Behind Player
        //    cameraPos.position = transform.position + cameraPosOffset;
        //}
        //attack
        FindNearestEnemy();
        //Debug.Log("---");

        // idle wil lock on EnemyNearest
        //if (!isMove)
        //{
        //    LockOntarget();
        //}
        if (!agent.hasPath)
        {
            isMove = false;
            LockOntarget();
            StartCoroutine("DelaySomeSencon");
            MoveToPointRandom();
        }
        else
        {
            isMove = true;
        }
        //if(transform.position == randomPoints)

        //Ckeck ------ timecoundownt and have enemyNearest, isMove?

        if (timeCountdownt <= 0 && NearestEnemyOtherFromThisEnemyTrans != null && isMove == false)//&& isMove == false
        {
            if((transform.position - NearestEnemyOtherFromThisEnemyTrans.position).magnitude <= rangeAttack)
            {
                AttackCharater();
                timeCountdownt = timeStart;
            }
        }
        else
        {
            timeCountdownt -= Time.deltaTime;
            timeCountdownt = Mathf.Clamp(timeCountdownt,  0, Mathf.Infinity);
        }

        //
    }
    //public override void MoveCharater()
    //{
    //    //override the ParentClass implementation here
    //    direction = Joystick.Direction;
    //    directionMove = new Vector3(-direction.y, 0, direction.x);
    //    agent.destination = transform.position + directionMove * Time.deltaTime * speed;
    //}
    public void FindNearestEnemy()
    {
        // Must Exclude itself
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] enemiesAll = new GameObject[enemies.Length];
        for (int i = 0; i < enemies.Length - 1; i++)
        {
            if(gameObject.GetInstanceID() != enemies[i].GetInstanceID())
            {
                enemiesAll[i] = enemies[i];
            }
        }
        enemiesAll[enemies.Length - 1] = player;
        float distanceToPlayer = (player.transform.position - transform.position).magnitude;
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        NearEnemyOutRangeAttackTrans = null;
        //
        foreach (GameObject enemy in enemiesAll)
        {
            if(enemy != null)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                distanceToEnemy = Mathf.Min(distanceToPlayer, distanceToEnemy);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
        }

        if (nearestEnemy != null && shortestDistance <= rangeAttack)
        {
            NearestEnemyOtherFromThisEnemyTrans = nearestEnemy.transform;
        }
        else
        {
            // onrange
            NearestEnemyOtherFromThisEnemyTrans = null;
            //out range
            NearEnemyOutRangeAttackTrans = nearestEnemy.transform;
        }
    }
    //
    public override void AttackCharater()
    {
        //GameObject arrow2 = (GameObject)Instantiate(enemyso.arrowPrefabs, pointFire.position, enemyso.arrowPrefabs.transform.rotation);
        //GameObject arrow2 = GetComponent<Spawn>().Spawns(enemyso.arrowPrefabs);
        GameObject arrow2 = GameObject.FindGameObjectWithTag("SpawArrow").GetComponent<SpawnArrow>().Spawns(enemyso.arrowPrefabs);
        arrow2.transform.position = pointFire.position;
        arrow2.transform.rotation = enemyso.arrowPrefabs.transform.rotation;
        arrow2.GetComponent<Arrow2>().SetTaget(NearestEnemyOtherFromThisEnemyTrans, rangeAttack, gameObject.name, gameObject.GetInstanceID());
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
    public void MoveToPointRandom()
    {
        //check if in range no Enemmy
        if (NearEnemyOutRangeAttackTrans != null)//!isMove && 
        {
            //Debug.Log(NearestEnemyOtherFromThisEnemyTrans.GetComponent<RandomPoints>().pointRandomAroundThisObject);
            pointToGo = NearEnemyOutRangeAttackTrans.GetComponent<RandomPoints>().GetPointRandomAroundThisObject();
            //Draw point wil go
            Debug.DrawRay(pointToGo, Vector3.up, Color.black, 2);
            agent.destination = pointToGo;
        }
        //check if in range has Enemmy
        if (NearestEnemyOtherFromThisEnemyTrans != null)//!isMove && 
        {
            //Debug.Log(NearestEnemyOtherFromThisEnemyTrans.GetComponent<RandomPoints>().pointRandomAroundThisObject);
            pointToGo = NearestEnemyOtherFromThisEnemyTrans.GetComponent<RandomPoints>().GetPointRandomAroundThisObject();
            //Draw point wil go
            Debug.DrawRay(pointToGo, Vector3.up, Color.black, 2);
            agent.destination = pointToGo;
        }
    }
    // coroutine delay
    IEnumerator DelaySomeSencon()
    {
        yield return new WaitForSeconds(1);
    }
    //DrawWireSphere
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 4);
        Gizmos.DrawWireSphere(transform.position, 10);
        Gizmos.DrawWireSphere(transform.position, rangeAttack);
    }

#endif
}

















//GameObject[] enemyOtherArrayObj = GameObject.FindGameObjectsWithTag("Enemy");
//GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
//float distance = Mathf.Infinity;
//Vector3 directToEnemy;
//Vector3 directToPlayer;
//if (enemyOtherArrayObj != null || playerObj != null)
//{
//    foreach (GameObject enemy in enemyOtherArrayObj)
//    {
//        if(enemy.GetInstanceID() != gameObject.GetInstanceID())
//        {
//            directToEnemy = enemy.transform.position - transform.position;
//            if (directToEnemy.magnitude < distance)
//            {
//                NearestEnemyOtherFromThisEnemyTrans = enemy.transform;
//                distance = directToEnemy.magnitude;
//            }
//        }
//    }
//    directToPlayer = playerObj.transform.position - transform.position;
//    if (NearestEnemyOtherFromThisEnemyTrans != null && directToPlayer.magnitude < distance)
//    {
//        NearestEnemyOtherFromThisEnemyTrans = playerObj.transform;
//        distance = directToPlayer.magnitude;
//    }
//}
//else
//{
//    NearestEnemyOtherFromThisEnemyTrans = null;
//}
//---------------------
//Collider[] colliders = Physics.OverlapSphere(transform.position, 1000);//  colliders has on Sphere   rangeAttack,distance 1000 to check all Enemy on MAP
//                                                                              // Set collider = null if only Player'Collider in range attack of player
//int maxColliders = 10;
//Collider[] hitColliders = new Collider[maxColliders];
//int numberColliderInRangePlayer = Physics.OverlapSphereNonAlloc(transform.position, rangeAttack, hitColliders);
//// if in Range attack Player no enemy, not assign EnemyNearest = null
//if (numberColliderInRangePlayer <= 1)
//{
//    colliders = null;
//}
////
//float distance = Mathf.Infinity;
//Vector3 directToEnemy;
//if (colliders != null)
//{
//    foreach (Collider collider in colliders)
//    {
//        if (collider.gameObject.GetInstanceID() != gameObject.GetInstanceID())
//        {
//            directToEnemy = transform.position - collider.gameObject.transform.position;
//            if (collider.CompareTag("Enemy") || collider.CompareTag("Player"))
//            {
//                if (directToEnemy.magnitude < distance)
//                {
//                    NearestEnemyOtherFromThisEnemyTrans = collider.gameObject.transform;
//                    distance = directToEnemy.magnitude;
//                    //Debug.Log(distance);
//                }
//            }

//        }
//    }
//}
//else
//{
//    NearestEnemyOtherFromThisEnemyTrans = null;
//}
//-------------------------------------------