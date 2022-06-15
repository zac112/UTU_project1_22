using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingVisualizer : MonoBehaviour
{
    public GameObject occupiedVisualizer;
    public GameObject availableVisualizer;
    GameObject visualizersParent;

    public List<GameObject> occupiedVisualizers;
    public List<GameObject> availableVisualizers;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<GameObject> InstantiateVisualizers(GameObject visualizer, int amount) {
        List<GameObject> visualizersList = new List<GameObject>();

        for (int i = 0; i < amount; i++) {
            AddVisualizer(visualizersList, visualizer);
        }
        return visualizersList;
    }

    public void MoveVisualizers(List<Vector3> ghostOccupiedTiles) {

        int availableIndex = 0;
        int occupiedIndex = 0;

        if (ghostOccupiedTiles.Count > availableVisualizers.Count) {
            int amount = ghostOccupiedTiles.Count - availableVisualizers.Count;
            AddToVisualizers(availableVisualizers, availableVisualizer, amount);
        }

        if (ghostOccupiedTiles.Count > occupiedVisualizers.Count) {
            int amount = ghostOccupiedTiles.Count - occupiedVisualizers.Count;
            AddToVisualizers(occupiedVisualizers, occupiedVisualizer, amount);
        }

        for (int i = 0; i < ghostOccupiedTiles.Count; i++) {

            Vector3 occupiedTile = ghostOccupiedTiles[i];

            Vector3Int cellPosition = tilemap.WorldToCell(occupiedTile);
            cellPosition.x += 5;
            cellPosition.y += 5;
            cellPosition.z = 0;

            GameObject tile = tilemap.GetInstantiatedObject(cellPosition);

            if (tile != null) {
                GroundTileData tileScript = tile.GetComponent<GroundTileData>();

                if (tileScript.isOccupied || !tileScript.isWalkable) {
                    GameObject visualizer = occupiedVisualizers[occupiedIndex];
                    visualizer.transform.position = ghostOccupiedTiles[i];
                    visualizer.SetActive(true);
                    occupiedIndex += 1;

                }
                else {
                    GameObject visualizer = availableVisualizers[availableIndex];
                    visualizer.transform.position = ghostOccupiedTiles[i];
                    visualizer.SetActive(true);
                    availableIndex += 1;
                }
            }
        }
    }

    public void DeactivateVisualizers() {
        for (int i = 0; i < occupiedVisualizers.Count; i++) {

            GameObject visualizer = occupiedVisualizers[i];

            if (visualizer.activeSelf == true) {
                visualizer.SetActive(false);
            }
        }

        for (int i = 0; i < availableVisualizers.Count; i++) {

            GameObject visualizer = availableVisualizers[i];

            if (visualizer.activeSelf == true) {
                visualizer.SetActive(false);
            }
        }
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
}
