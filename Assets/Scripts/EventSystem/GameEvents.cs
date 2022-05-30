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

    public event Action<int> Tick;
    public void OnTick(int currentTick)
    {
        Tick?.Invoke(currentTick);
    }

    public event Action<Vector2, Vector2> MovementInputChanged;
    public void OnMovementInputChanged(Vector2 input, Vector2 delta)
    {
        MovementInputChanged?.Invoke(input, delta);
    }

    public event Action<Vector3, Vector3> MouseMoved;
    public void OnMouseMoved(Vector3 position, Vector3 delta)
    {
        MouseMoved?.Invoke(position, delta);
    }

    public event Action<bool> GameOver;
    public void OnGameOver(bool over)
    {
        // launch end scene
    }


}
