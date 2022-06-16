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

    public event Action<GameObject> BuildingSelectedForBuilding;
    public void OnBuildingSelected(GameObject building){ BuildingSelectedForBuilding?.Invoke(building); }

    public event Action<Vector3,int> MapChange;
    public void OnMapChanged(Vector3 worldPos, int size){ MapChange?.Invoke(worldPos, size); }  

    public event Action<int> Tick;
    public void OnTick(int currentTick){ Tick?.Invoke(currentTick); }

    public event Action<Vector2, Vector2> MovementInputChanged;
    public void OnMovementInputChanged(Vector2 input, Vector2 delta){ MovementInputChanged?.Invoke(input, delta); }

    public event Action<Vector3, Vector3> MouseMoved;
    public void OnMouseMoved(Vector3 position, Vector3 delta) { MouseMoved?.Invoke(position, delta); }

    public event Action<GameOverType> GameOver;
    public void OnGameOver(GameOverType type){ GameOver?.Invoke(type);}

    //When player attempts to buy a new technology
    public event Action<Technology> TechnologyPurchaseAttempt;
    public void OnTechnologyPurchase(Technology tech) { TechnologyPurchaseAttempt?.Invoke(tech); }

    //When player unlocks a new tech
    public event Action<TechnologyPrerequisite> TechnologyUnlock;
    public void OnTechnologyUnlock(TechnologyPrerequisite tech) { TechnologyUnlock?.Invoke(tech); }

    public event Action<GameObject> FogSpawned;
    public void OnFogSpawned(GameObject fog) { FogSpawned?.Invoke(fog);  }

    public event Action<GameObject> FogDespawned;
    public void OnFogDespawned(GameObject fog) { FogDespawned?.Invoke(fog); }

    public event Action<PlayerSkills.SkillType> OnSkillUnlockedEvent;
    public void OnSkillUnlocked(PlayerSkills.SkillType skillType){ OnSkillUnlockedEvent?.Invoke(skillType);}

}
