using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject()) return;

        if (!player.GetKitchenObject().TryGetPlate(out var plateKitchenObject)) return; // Not a Plate

        DeliveryManager.Instance.TryDeliverRecipe(plateKitchenObject);
        
        player.GetKitchenObject().DestroySelf();
    }

    public override void InteractAlternate(Player player) { }
}
