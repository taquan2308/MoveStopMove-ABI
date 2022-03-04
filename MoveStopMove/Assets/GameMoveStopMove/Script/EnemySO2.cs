using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemySO2", menuName = "ScriptableObjects/EnemySO2")]
public class EnemySO2 : ScriptableObject
{
    public float rangeAttack;
    public GameObject arrowPrefabs;
    public float speedAttack;
    public float turnSpeed;
    public int experience;
    public float timeIdleStart;
}
