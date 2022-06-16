using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathVisualizer : MonoBehaviour
{
    AIPathfinding ai;
    GameObject target;
    Collider2D targetCollider;

    private void Start() {
        ai = GetComponent<AIPathfinding>();
        target = GameObject.FindGameObjectWithTag("Player");
        targetCollider = target.GetComponent<Collider2D>();

        ai.setTarget(target.transform);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == target) {          
            Destroy(this.gameObject);
        }
    }
}
