using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Different types of gatherable resources
public enum Resource{
    Wood, Gold
}

public class ResourceNode : MonoBehaviour
{
    public Resource resource;
    public int capacity;

    // Gather resources
    public void Gather(int amount){
        capacity -= amount;

        // If no resource left delete resource
        if (capacity <= 0){
            Destroy(gameObject);
        }
    }
}
