using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArrow : Singeton<SpawnArrow>
{
    public GameObject Spawns(GameObject elementUIPrefab)
    {
        var element = LightPool.Instance.GetPrefab(elementUIPrefab);
        return element;
    }
}
