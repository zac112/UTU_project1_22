using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [Range(0, 1)][SerializeField] float buildingGhostOpacity = 0.5f;

    public List<Vector3> ghostOccupiedTiles;
    public GameObject Ghost;

    BuildingPlacementSystem buildingPlacementSystem;
    BuildingVisualizer buildingVisualizer;

    Coroutine ghostCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        ghostOccupiedTiles = new List<Vector3>();
        buildingPlacementSystem = GameObject.Find("BuildingPlacementSystem").GetComponent<BuildingPlacementSystem>();
        buildingVisualizer = GameObject.Find("BuildingVisualizerSystem").GetComponent<BuildingVisualizer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyGhost(GameObject ghost) {
        if (ghost != null) {
            Destroy(ghost);
            StopCoroutine(ghostCoroutine);
        }
    }

    public void InstantiateGhost(GameObject selectedBuilding, ref GameObject ghost, List<Vector3> ghostOccupiedTiles) {
        DestroyGhost(ghost);

        Vector3 position = buildingPlacementSystem.GetTileLocationInWorld();
        position.z = buildingPlacementSystem.BuildingZ;
        ghost = Instantiate(selectedBuilding, position, Quaternion.identity);

        // Turn off collider
        PolygonCollider2D collider = ghost.GetComponent<PolygonCollider2D>();
        collider.enabled = !collider.enabled;

        // Turn opacity down
        SpriteRenderer spriteComponent = ghost.GetComponentInChildren<SpriteRenderer>();
        spriteComponent.color = new Color(spriteComponent.color.r, spriteComponent.color.g, spriteComponent.color.b, buildingGhostOpacity);

        ghostCoroutine = StartCoroutine(UpdateGhostPosition(ghost, ghostOccupiedTiles));
    }

    public IEnumerator UpdateGhostPosition(GameObject selectedBuilding, List<Vector3> ghostOccupiedTiles) {
        while (true) {
            // Repeating code that exists in the if statement below
            // Make into a method
            buildingPlacementSystem.selectedBuildingScript = selectedBuilding.GetComponent<IBuildable>();
            int buildingWidth = buildingPlacementSystem.selectedBuildingScript.Width;
            int buildingLength = buildingPlacementSystem.selectedBuildingScript.Length;

            Vector3 selectedTileLocationInWorld = buildingPlacementSystem.GetTileLocationInWorld();

            // If building is rotated, switch length and width with eachother
            if (buildingPlacementSystem.selectedBuildingScript.IsRotated) {
                int temp = buildingWidth;
                buildingWidth = buildingLength;
                buildingLength = temp;
            }

            // Calculate tile coordinates that the building will occupy based on selected buildings width and selected building script length
            // Currently, moving NW will modify X by -0.50 and Y by +0.25
            // Moving NE will modify X by +0.50 and Y by +0.25

            // Loop through width and height and add these tiles to tilesOccupiedByBuilding
            // TODO: First check if tiles are already occupied

            float widthX;
            float widthY;

            for (int width = 0; width < buildingWidth; width++) {
                widthX = selectedTileLocationInWorld.x - 0.50f * width;
                widthY = selectedTileLocationInWorld.y + 0.25f * width;

                for (int length = 0; length < buildingLength; length++) {
                    widthX += 0.50f;
                    widthY += 0.25f;

                    ghostOccupiedTiles.Add(new Vector3(widthX, widthY, buildingPlacementSystem.BuildingZ));
                }
            }

            // Move the ghost when mouse moves
            Vector3 position = buildingPlacementSystem.CalculateBuildingLocation(ghostOccupiedTiles);
            position.z = buildingPlacementSystem.BuildingZ;
            Ghost.transform.position = position;

            // Update allowed to build vizualization
            // Change to Vector3Int and add to list
            Vector3 mousePosition = buildingPlacementSystem.GetTileLocationInWorld();
            mousePosition.z = 10;

            if (buildingPlacementSystem.currentMousePositionInWorld != mousePosition) {
                buildingVisualizer.DeactivateVisualizers(buildingVisualizer.availableVisualizers, buildingVisualizer.occupiedVisualizers);

                buildingPlacementSystem.currentMousePositionInWorld = mousePosition;
                buildingVisualizer.MoveVisualizers(ghostOccupiedTiles);
            }

            ghostOccupiedTiles.Clear();
            yield return null;
        }
    }

    public void RotateGhost() {
        buildingPlacementSystem.selectedBuildingScript = buildingPlacementSystem.SelectedBuilding.GetComponent<IBuildable>();

        GameObject nextRotation = buildingPlacementSystem.selectedBuildingScript.NextRotation;
        if (nextRotation != null) {
            // TODO: Same code as in instantiating ghost, except that we are instantiating nextRotation. Make it into a method.            
            Destroy(Ghost);

            Vector3 position = buildingPlacementSystem.GetTileLocationInWorld();
            position.z = buildingPlacementSystem.BuildingZ;
            Ghost = Instantiate(nextRotation, position, Quaternion.identity);

            // Turn opacity down
            SpriteRenderer spriteComponent = Ghost.GetComponentInChildren<SpriteRenderer>();
            spriteComponent.color = new Color(spriteComponent.color.r, spriteComponent.color.g, spriteComponent.color.b, buildingGhostOpacity);

            // Not a part of the repeated code
            buildingPlacementSystem.SelectedBuilding = nextRotation;
        }
        else {
            Debug.Log("Next rotation was null (Next rotation has not been set on the prefab)");
        }
    }
}
