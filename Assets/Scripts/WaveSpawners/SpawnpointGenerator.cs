using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnpointGenerator : MonoBehaviour
{
    [SerializeField] int amount = 1;
    Tilemap tilemap; // We need the size of the world, so we do not spawn outside of it.
    GameObject[] fogs;

    // Create empty objects at a location covered by fog
    // Attach spawnpoint script to them
    // ??

    private void Awake() {
        // Place spawn points
        instantiateSpawnpoints(amount);
    }

    private void Start() {
        fogs = GameObject.FindGameObjectsWithTag("FogOfWar"); // Returns an  array, when we could use a list
        Debug.Log(fogs.Length);
    }

    private void instantiateSpawnpoints(int amount) {
        GameObject spawnpoints = new GameObject("Spawnpoints");

        for (int i = 0; i < amount; i++) {
            GameObject spawnpointObj = new GameObject($"Spawnpoint{i+1}");
            spawnpointObj.AddComponent<Spawnpoint>();
            spawnpointObj.transform.SetParent(spawnpoints.transform);          
        }
    }

    private void updateLocation() {
        // Find all "FogOfWar(Clone)", pick a random one and place spawnpoint on same pos
        // If too performance heavy, pick a random tile and check if it has fog?
        // Then add logic. Perhaps only allow spawning at a set distance from border


    }
}
