using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoteItself : MonoBehaviour
{
    [HideInInspector] public Arrow2 arrow2;
    [HideInInspector] public bool isRoteArrow;
    public GameObject objectRote;
    [HideInInspector] public int speedRote;
    private void Awake()
    {
        arrow2 = GetComponent<Arrow2>();
        isRoteArrow = arrow2.arrowSO2.isRoteArrow;
        speedRote = arrow2.arrowSO2.speedRote;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RoteAround();
    }
    public void RoteAround()
    {
        objectRote.transform.RotateAround(objectRote.transform.position, Vector3.up, speedRote * Time.deltaTime);
    }
}
