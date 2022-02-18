using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArrow : Singeton<SpawnArrow>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject Spawns(GameObject elementUIPrefab)
    {
        var element = LightPool.Instance.GetPrefab(elementUIPrefab);
        return element;
    }
}
