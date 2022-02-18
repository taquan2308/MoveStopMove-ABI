using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
// named GameManager2 because do multi Project on One Project, other has GameManager
public class GameManager2 : MonoBehaviour
{
    [HideInInspector] public Transform NearestEnemyFromPlayerTrans;
    [HideInInspector] public Player player;
    [HideInInspector] public bool hasNearestEnemyOfPlayer;
    //Array of point spaw
    public GameObject[] arrEnemyPrefabs;
    public int indexPrefabsInArrSpawn;
    public int numberEnemyInMapNow;
    public Transform[] arrayTransformPoints = new Transform[4];
    [HideInInspector] public int indextOfCornerHasLeastEnemy;
    int topLeft = 0;
    int topRight = 0;
    int bootLeft = 0;
    int bootRight = 0;
    //
    private bool isSpaw;
    //Show count enemy
    public TextMeshProUGUI txtLive;
    public int countEnemy;
    //Canvas Game Over
    public GameObject canvasObj;
    public Button btnRestart;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        NearestEnemyFromPlayerTrans = player.NearestEnemyFromPlayerTrans;
        hasNearestEnemyOfPlayer = false;
        indexPrefabsInArrSpawn = 0;
        numberEnemyInMapNow = 8;
        isSpaw = true;
        //
        topLeft = 0;
        topRight = 0;
        bootLeft = 0;
        bootRight = 0;
        //
        Application.targetFrameRate = 60;
        // canvas
        canvasObj.SetActive(false);
        btnRestart.onClick.AddListener(Restart);
    }

    // Update is called once per frame
    void Update()
    {
        //
        if(player == null)
        {
            //GameOver
            canvasObj.SetActive(true);
            //Time.timeScale = 0;

        }
        //
        countEnemy = GameObject.FindGameObjectsWithTag("Enemy").Length;
        txtLive.text = "Alive: " + countEnemy.ToString();
        //
        //Debug.Log("indexPrefabsInArrSpawn     "+indexPrefabsInArrSpawn);
        GameObject[] enemyarrs = GameObject.FindGameObjectsWithTag("Enemy");
        // Check To Spaw
        if (enemyarrs.Length < 8 && isSpaw == true)
        {
            isSpaw = false;
            SpawEnemy();
        }
        else
        {
            isSpaw = true;
            topLeft = 0;
            topRight = 0;
            bootLeft = 0;
            bootRight = 0;
        }
        //
    }
    public void SpawEnemy()
    {
        // Random a Enemy Type, scriptableObject container list prefabs Enemy
        if (indexPrefabsInArrSpawn < arrEnemyPrefabs.Length)
        {
            indextOfCornerHasLeastEnemy = 1;//instalize Index default Corner 1 TopLeft
            GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
            // Check Position increa
            for (int i = 0; i < enemy.Length; i++)
            {
                if (enemy[i].transform.position.z < 0 && enemy[i].transform.position.x < 0)
                {
                    topLeft += 1;
                }
                else if (enemy[i].transform.position.z > 0 && enemy[i].transform.position.x < 0)
                {
                    topRight += 1;
                }
                else if (enemy[i].transform.position.z > 0 && enemy[i].transform.position.x > 0)
                {
                    bootRight += 1;
                }
                else if (enemy[i].transform.position.z < 0 && enemy[i].transform.position.x > 0)
                {
                    bootRight += 1;
                }
            }
            //Debug.Log(topLeft + "----" + topRight + "----" + bootRight + "----" + bootLeft);
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
            //Spaw
            Instantiate(arrEnemyPrefabs[indexPrefabsInArrSpawn], arrayTransformPoints[indextOfCornerHasLeastEnemy].position, transform.rotation);
            indexPrefabsInArrSpawn += 1;
            isSpaw = false;
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
