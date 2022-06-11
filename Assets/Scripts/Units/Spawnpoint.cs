using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{

    [SerializeField]
    List<UnitBase> unitsToSpawn = new List<UnitBase>();
    int spawntime = 1;
    
    List<GameObject> unitPrefabs = new List<GameObject>();
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        foreach(String t in Enum.GetNames(typeof(EnemyType))){
            GameObject go = Resources.Load<GameObject>("Units/"+t);
            unitPrefabs.Add(go);
        }
        StartCoroutine("Spawn");        
    }

    // Update is called once per frame
    IEnumerator Spawn()
    {
        while (true){
            yield return new WaitWhile(() => unitsToSpawn.Count == 0);
            yield return new WaitForSeconds(spawntime); 
             
            UnitBase unit = unitsToSpawn[0];
            unitsToSpawn.RemoveAt(0);
            
            GameObject go = Instantiate(unitPrefabs[(int)unit.type]);
            go.transform.position = transform.position;
            go.GetComponent<AIPathfinding>()?.setTarget(player.transform);
        }
    }

    public void SpawnUnit(UnitBase unit){
        unitsToSpawn.Add(unit);
    }
}
