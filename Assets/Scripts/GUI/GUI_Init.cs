using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Init : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.localPosition = new Vector3(0,-Camera.main.orthographicSize+2,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
