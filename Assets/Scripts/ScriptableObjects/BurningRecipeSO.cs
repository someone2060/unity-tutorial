using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BurningRecipe", menuName = "Scriptable Objects/Burning Recipe")]
public class BurningRecipeSO : ScriptableObject
{
    public KitchenObjectSO input, output;
    public float burningTimerMax;
}