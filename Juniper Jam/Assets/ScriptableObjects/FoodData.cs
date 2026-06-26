using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodData", menuName = "Scriptable Objects/FoodData")]
public class FoodData : ScriptableObject
{
    public FoodType Type;
    public Sprite Icon;
    public string Description = "INSERT ITEM DESCRIPTION";
    public GameObject Prefab;
}

[Serializable]
public enum FoodType
{
    Pizza,
    MealPrep,
    Soup,
    Egg,
    Popcorn
}
