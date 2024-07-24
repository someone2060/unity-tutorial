using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KitchenObject", menuName = "Scriptable Objects/Kitchen Object")]
public class KitchenObjectSO : ScriptableObject
{
    public Transform prefab;

    public Sprite sprite;

    public string objectName;
}
