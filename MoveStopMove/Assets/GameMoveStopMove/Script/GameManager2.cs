using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// named GameManager2 because do multi Project on One Project, other has GameManager
public class GameManager2 : MonoBehaviour
{
    public Transform NearestEnemyFromPlayerTrans;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        NearestEnemyFromPlayerTrans = player.NearestEnemyFromPlayerTrans;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
