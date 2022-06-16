using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DragWallBuilding : MonoBehaviour
{
    // Click mouse and hold
    // Get first tile, add to list and add visualization
    // If mouse moves to a tile already in the list other than current
    //      remove all tiles that have been added after it
    //      update visuals
    // If mouse moves to another tile
    //      get that tile and add it to the list
    //      add visualization
    // On release, place walls on visualizers according to the rules
    //      If straight, add straigth piece
    //      If corner, add corner piece
    //      If crossing, add crossing piece
    //      (For now, corners and crossings should just be a tower)

    Grid grid;
    Tilemap tilemap;
    BuildingPlacementSystem bps;
    BuildingVisualizer buildingVisualizer;

    [SerializeField] GameObject occupiedVisualizer;
    [SerializeField] GameObject availableVisualizer;
    [SerializeField] GameObject wallTestingPrefab;

    public List<Vector3> wallPositions;

    public List<Vector3> startNW;
    public List<Vector3> startNE;
    public List<Vector3> startSE;
    public List<Vector3> startSW;
    public List<Vector3> targetNW;
    public List<Vector3> targetNE;
    public List<Vector3> targetSE;
    public List<Vector3> targetSW;

    private List<List<Vector3>> startDirections;
    private List<List<Vector3>> targetDirections;
    public List<List<Vector3>> foundPaths;

    Vector3 currentMousePositionInWorld;
    Vector3 startingPosition;
    Vector3 NW;
    Vector3 NE;
    Vector3 SE;
    Vector3 SW;

    bool placingWalls;
    bool foundCorner;
    bool buildingAllowed;

    void Start() {
        grid = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Grid>();
        tilemap = grid.GetComponentInChildren<Tilemap>();
        bps = GameObject.Find("BuildingPlacementSystem").GetComponent<BuildingPlacementSystem>();
        wallPositions = new List<Vector3>();
        buildingVisualizer = GameObject.Find("BuildingVisualizerSystem").GetComponent<BuildingVisualizer>();
        placingWalls = false;        
        foundCorner = false;
        buildingAllowed = true;

        startNW = new List<Vector3>();
        startNE = new List<Vector3>();
        startSW = new List<Vector3>();
        startSE = new List<Vector3>();

        targetNW = new List<Vector3>();
        targetNE = new List<Vector3>();
        targetSE = new List<Vector3>();
        targetSW = new List<Vector3>();

        startDirections = new List<List<Vector3>>() { 
            startNW,
            startNE,
            startSE,
            startSW,    
        };
        targetDirections = new List<List<Vector3>>() { 
            targetNW,
            targetNE,
            targetSE,
            targetSW,           
        };

        foundPaths = new List<List<Vector3>>();

        // NW X: -0.50, Y: +0.25
        // NE X: +0.50, Y: +0.25
        // SE X: +0.50, Y: -0.25
        // SW X: -0.50, Y: -0.25

        NW = new Vector3(-0.50f, 0.25f, 0);
        NE = new Vector3(0.50f, 0.25f, 0);
        SE = new Vector3(0.50f, -0.25f, 0);
        SW = new Vector3(-0.50f, -0.25f, 0);

    }

    void Update() {
        if (Input.GetMouseButtonDown(1) && placingWalls) {
            if (buildingAllowed) BuildWalls(wallPositions);
            else Debug.Log("Building was not allowed");

            placingWalls = false;
            wallPositions.Clear();
            buildingVisualizer.DeactivateVisualizers();
        }

        else if (Input.GetMouseButtonDown(1) && !placingWalls) {
            wallPositions.Clear();

            // Position where mouse was clicked
            Vector3 cellPosition = bps.GetTileLocationInWorld();
            //cellPosition.z = 10;
            startingPosition = cellPosition;

            // Add first position to the list
            wallPositions.Add(cellPosition);

            // Set placing walls to true
            placingWalls = true;
        }

        else if (placingWalls == true) {
            //Get current world position of the tile the mouse is on
            //Debug.Log("True");
            Vector3 mousePosition = bps.GetTileLocationInWorld();
            //mousePosition.z = 10;

            // If mouse is moved to a new tile
            // If startingPosition != current mouse position, remove all but first from wallpositions, add all again and recalculate visualizers
            if (mousePosition != currentMousePositionInWorld) {
                foundCorner = false;
                wallPositions.Clear();

                foreach (List <Vector3> startDirection in startDirections) startDirection.Clear();

                foreach (List<Vector3> targetDirection in targetDirections) targetDirection.Clear();

                //RemoveAllAfterIndex(wallPositions, 0);
                buildingVisualizer.DeactivateVisualizers();

                currentMousePositionInWorld = mousePosition;

                int index = 0;

                while (!foundCorner) {
                    if (index >= 100) {
                        Debug.Log("Index reached 100");
                        break;
                    }

                    CalculateNextVector3(startNW, NW, true);
                    CalculateNextVector3(startNE, NE, true);
                    CalculateNextVector3(startSE, SE, true);
                    CalculateNextVector3(startSW, SW, true);
                    CalculateNextVector3(targetNW, NW, false);
                    CalculateNextVector3(targetNE, NE, false);
                    CalculateNextVector3(targetSE, SE, false);
                    CalculateNextVector3(targetSW, SW, false);

                    CheckCollisions(startDirections, targetDirections);
                    CheckCollisions(targetDirections, startDirections);

                    index++;
                }

                wallPositions = foundPaths[0];

                buildingVisualizer.MoveVisualizers(wallPositions);

                // Check if tiles are occupied
                CheckForOccupiedTiles(wallPositions);

                foundPaths.Clear();
            }
        }
    }

    private void RemoveAllAfterIndex<T>(List<T> list, int index) {
        for (int i = index + 1; i < list.Count; i++) {
            list.RemoveAt(i);
        }
    }

    private void CalculateNextVector3(List<Vector3> list, Vector3 direction, bool startFromStart) {
        if (list.Count == 0) {
            {
                if (startFromStart) {
                    list.Add(startingPosition + direction);
                }
                else {
                    list.Add(currentMousePositionInWorld + direction);
                }
            }
        }
        else {
            Vector3 last = list[list.Count - 1];
            Vector3 next = last + direction;
            list.Add(next);
        }
    }

    private void CheckCollisions(List<List<Vector3>> listToCheckFrom, List<List<Vector3>> listToCheck) {
        foreach (List<Vector3> list1 in listToCheckFrom) {
            Vector3 last = list1[list1.Count - 1];
            foreach (List<Vector3> list2 in listToCheck) {
                if (list2.Contains(last)) {
                    // Remove all instances after last
                    int index = list2.IndexOf(last);
                    if (index < list2.Count - 1) {
                        for (int i = index + 1; i < list2.Count; i++) {
                            list2.RemoveAt(i);
                            i--;
                        }
                    }

                    List<Vector3> combinedList = new List<Vector3>();
                    combinedList.AddRange(list1);
                    combinedList.AddRange(list2);
                    combinedList.Add(startingPosition);
                    combinedList.Add(currentMousePositionInWorld);
                    foundPaths.Add(combinedList);
                    foundCorner = true;
                    break;

                    // We should always get two paths, but don't know
                    // if we need to create logic to get the "best path"
                    // For now we just use the first one
                }
            }
        }
    }

    private void CheckForOccupiedTiles(List<Vector3> list) {
        foreach (var item in list) {
            Vector3Int position = tilemap.WorldToCell(item);
            // TODO: Fix these values
            position.x -= 5;
            position.y -= 5;
            position.z = 0;
            GameObject tile = tilemap.GetInstantiatedObject(position);
            if (tile != null) {
                GroundTileData tileScript = tile.GetComponent<GroundTileData>();
                if (tileScript.isOccupied) {
                    buildingAllowed = false;
                    break;
                }
            }  
        }
    }

    private void BuildWalls(List<Vector3> positionsList) {
        foreach (var p in positionsList) {
            // Check if last piece was at NW/SW or NE/SE
            // Turn piece according to above result
            // Instantiate wall piece
            Vector3 position = new Vector3(p.x, p.y, 10); 
            Instantiate(wallTestingPrefab, position, Quaternion.identity);
        }
    }
}