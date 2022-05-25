using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tick : MonoBehaviour
{
    // Tick values
    private float tickTimer;
    private float tickTimerMax;
    private int currentTick = 0;

    [Tooltip("Number of game ticks per second")]
    private int tickSpeed = 20;

    private IEnumerator coroutine;
    
    private void Start()
    {
        coroutine = DoTick();
        StartCoroutine(coroutine);
    }

    private IEnumerator DoTick() {
        while(true){
            currentTick++;
            GameEvents.current.OnTick(currentTick);
            yield return new WaitForSeconds(1f/tickSpeed);
        }
    }
    
    public int GetTickSpeed()
    {
        return tickSpeed;
    }
}
