using UnityEngine;
[CreateAssetMenu(fileName = "HornSO", menuName = "ScriptableObjects/HornSO")]
public class HornSO : ScriptableObject
{
    public Sprite iconHorn;
    public GameObject prefabsHorn;
    public int priceHorn;
    public int rangeAddHorn;
    public Material materialFullSet;
    public GameObject prefabsWing;
    public GameObject prefabsPan;
    public GameObject prefabsShield;
    public Material materialPan;
    public string nameGroup;
    public string nameItem;
}
