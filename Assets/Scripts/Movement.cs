using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    [Range(0.5f,3f)]
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A)){
            transform.position = transform.position + Vector3.left*speed;
        }
        if(Input.GetKey(KeyCode.D)){
            transform.position = transform.position + Vector3.right*speed;
        }
    }
}
