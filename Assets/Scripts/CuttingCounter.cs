using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO cutKitchenObjectSO;
    
    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            return;
        }

        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
        }
    }
}
