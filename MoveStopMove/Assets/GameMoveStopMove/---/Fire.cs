using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Singeton<Fire>
{
    public GameObject prefabs;
    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("Spawn", 0 , 1);
    }

    // Update is called once per frame
    void Update()
    {
        Spawn();
    }
    IEnumerator deactive(GameObject gameObject)
    {

        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }
    public void Spawn()
    {
        var element = LightPool.Instance.GetPrefab(prefabs);
        element.transform.localPosition = transform.position + new Vector3(-10, 0, 0);
        StartCoroutine(deactive(element));
    }
}
