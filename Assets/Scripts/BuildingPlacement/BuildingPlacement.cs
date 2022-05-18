using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    [SerializeField] private Transform buildingToPlace;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse position in the world
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Make z-coordinate 0
        worldPosition.z = 0;

        /*
        if (Input.GetMouseButtonDown(0)) 
        {
            Instantiate(buildingToPlace, worldPosition, Quaternion.identity);
        }
        */
    }
}
