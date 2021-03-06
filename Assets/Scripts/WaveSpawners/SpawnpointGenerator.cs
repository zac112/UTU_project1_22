using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnpointGenerator : MonoBehaviour
{
    [SerializeField] int amount = 1;
    [SerializeField] float minDistance = 1;
    [SerializeField] float maxDistance = 1000;
    [SerializeField] GameObject spawnpointPrefab;
    GameObject player;
    List<GameObject> fogs;
    List<GameObject> spawnpoints;

    private void Awake()
    {
        spawnpoints = new List<GameObject>();

        InstantiateSpawnpoints(amount);
    }

    private void Start()
    {
        fogs = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");

        UpdateLocations();
    }

    private void InstantiateSpawnpoints(int amount)
    {
        GameObject spawnpointsGO = new GameObject("Spawnpoints");

        for (int i = 0; i < amount; i++)
        {
            GameObject spawnpointGO = Instantiate<GameObject>(spawnpointPrefab);
            spawnpointGO.transform.SetParent(spawnpointsGO.transform);
            spawnpoints.Add(spawnpointGO);
        }
    }

    private void UpdateLocations()
    {
        fogs.Clear();

        foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == "FogOfWar(Clone)" || go.name == "FOW")
            {
                fogs.Add(go);
            }
        }

        PlaceSpawnpoints(fogs, spawnpoints);
    }

    private void PlaceSpawnpoints(List<GameObject> fogs, List<GameObject> spawnpoints) {
        float playerDistance;

        List<GameObject> fogsCopy = new List<GameObject>(fogs);
        Shuffle(fogsCopy);

        for (int i = 0; i < spawnpoints.Count; i++)
        {
            for (int j = 0; j < fogsCopy.Count; j++)
            {
                // TODO: check if another spawnpoint is too close
                playerDistance = Vector3.Distance(player.transform.position, fogsCopy[j].transform.position);
                if (playerDistance >= minDistance && playerDistance <= maxDistance)
                {
                    // TODO: if pathfinding finds the player
                    spawnpoints[i].transform.position = fogsCopy[j].transform.position;
                    fogsCopy.Remove(fogsCopy[j]);
                    break;
                }
            }
        }
    }

    private static void Shuffle<T>(List<T> list) {
        int count = list.Count;
        int last = count - 1;
        for (int i = 0; i < last; ++i) {
            int random = UnityEngine.Random.Range(i, count);
            var temp = list[i];
            list[i] = list[random];
            list[random] = temp;
            }
    }

    private void AddToFogs(GameObject fog) {
        fogs.Add(fog);
    }

    private void RemoveFromFogs(GameObject fog) {
        fogs.Remove(fog);
    }
}
