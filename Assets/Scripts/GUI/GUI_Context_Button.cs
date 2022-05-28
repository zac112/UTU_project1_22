using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_ContextButton : MonoBehaviour
{

    [SerializeField]
    GameObject context;

    [SerializeField]
    GameObject GUI_GO;

    void OnMouseDown(){        
        Destroy(GUI_GO.transform.GetChild(0).gameObject);
        GameObject go = Instantiate(context,Vector3.zero,Quaternion.identity);        
        go.transform.SetParent(GUI_GO.transform);
        go.transform.localPosition = Vector3.zero;
    }
}
