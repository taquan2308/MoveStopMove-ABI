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
    public GameObject emptyGameObjPrefabs;
    private Vector3 offsetPositionTarget;
    private string nameOfArrowOwner;
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
        rangeAdd = 2;
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
    public void SetTaget(Transform _targetTrans, float _rangeAttack, string _nameOfArrowOwner, int _iDOwner)
    {
        //GameObject targetPosition = (GameObject) Instantiate(emptyGameObjPrefabs, _targetTrans.position, targetTrans.rotation);
        targetPosition = _targetTrans.position + offsetPositionTarget;
        rangeAttack = _rangeAttack;
        nameOfArrowOwner = _nameOfArrowOwner;
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
        if((targetPosition - transform.position).magnitude <= 0.1f)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //check attack
        if (((int)iDOwner - (int)other.gameObject.GetInstanceID()) == 0)
        {
            return;
        }
        //Debug.Log("ZZZZZZZZZ       " + (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player") && ((int)iDOwner - (int)other.gameObject.GetInstanceID()) == 0));
        if ( other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player") && ((int)iDOwner - (int)other.gameObject.GetInstanceID()) == 0)
        {
            //Debug.Log("ZZZZZZZZZ       "+other.gameObject.name);
            GameObject[] enemyArr = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            // Increase rangeAttack and experience if look name Owner of Arrow
            GameObject[] enemiesAll = new GameObject[enemyArr.Length];
            for (int i = 0; i < enemyArr.Length - 1; i++)
            {
                if (gameObject.GetInstanceID() != enemyArr[i].GetInstanceID())
                {
                    enemiesAll[i] = enemiesAll[i];
                }
            }
            enemiesAll[enemiesAll.Length - 1] = player;
            foreach (GameObject obj in enemiesAll)
            {
                if(obj != null)
                {
                    if(obj.name == nameOfArrowOwner)
                    {
                        if(obj.GetComponent<Player>() != null)
                        {
                            obj.GetComponent<Player>().rangeAttack += rangeAdd;
                            obj.GetComponent<Player>().experience += experienceAdd;
                            Destroy(other.gameObject);
                            gameObject.SetActive(false);
                        }
                        if (obj.GetComponent<Enemy>() != null)
                        {
                            obj.GetComponent<Enemy>().rangeAttack += rangeAdd;
                            obj.GetComponent<Enemy>().experience += experienceAdd;
                            Destroy(other.gameObject);
                            gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

    }
}
