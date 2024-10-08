using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;

    public new static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }
    
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject()) return;
        
        player.GetKitchenObject().DestroySelf();
        
        OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
    }

    public override void InteractAlternate(Player player) { }
}
