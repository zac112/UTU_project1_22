using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathVisualizer : AIPathfinding
{
    [SerializeField] float speedMod = 0.001f;
    
    protected override void Move(Vector3 from, Vector3 to)
    {
        Vector3 newPos = Vector3.MoveTowards(transform.position, to, GetSpeed()*speedMod);
        float t = 1-(Vector3.Distance(newPos,to)/Vector3.Distance(from,to));
        transform.position = Vector3.Lerp(from, to, t);
    }
}
