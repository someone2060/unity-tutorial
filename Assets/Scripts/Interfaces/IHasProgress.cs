using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Objects that implement this interface will have some kind of progress behaviour.
 */
public interface IHasProgress
{
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float ProgressNormalized;
    }
}