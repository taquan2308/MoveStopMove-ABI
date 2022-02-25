using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPoints : MonoBehaviour
{
    //Creat Point Random Around this Object
    [HideInInspector] public Vector3 pointRandomAroundThisObject;
    [HideInInspector] public float radiusOut;
    [HideInInspector] public float radiusIn;
    // instalize Area Clamp Position
    private float maxPosX;
    private float maxPosZ;
    private float minPosX;
    private float minPosZ;
    // Start is called before the first frame update
    void Start()
    {
        // instalize radius point radom
        radiusOut = 15;
        radiusIn = 6;
        // Set Area Clamp Position
        maxPosX = 40;
        maxPosZ = 10;
        minPosX = -10;
        minPosZ = -40;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Vector3 GetPointRandomAroundThisObject()
    {
        Vector3 directionRandom = new Vector3(Random.Range(-1000, 1000), 0, Random.RandomRange(-1000, 1000)).normalized;
        float rangeRandom = Random.Range(radiusIn, radiusOut);
        pointRandomAroundThisObject = transform.position + rangeRandom * directionRandom;
        // Clamp Position
        var pos = pointRandomAroundThisObject;
        pos.x = Mathf.Clamp(pointRandomAroundThisObject.x, minPosX, maxPosX);
        pos.z = Mathf.Clamp(pointRandomAroundThisObject.z, minPosZ, maxPosZ);
        pointRandomAroundThisObject = pos;
        //
        return pointRandomAroundThisObject;
    }
}
