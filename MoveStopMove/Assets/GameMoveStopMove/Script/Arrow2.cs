using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// named Arrow2 because do multi Project on One Project, other has Arrow
public class Arrow2 : MonoBehaviour
{
    public ArrowSO2 arrowSO2;
    [HideInInspector] public float speedArrow2;
     public Vector3 targetPosition;
    [HideInInspector] public Vector3 dirrectVector3;
    //
    private float rangeAttack;
    public Vector3 enemyPos;
    private Vector3 offsetPositionTarget;
    //private string nameOfArrowOwner;
    private int iDOwner;
    public float rangeAdd;
    public int experienceAdd;
    //
    private float distaneToTarget;
    private bool isFirtSetUp;
    
    private void Awake()
    {
        speedArrow2 = arrowSO2.speedArrow2;
        isFirtSetUp = true;
        offsetPositionTarget =new Vector3(0, 1.26f, 0);
        //
        rangeAdd = 0.1f;// add 10 %
        experienceAdd = 2;
    }
    // Start is called before the first frame update
    void Start()
    {
        rangeAdd = 2;
        experienceAdd = 2;
    }

    // Update is called once per frame
    void Update()
    {
        MoveArrow2();
    }
    public void SetTaget(Vector3 _targetPosition, float _rangeAttack, int _iDOwner)
    {
        //GameObject targetPosition = (GameObject) Instantiate(emptyGameObjPrefabs, _targetTrans.position, targetTrans.rotation);
        targetPosition = _targetPosition + offsetPositionTarget;
        rangeAttack = _rangeAttack;
        //nameOfArrowOwner = _nameOfArrowOwner;
        iDOwner = _iDOwner;
        //enemyPos = _targetTrans.position;
        ////targetTrans.position = new Vector3(_targetTrans.position.x, _targetTrans.position.y, _targetTrans.position.z);
        //targetTrans.position = enemyPos;

    }
    public void MoveArrow2()
    {
        // Move Arrow to Max Pig rangeAttack Player with direction cacular
        dirrectVector3 = targetPosition - transform.position;
        if (isFirtSetUp)
        {
            targetPosition = transform.position + (dirrectVector3.normalized * rangeAttack);
            isFirtSetUp = false;
        }
        distaneToTarget = dirrectVector3.magnitude;
        transform.Translate(dirrectVector3.normalized * Time.deltaTime * speedArrow2, Space.World);
        // do Arrow look at Enemy Direction
        if (!arrowSO2.isRoteArrow)
        {
            Vector3 dirPlayerToEnemy = targetPosition - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dirPlayerToEnemy);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 30).eulerAngles;//Time.deltaTime * turnSpeed
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
        //
        if ((targetPosition - transform.position).magnitude <= 0.1f)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("ZZZZZZZZZ       " + other.gameObject.name);
        //check attack
        if (((int)iDOwner - (int)other.gameObject.GetInstanceID()) == 0)
        {
            return;
        }
        //Debug.Log("ZZZZZZZZZ       " + (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player") && ((int)iDOwner - (int)other.gameObject.GetInstanceID()) == 0));
        if ( other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player") && ((int)iDOwner - (int)other.gameObject.GetInstanceID()) != 0)
        {
            //Debug.Log("ZZZZZZZZZ       "+other.gameObject.name);
            GameObject[] enemyArr = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            // Increase rangeAttack and experience if look name Owner of Arrow
            GameObject[] enemiesAll = new GameObject[enemyArr.Length + 1];
            //Debug.Log(enemyArr[0].gameObject.name);
            for (int i = 0; i < enemyArr.Length; i++)
            {
                if (gameObject.GetInstanceID() != enemyArr[i].GetInstanceID())
                {
                    enemiesAll[i] = enemyArr[i];
                    //Debug.Log("==========" + enemiesAll[i].name);
                }
            }
            if(player != null)
            {
                //Check lenght if array no element
                if(enemiesAll.Length > 0)
                {
                    enemiesAll[enemiesAll.Length - 1] = player;
                }
                else
                {
                    enemiesAll[0] = player;
                }
            }
            foreach (GameObject obj in enemiesAll)
            {
                //if (obj.gameObject.CompareTag("Enemy"))
                //{
                //    Debug.Log("ZZZZZZZZZ");
                //}
                if(obj != null)
                {
                    if(obj.GetInstanceID() == iDOwner)
                    {
                        if(obj.GetComponent<Player>() != null)
                        {
                            //Debug.Log(other.gameObject.name);
                            obj.GetComponent<Player>().rangeAttack += obj.GetComponent<Player>().rangeAttack * rangeAdd;
                            //obj.GetComponent<Player>().experience += obj.GetComponent<Player>().experience * experienceAdd;
                            obj.GetComponent<Player>().experience += 2;
                            obj.GetComponent<Player>().isAddExp = true;
                            obj.GetComponent<Player>().isEffect = true;
                            Debug.Log("ZZZZZZZZZ");
                            //obj.GetComponent<Player>().SetDead();
                            obj.GetComponent<Player>().PlayDieAudio();
                            //
                            // Destroy Enemy
                            if (other.GetComponent<Player>() != null)
                            {
                                other.GetComponent<Player>().DieLater();
                            }
                            if (other.GetComponent<Enemy>() != null)
                            {
                                other.GetComponent<Enemy>().DieLater();
                            }
                            //
                            //Destroy(other.gameObject);
                            gameObject.SetActive(false);
                        }
                        if (obj.GetComponent<Enemy>() != null)
                        {
                            obj.GetComponent<Enemy>().rangeAttack += obj.GetComponent<Enemy>().rangeAttack * rangeAdd;
                            obj.GetComponent<Enemy>().experience += 2;
                            //obj.GetComponent<Enemy>().experience += obj.GetComponent<Enemy>().experience * experienceAdd;
                            obj.GetComponent<Enemy>().isAddExp = true;
                            obj.GetComponent<Enemy>().isEffect = true;
                            obj.GetComponent<Enemy>().PlayDieAudio();
                            // Destroy Enemy
                            if (other.GetComponent<Player>() != null)
                            {
                                other.GetComponent<Player>().DieLater();
                            }
                            if (other.GetComponent<Enemy>() != null)
                            {
                                other.GetComponent<Enemy>().DieLater();
                            }
                            //
                            gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        
    }
}
