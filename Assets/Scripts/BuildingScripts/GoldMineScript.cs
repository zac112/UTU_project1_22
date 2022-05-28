using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMineScript : MonoBehaviour, IBuildable
{
    private int _width;
    public int Width { get => _width; set => Width = _width; }

    private int _length;
    public int Length { get => _length; set => Length = _length; }

    [Tooltip("Amount of gold per minute generated by the mine")] [SerializeField]
    private int miningSpeed;

    [Tooltip("Amount of gold stored in the mine")] [SerializeField]
    private int storedGold;
    
    // Progress of gold generation 
    private float goldProgress;

    private void Start()
    {
        GameEvents.current.Tick += Tick;
    }

    private void OnEnable()
    {
        GameEvents.current.Tick += Tick;
    }

    private void OnDisable()
    {
        GameEvents.current.Tick -= Tick;
    }

    private void Tick(int tick)
    {
        float goldPerTick = miningSpeed / 60f / 20f;    // Assuming tick speed is 20. TODO: implement getter for tick speed
        goldProgress += goldPerTick;
        
        // If one whole unit of gold has been generated, add it to storage
        if (goldProgress >= 1)
        {
            int goldToAdd = Mathf.FloorToInt(goldProgress);
            storedGold += goldToAdd;
            goldProgress -= goldToAdd;
        }
    }
}
