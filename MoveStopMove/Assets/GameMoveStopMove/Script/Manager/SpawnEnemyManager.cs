using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyManager : Singleton<SpawnEnemyManager>, IInitializeVariables
{
    //Array of point spaw
    [SerializeField] private GameObject[] arrEnemyPrefabs;
    private int indexPrefabsInArrSpawn;
    private int maxEnemyInMapNow;
    [SerializeField] private Transform[] arrayTransformPoints = new Transform[4];
    private int indextOfCornerHasLeastEnemy;
    private int topLeft;
    private int topRight;
    private int bootLeft;
    private int bootRight;
    //
    private bool isSpawn;
    //Point Center to check 4 coner
    [SerializeField] private Transform pointCenterMapTrans;
    protected override void Awake()
    {
        base.Awake();
    }
    private void OnEnable()
    {
        InitializeVariables();
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Check", 0, 1.5f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Check()
    {
        // Check To Spaw
        if (GameManager.Instance.ListEnemy.Count < maxEnemyInMapNow && isSpawn && indexPrefabsInArrSpawn < arrEnemyPrefabs.Length)
        {
            isSpawn = false;
            SpawEnemy();
        }
        if (!isSpawn)
        {
            isSpawn = true;
            topLeft = 0;
            topRight = 0;
            bootLeft = 0;
            bootRight = 0;
        }
    }
    public void SpawEnemy()
    {
        FindCornerToSpaw();
        GameObject enemySpawed = (GameObject)Instantiate(arrEnemyPrefabs[indexPrefabsInArrSpawn], arrayTransformPoints[indextOfCornerHasLeastEnemy].position, transform.rotation);
        StartCoroutine(delay(enemySpawed));// because conflict order inistalize isPlay two time so delay set bool to actualy isPlay = true
        indexPrefabsInArrSpawn += 1;
        isSpawn = false;
    }
    #region FindCornerToSpaw
    public void FindCornerToSpaw() 
    {
        foreach (var enemy in GameManager.Instance.ListEnemy)
        {
            if (enemy != null)
            {
                if (enemy.transform.position.z < pointCenterMapTrans.position.z && enemy.transform.position.x < pointCenterMapTrans.position.x)
                {
                    topLeft += 1;
                }
                else if (enemy.transform.position.z > pointCenterMapTrans.position.z && enemy.transform.position.x < pointCenterMapTrans.position.x)
                {
                    topRight += 1;
                }
                else if (enemy.transform.position.z > pointCenterMapTrans.position.z && enemy.transform.position.x > pointCenterMapTrans.position.x)
                {
                    bootRight += 1;
                }
                else if (enemy.transform.position.z < pointCenterMapTrans.position.z && enemy.transform.position.x > pointCenterMapTrans.position.x)
                {
                    bootRight += 1;
                }
            }
        }
        // find indextOfCornerHasLeastEnemy
        if (Mathf.Min(topLeft, topRight, bootLeft, bootRight) == topLeft)
        {
            indextOfCornerHasLeastEnemy = 0;
        }
        else if (Mathf.Min(topLeft, topRight, bootLeft, bootRight) == topRight)
        {
            indextOfCornerHasLeastEnemy = 1;
        }
        else if (Mathf.Min(topLeft, topRight, bootLeft, bootRight) == bootRight)
        {
            indextOfCornerHasLeastEnemy = 2;
        }
        else if (Mathf.Min(topLeft, topRight, bootLeft, bootRight) == bootLeft)
        {
            indextOfCornerHasLeastEnemy = 3;
        }
    }
    #endregion
    public void InitializeVariables()
    {
        indexPrefabsInArrSpawn = 0;
        maxEnemyInMapNow = 8;
        isSpawn = true;
        topLeft = 0;
        topRight = 0;
        bootLeft = 0;
        bootRight = 0;
        indextOfCornerHasLeastEnemy = 1;
    }
    IEnumerator delay(GameObject _gameObject)
    {
        yield return new WaitForSeconds(1.5f);
        if(_gameObject != null)
        {
            _gameObject.GetComponent<EnemyMain>().IsPlay = true;
        }
    }
}