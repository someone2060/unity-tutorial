using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FryingRecipe", menuName = "Scriptable Objects/Frying Recipe")]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input, output;
    public float fryingTimerMax;
}