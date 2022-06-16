using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawEnemyPath : MonoBehaviour
{
    [SerializeField] GameObject pathVisualizer;
    GameObject enemyPathVisualizers;

    private Spawnpoint[] spawnpoints;

    [SerializeField] float spawnTime = 2f;
    [SerializeField] GameObject mainBuildingPrefab;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        enemyPathVisualizers = new GameObject("EnemyPathVisualizers");
        spawnpoints = GameObject.FindObjectsOfType<Spawnpoint>();        
    }

    void OnEnable(){
        GameEvents.current.BuildingBuilt += WaitForMainBuilding;
    }

    void OnDisable(){
        GameEvents.current.BuildingBuilt -= WaitForMainBuilding;
    }

    // Update is called once per frame
    void Spawn()
    {
        foreach (Spawnpoint spawnpoint in spawnpoints) {
            GameObject go = Instantiate(pathVisualizer, spawnpoint.transform.position, Quaternion.identity);
            go.transform.SetParent(enemyPathVisualizers.transform);            
            go.GetComponent<AIPathfinding>().setTarget(target.transform);
        }
    }

    void WaitForMainBuilding(GameObject prefab, GameObject go){
        if (prefab!=mainBuildingPrefab)return;

        target = go;
        InvokeRepeating("Spawn", 0, spawnTime);
        OnDisable();
    }
}
