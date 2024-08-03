using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject()) return;

        if (!player.GetKitchenObject().TryGetPlate(out var plateKitchenObject)) return; // Not a Plate

        if (!DeliveryManager.Instance.TryDeliverRecipe(plateKitchenObject)) return; // Unable to deliver recipe
        
        player.GetKitchenObject().DestroySelf();
    }

    public override void InteractAlternate(Player player) { }
}
