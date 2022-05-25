using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    // Current event manager used by the game
    // I'm not entirely sure why this is here but the tutorial had this
    public static GameEvents current;

    // Set the current event manager to this instance of the GameEvents class
    private void Awake()
    {
        current = this;
    }

    public event Action<int> onTick;
    public void OnTick(int currentTick)
    {
        onTick?.Invoke(currentTick);
    }
}
