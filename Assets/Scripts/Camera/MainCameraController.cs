using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    
    // Control variables for camera movement
    [SerializeField] private float CameraSpeed;
    [SerializeField] private float MouseCameraIgnoreRadius;
    [SerializeField] bool CameraMouseControlEnabled;
    
    // Move direction and speed of the camera (not accounting for deltaTime)
    private Vector3 cameraMoveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Listens for mouseinput, for whatever reason it doesn't work after the NullReference check.
        listen();
        // Avoid NullReferenceException
        if (!Camera.current)
        {
            return;
        }
        
        Rect viewport = Camera.current.pixelRect;
        
        // Check if the mouse cursor is inside the viewport and that mouse control is enabled
        if (!viewport.Contains(Input.mousePosition) || !CameraMouseControlEnabled)
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
    void listen(){
        //Listens to change of Scroll wheel change
        if (Input.mouseScrollDelta.y != 0)
        {
            GetComponent<Camera>().orthographicSize+=0.5f*(-Input.mouseScrollDelta.y);
        }
    }
}
