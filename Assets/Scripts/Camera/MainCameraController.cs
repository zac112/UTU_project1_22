using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCameraController : MonoBehaviour
{
    
    // Control variables for camera movement
    [SerializeField] private float CameraSpeed;
    [SerializeField] private float MouseCameraIgnoreRadius;
    [SerializeField] public bool TargetCamera = true;
    [SerializeField] public bool FreeCamera = false;

    [SerializeField] public Transform target;
    [SerializeField] public float smoothTime = 0.5f;
    [SerializeField] public Vector3 offset;
    [SerializeField] public BuildingPlacementSystem bps;
    GameObject player;
    WASDMovement wasd;

    // Move direction and speed of the camera (not accounting for deltaTime)
    private Vector3 cameraMoveDirection = Vector3.zero;

    // Zoom variables
    float minz;
    float maxz;

    void Start()
    {
        bps = FindObjectOfType<BuildingPlacementSystem>();
        wasd = GameObject.Find("Player").GetComponent<WASDMovement>();       
    }
    
    // Start is called before the first frame update
    public void ChangeCameraMode()
    {
        if(TargetCamera)
        {
            TargetCamera = false;
            FreeCamera = true;
        }
        else if (FreeCamera)
        {
            FreeCamera = false;
            TargetCamera = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (FreeCamera)
        {
            FreeCameraMode();
        }

        else if (TargetCamera)
        {
            TargetCameraMode();
        }
        else
        {
            FreeCameraMode();
        }
    }

    private bool IsMouseOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    void FreeCameraMode()
    {
        wasd.canMove = false;

        if(!IsMouseOverUI()){
        // Max zoomout in freecamera
        minz = 1f;
        maxz = 17f;

        // Avoid NullReferenceException
        if (!Camera.current)
        {
            return;
        }

        // If player is building --> lock zooming
        if (bps.selectedBuilding != null){
            maxz = 7f;
            minz = 7f;
        }

        Rect viewport = Camera.current.pixelRect;
        
        // Check if the mouse cursor is inside the viewport and that mouse control is enabled
        if (!viewport.Contains(Input.mousePosition))
        {
            return;
        }

        // Calculate the mouse x and y position as percentage of the viewport height and width
        // Position is -1 at the left and bottom edges and 1 at the right and top edges 
        Vector3 normalisedMousePosition = new Vector3(2*Input.mousePosition.x / viewport.width - 1,
            2*Input.mousePosition.y / viewport.height - 1, 0);
        
        // Calculate the angle of the mouse position relative to the centre of the viewport
        float mouseAngle = Mathf.Atan2(normalisedMousePosition.y, normalisedMousePosition.x);
        
        // Calculate the distance of the mouse position from the centre of the viewport
        float mouseDistance = Mathf.Sqrt(normalisedMousePosition.x * normalisedMousePosition.x +
                                        normalisedMousePosition.y * normalisedMousePosition.y);
        
        // Calculate the camera movement speed based on the distance of the mouse position from the centre of the viewport
        float cameraMoveSpeed = Mathf.Lerp(0, CameraSpeed, (mouseDistance-MouseCameraIgnoreRadius)/MouseCameraIgnoreRadius);
        
        // Calculate the camera move direction and set its magnitude correctly
        cameraMoveDirection = new Vector3(Mathf.Cos(mouseAngle), Mathf.Sin(mouseAngle), 0);
        cameraMoveDirection = cameraMoveDirection.normalized * cameraMoveSpeed;

        // Move the camera
        Camera.current.transform.Translate(cameraMoveDirection*Time.deltaTime);
    }
    }

    void TargetCameraMode()
    {
        // This mode centers the camera to the player and visibility is lowered(can't zoom as much as in freecamera)
        minz = 1f;
        maxz = 4f;

        // Player movement activated again
        wasd.canMove = true;

        // If player is building --> lock zooming
        if (bps.selectedBuilding != null){
            maxz = 7f;
            minz = 7f;
        }

        Vector3 goPosition = transform.position = target.position + offset;
        offset.z = -10;
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, goPosition, ref cameraMoveDirection, smoothTime);
        transform.position = smoothPosition;
    }

    void OnGUI()
    {
        if (!IsMouseOverUI())
        {
        //Listens to change of Scroll wheel change
        GetComponent<Camera>().orthographicSize+=0.5f*(-Input.mouseScrollDelta.y);
        GetComponent<Camera>().orthographicSize=Mathf.Clamp(GetComponent<Camera>().orthographicSize,minz,maxz);
        }
    }
}
