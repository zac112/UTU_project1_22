using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tick : MonoBehaviour
{
    public delegate void TickDelegate(int tickNumber);

    public static TickDelegate OnTick;

    // Tick values
    private static float tickTimer;
    private static float tickTimerMax;
    private static int currentTick = 0;

    [Tooltip("Number of game ticks per second")] [SerializeField]
    private int tickSpeed;

    private void Start()
    {
        tickTimerMax = 1f / tickSpeed;
    }

    private void Update()
    {
        tickTimer += Time.deltaTime;
        // Perform multiple ticks if a lag spike occurs
        while (tickTimer >= tickTimerMax)
        {
            tickTimer -= tickTimerMax;
            currentTick++;
            OnTick?.Invoke(currentTick);
        }
    }
}
