using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour{

    [SerializeField] GameObject target;

    public void Toggle(){
        target.SetActive(!target.activeSelf);
    }
}
