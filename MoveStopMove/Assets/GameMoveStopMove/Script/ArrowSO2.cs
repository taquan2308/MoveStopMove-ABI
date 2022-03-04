using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "ArrowSO2", menuName = "ScriptableObjects/ArrowSO2")]
public class ArrowSO2 : ScriptableObject
{
    public GameObject arrowPrefabs;
    public float speedArrow2;
    public bool isRoteArrow;
    public int speedRote;
    public bool isThreeDirection;
    public Sprite iconArrow;
    public int priceArrow;
}
