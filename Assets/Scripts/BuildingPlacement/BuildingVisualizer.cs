using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingVisualizer : MonoBehaviour
{
    public GameObject occupiedVisualizer;
    public GameObject availableVisualizer;
    public GameObject goldMineRangeVisualizer;
    GameObject visualizersParent;

    public List<GameObject> occupiedVisualizers;
    public List<GameObject> availableVisualizers;
    public List<GameObject> goldMineRangeVisualizers;
    
    Tilemap tilemap;
    Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Grid>();
        tilemap = grid.GetComponentInChildren<Tilemap>();
        visualizersParent = new GameObject("Visualizers");
        occupiedVisualizers = InstantiateVisualizers(occupiedVisualizer, 20);
        availableVisualizers = InstantiateVisualizers(availableVisualizer, 20);
        goldMineRangeVisualizers = InstantiateVisualizers(goldMineRangeVisualizer, 25);
    }

    private List<GameObject> InstantiateVisualizers(GameObject visualizer, int amount) {
        List<GameObject> visualizersList = new List<GameObject>();

        for (int i = 0; i < amount; i++) {
            AddVisualizer(visualizersList, visualizer);
        }
        return visualizersList;
    }

    public void MoveVisualizers(List<Vector3> tilesList) {

        int availableIndex = 0;
        int occupiedIndex = 0;

        if (tilesList.Count > availableVisualizers.Count) {
            int amount = tilesList.Count - availableVisualizers.Count;
            AddToVisualizers(availableVisualizers, availableVisualizer, amount);
        }

        if (tilesList.Count > occupiedVisualizers.Count) {
            int amount = tilesList.Count - occupiedVisualizers.Count;
            AddToVisualizers(occupiedVisualizers, occupiedVisualizer, amount);
        }

        foreach (Vector3 tilePosition in tilesList) {

            Vector3Int cellPosition = tilemap.WorldToCell(tilePosition);
            cellPosition.x += 5;
            cellPosition.y += 5;
            cellPosition.z = 0;

            GameObject tile = tilemap.GetInstantiatedObject(cellPosition);

            if (tile != null) {
                GroundTileData tileScript = tile.GetComponent<GroundTileData>();

                if (tileScript.isOccupied || !tileScript.isWalkable) {
                    ActivateVisualizer(occupiedIndex, tilePosition);
                    occupiedIndex += 1;

                }
                else {
                    ActivateVisualizer(availableIndex, tilePosition);
                    availableIndex += 1;
                }
            }
        }
    }

    public void MoveGoldMineRangeVisualizers(List<Vector3> tilesList)
    {
        int goldMineRangeIndex = 0;

        if (tilesList.Count > goldMineRangeVisualizers.Count)
        {
            int amount = tilesList.Count - goldMineRangeVisualizers.Count;
            AddToVisualizers(goldMineRangeVisualizers, goldMineRangeVisualizer, amount);
        }

        foreach (Vector3 tilePosition in tilesList)
        {
            Vector3Int cellPosition = tilemap.WorldToCell(tilePosition);
            cellPosition.x += 5;
            cellPosition.y += 5;
            cellPosition.z = 0;

            GameObject tile = tilemap.GetInstantiatedObject(cellPosition);

            if (tile != null)
            {
                GroundTileData tileScript = tile.GetComponent<GroundTileData>();

                if(tileScript.isWalkable)
                {
                    ActivateGoldMineRangeVisualizer(goldMineRangeIndex, tilePosition);
                    goldMineRangeIndex++;
                }
            }
        }
    }

    public void DeactivateVisualizers() {
        foreach (GameObject visualizer in occupiedVisualizers) visualizer.SetActive(false);
        foreach (GameObject visualizer in availableVisualizers) visualizer.SetActive(false);
        foreach (GameObject visualizer in goldMineRangeVisualizers) visualizer.SetActive(false);
    }

    private void AddVisualizer(List<GameObject> visualizersList, GameObject visualizer) {
        GameObject go = Instantiate(visualizer, Vector3.zero, Quaternion.identity);
        go.SetActive(false);
        go.transform.SetParent(visualizersParent.transform);
        visualizersList.Add(go);
    }

    private void AddToVisualizers(List<GameObject> visualizersList, GameObject visualizer, int amount) {
        for (int i = 0; i < amount; i++) {
            AddVisualizer(visualizersList, visualizer);
        }
    }

    private void ActivateVisualizer(int index, Vector3 tilePosition) {
        GameObject visualizer = occupiedVisualizers[index];
        Vector3 position = visualizer.transform.position;
        position = tilePosition;
        visualizer.transform.position = new Vector3(position.x, position.y, 10);
        visualizer.SetActive(true);
    }

    private void ActivateGoldMineRangeVisualizer(int index, Vector3 tilePosition)
    {
        GameObject visualizer = goldMineRangeVisualizers[index];
        Vector3 position = visualizer.transform.position;
        position = tilePosition;
        visualizer.transform.position = new Vector3(position.x, position.y, 10);
        visualizer.SetActive(true);
    }

}
