using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLevel1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        PlayerPrefs.SetInt("CurrentLevel", 1);
    }
    IEnumerator qit()
    {
        yield return new WaitForSeconds(30);
        Application.Quit();
    }
}
