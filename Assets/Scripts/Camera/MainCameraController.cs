using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCameraController : MonoBehaviour
{
    
    // Control variables for camera movement

    [SerializeField] public bool TargetCamera = true;
    [SerializeField] public bool FreeCamera = false;

    [SerializeField] private float CameraSpeed;
    [SerializeField] private float MouseCameraIgnoreRadius;

    [SerializeField] public Transform target;
    [SerializeField] public float smoothTime = 0.5f;
    [SerializeField] public Vector3 offset;
    [SerializeField] public BuildingPlacementSystem bps;
    GameObject player;
    protected WASDMovement wasd;

    // Zoom variables
    protected float minz = 1;
    protected float maxz = 4;

    [SerializeField] float currentZoom = 1;

    void Start()
    {
        bps = FindObjectOfType<BuildingPlacementSystem>();
        StartCoroutine("FindPlayer");        
        StartCoroutine(Zoom());
    }

    IEnumerator FindPlayer() {
        yield return new WaitWhile(() => GameObject.FindGameObjectWithTag("Player") == null);
        wasd = GameObject.FindGameObjectWithTag("Player").GetComponent<WASDMovement>();
        target = wasd.transform;
        SetTargetMode(gameObject);
        StartCoroutine(Control());

        GameEvents.current.BuildingSelectedForBuilding += SetBuildMode;
        GameEvents.current.BuildingBuilt += SetTargetMode;
    }

    private void OnDestroy()
    {
        GameEvents.current.BuildingSelectedForBuilding -= SetBuildMode;
        GameEvents.current.BuildingBuilt -= SetTargetMode;
    }

    private void SetBuildMode(GameObject go)
    {
        StartCoroutine(Control());
        StopCoroutine(FreeControl());

        minz = 7f;
        maxz = 7f;
    }
    private void SetTargetMode(GameObject go)
    {
        StartCoroutine(Control());
        StopCoroutine(FreeControl());

        minz = 1f;
        maxz = 4f;
    }
    private void SetFreeMode()
    {
        StopCoroutine(Control());
        StartCoroutine(FreeControl());
    }

    protected IEnumerator Control() {
        Vector3 cameraMoveDirection = Vector3.zero;
        // Player movement activated again
        yield return new WaitWhile(() => !wasd);
        wasd.canMove = true;

        while (true)
        {
            Vector3 goPosition = transform.position = target.position + offset;
            offset.z = -10;
            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, goPosition, ref cameraMoveDirection, smoothTime);
            transform.position = smoothPosition;
            yield return null;
        }
    }

    protected IEnumerator FreeControl()
    {
        yield return new WaitWhile(() => !wasd);
        while (true)
        {
            wasd.canMove = false;

            yield return new WaitWhile(() => IsMouseOverUI());

            // Avoid NullReferenceException
            while (!Camera.current) yield return null;

            // Check if the mouse cursor is inside the viewport and that mouse control is enabled
            Rect viewport = Camera.current.pixelRect;
            while (!viewport.Contains(Input.mousePosition)) yield return null;

            // Max zoomout in freecamera
            minz = 1f;
            maxz = 17f;

            // If player is building --> lock zooming
            if (bps.SelectedBuilding != null)
            {
                maxz = 7f;
                minz = 7f;

            }

            // Calculate the mouse x and y position as percentage of the viewport height and width
            // Position is -1 at the left and bottom edges and 1 at the right and top edges 
            Vector3 normalisedMousePosition = new Vector3(2 * Input.mousePosition.x / viewport.width - 1,
                2 * Input.mousePosition.y / viewport.height - 1, 0);

            // Calculate the angle of the mouse position relative to the centre of the viewport
            float mouseAngle = Mathf.Atan2(normalisedMousePosition.y, normalisedMousePosition.x);

            // Calculate the distance of the mouse position from the centre of the viewport
            float mouseDistance = Mathf.Sqrt(normalisedMousePosition.x * normalisedMousePosition.x +
                                            normalisedMousePosition.y * normalisedMousePosition.y);

            // Calculate the camera movement speed based on the distance of the mouse position from the centre of the viewport
            float cameraMoveSpeed = Mathf.Lerp(0, CameraSpeed, (mouseDistance - MouseCameraIgnoreRadius) / MouseCameraIgnoreRadius);

            // Calculate the camera move direction and set its magnitude correctly
            Vector3 cameraMoveDirection = new Vector3(Mathf.Cos(mouseAngle), Mathf.Sin(mouseAngle), 0);
            cameraMoveDirection = cameraMoveDirection.normalized * cameraMoveSpeed;

            // Move the camera
            Camera.current.transform.Translate(cameraMoveDirection * Time.deltaTime);
        }

    }

    private void LateUpdate()
    {
        if (!IsMouseOverUI())
        {
            //Listens to change of Scroll wheel change            
            float size = GetComponent<Camera>().orthographicSize;
            currentZoom = Mathf.Clamp(size + 0.5f * (-Input.mouseScrollDelta.y), minz, maxz);
        }
    }

    private IEnumerator Zoom() 
    {
        while (true)
        {
            float size = GetComponent<Camera>().orthographicSize;
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(size, currentZoom, Mathf.Abs(currentZoom - size) / (size)); ;
            yield return null;
        }
    }

    protected bool IsMouseOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }




}
