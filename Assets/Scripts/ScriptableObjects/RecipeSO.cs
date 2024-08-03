using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
public class RecipeSO : ScriptableObject
{
    public List<KitchenObjectSO> kitchenObjectSOList;
    public string recipeName;
}