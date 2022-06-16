using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathVisualizer : MonoBehaviour
{
    AIPathfinding ai;
    GameObject target;    

    private void Start() {
        ai = GetComponent<AIPathfinding>();
        target = ai.getTarget().gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == target) {          
            Destroy(this.gameObject);
        }
    }
}
