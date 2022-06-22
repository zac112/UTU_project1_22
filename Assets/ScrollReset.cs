using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ScrollReset : MonoBehaviour
{
    public GameObject scrollBar;

    void OnEnable()
    {
        StartCoroutine(resetScrollPos());
    }
    
    IEnumerator resetScrollPos()
    {
        yield return null;
        gameObject.GetComponent<Scrollbar>().value = 1;
    }
}
