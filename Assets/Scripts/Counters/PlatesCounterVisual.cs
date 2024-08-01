using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> _plateVisualGameObjectList;
    private const float PlateOffsetY = 0.1f;

    private void Awake()
    {
        _plateVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
    }

    private void PlatesCounter_OnPlateSpawned(object sender, EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        
        plateVisualTransform.localPosition = new Vector3(0, PlateOffsetY * _plateVisualGameObjectList.Count, 0);
        
        _plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}