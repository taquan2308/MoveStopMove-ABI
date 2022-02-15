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
    public Vector3 enemyPos;
    public GameObject emptyGameObjPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        speedArrow2 = arrowSO2.speedArrow2;
    }

    // Update is called once per frame
    void Update()
    {
        MoveArrow2();
        if(transform.position == targetPosition)
        {
            //GetComponent<RoteItself>().enabled = false;
            Destroy(gameObject, 1);
        }
    }
    public void SetTaget(Transform _targetTrans)
    {
        //GameObject targetPosition = (GameObject) Instantiate(emptyGameObjPrefabs, _targetTrans.position, targetTrans.rotation);
        targetPosition = _targetTrans.position;
        //enemyPos = _targetTrans.position;
        ////targetTrans.position = new Vector3(_targetTrans.position.x, _targetTrans.position.y, _targetTrans.position.z);
        //targetTrans.position = enemyPos;

    }
    public void MoveArrow2()
    {
        dirrectVector3 = targetPosition - transform.position;
        transform.Translate(dirrectVector3 * Time.deltaTime * speedArrow2, Space.World);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
        {
            //Destroy(other.gameObject);
        }
    }
}
