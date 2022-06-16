using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawEnemyPath : MonoBehaviour
{
    [SerializeField] GameObject pathVisualizer;
    GameObject enemyPathVisualizers;

    private Spawnpoint[] spawnpoints;
    private List<GameObject> visualizersList;

    [SerializeField] float spawnTime = 2f;
    private float currentTimeLeft;
    

    // Start is called before the first frame update
    void Start()
    {
        enemyPathVisualizers = new GameObject("EnemyPathVisualizers");
        visualizersList = new List<GameObject>();

        spawnpoints = GameObject.FindObjectsOfType<Spawnpoint>();
        currentTimeLeft = spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTimeLeft -= Time.deltaTime;
        if (currentTimeLeft < 0) {
            currentTimeLeft = spawnTime + currentTimeLeft;

            foreach (Spawnpoint spawnpoint in spawnpoints) {
                GameObject go = Instantiate(pathVisualizer, spawnpoint.transform.position, Quaternion.identity);
                go.transform.SetParent(enemyPathVisualizers.transform);
                visualizersList.Add(go);
            }
        }

        if (visualizersList.Count > 20) {
            visualizersList.RemoveAt(0);
        }
    }
}
