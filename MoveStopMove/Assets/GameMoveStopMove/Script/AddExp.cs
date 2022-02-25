using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class AddExp : MonoBehaviour
{
    public float speedGo;
    //Exp Canvas
    [SerializeField] public TextMeshProUGUI txtExp;
    [SerializeField] public Transform canvasExpTrans;
    public float t;
    private void Awake()
    {
        speedGo = 2;
        //Canvas Exp
        txtExp = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        canvasExpTrans = gameObject.GetComponentInChildren<Transform>();
        t = Time.time;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speedGo);
        if((Time.time - t) > 1)
        {
            gameObject.SetActive(false);
        }
    }
    
}
