using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    private const string PlayerPrefsSoundEffectsVolume = "SoundEffectsVolume";

    [SerializeField] private AudioClipReferencesSO audioClipReferencesSO;

    private float _volume;
    
    private void Awake()
    {
        Instance = this;
    
        _volume = PlayerPrefs.GetFloat(PlayerPrefsSoundEffectsVolume, 1.0f);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlaced += BaseCounter_OnAnyObjectPlaced;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        var trashCounter = sender as TrashCounter;
        PlaySound(audioClipReferencesSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlaced(object sender, EventArgs e)
    {
        var baseCounter = sender as BaseCounter;
        PlaySound(audioClipReferencesSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender, EventArgs e) => 
        PlaySound(audioClipReferencesSO.objectPickup, Player.Instance.transform.position);

    private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
    {
        var cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipReferencesSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e) => 
        PlaySound(audioClipReferencesSO.deliverySuccess, DeliveryCounter.Instance.transform.position);

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e) => 
        PlaySound(audioClipReferencesSO.deliveryFail, DeliveryCounter.Instance.transform.position);

    public void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1.0f) => 
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volumeMultiplier);

    public void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1.0f) => 
        AudioSource.PlayClipAtPoint(audioClip, position, _volume * volumeMultiplier);

    public void PlayFootstepSound(Vector3 position, float volume = 1.0f) => 
        PlaySound(audioClipReferencesSO.footstep, position, volume);

    public void PlayCountdownSound() => 
        PlaySound(audioClipReferencesSO.warning[1], Vector3.zero);

    public void PlayWarningSound(Vector3 position, float volume = 1.0f) => 
        PlaySound(audioClipReferencesSO.warning[0], position, volume);
    
    public float GetVolume() => _volume;

    public void SetVolume(float volume)
    {
        if (volume > 1) volume = 1.0f;
        if (volume < 0) volume = 0.0f;
        _volume = volume;
        
        PlayerPrefs.SetFloat(PlayerPrefsSoundEffectsVolume, _volume);
        PlayerPrefs.Save();
    }
}
