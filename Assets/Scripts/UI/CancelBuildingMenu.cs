using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelBuildingMenu : MonoBehaviour
{
    Vector3 start = new Vector3(0,1500,0);
    Vector3 end = new Vector3(0,1300,0);

    RectTransform t = new RectTransform();
    
    private void OnEnable()
    {
        GameEvents.current.BuildingSelectedForBuilding += Begin;
        GameEvents.current.BuildingBuilt += End;
    }

    private void OnDisable()
    {
        GameEvents.current.BuildingSelectedForBuilding -= Begin;
        GameEvents.current.BuildingBuilt -= End;
    }

    private void Begin(GameObject go)
    {
        MoveDown();   
    }

    private void End(GameObject go)
    {
        MoveUp();
    }

    public void MoveDown()
    {
        StartCoroutine(Move(start, end, 2));
    }

    public void MoveUp()
    {
        StartCoroutine(Move(end, start, 2));
    }

    public void Click()
    {
        GameEvents.current.OnBuildingCancelled();
        MoveUp();
    }

    IEnumerator Move(Vector3 from, Vector3 to, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(from,to,t/duration);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
