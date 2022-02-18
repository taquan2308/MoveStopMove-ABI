using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ArrowSO2", menuName = "ScriptableObjects/ArrowSO2")]
public class ArrowSO2 : ScriptableObject
{
    public GameObject arrowPrefabs;
    public float speedArrow2;
    public bool isRoteArrow;
    public int speedRote;
}
