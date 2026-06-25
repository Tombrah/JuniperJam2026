using UnityEngine;

[CreateAssetMenu(fileName = "FoodData", menuName = "Scriptable Objects/FoodData")]
public class FoodData : ScriptableObject
{
    public Sprite Icon;
    public string Description = "INSERT ITEM DESCRIPTION";
    public GameObject Prefab;
}
