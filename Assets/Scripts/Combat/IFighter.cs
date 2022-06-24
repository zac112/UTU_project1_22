using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFighter
{
    public void AddTarget(CombatTarget target);
    public void RemoveTarget(CombatTarget target);
}

