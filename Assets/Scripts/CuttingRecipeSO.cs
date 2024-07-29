using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CuttingRecipe", menuName = "Scriptable Objects/Cutting Recipe")]
public class NewBehaviourScript : ScriptableObject
{
    public KitchenObjectSO input, output;
}
