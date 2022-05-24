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

    [Tooltip("Number of game ticks per second")]
    private static int tickSpeed = 20;

    private void Start()
    {
        tickTimerMax = 1f / tickSpeed;
        DoTick();
    }

    private IEnumerable DoTick() {
        while(true){
            currentTick++;
            OnTick?.Invoke(currentTick);
            yield return new WaitForSeconds(1f/tickSpeed);
        }
    }


    public static int GetTickSpeed()
    {
        return tickSpeed;
    }
}
