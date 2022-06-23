using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmScript : MonoBehaviour
{
    [SerializeField] List<House> houses;
    [SerializeField] List<PlotSystem> plots;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Feed());
    }

    IEnumerator Feed() 
    {
        while (true)
        {
            int length = Mathf.Min(houses.Count, plots.Count);
            for (int i = 0; i < length; i++) 
            {
                houses[i].GetComponent<House>().Feed();
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void RegisterPlot(PlotSystem plot)
    {
        if (IsEligible(plot.transform)) plots.Add(plot);
    }

    public void RegisterHouse(House house)
    {
        if(IsEligible(house.transform)) houses.Add(house);
        if (houses.Count >= 5)
        {
            TechnologyUnlocker t = gameObject.AddComponent<TechnologyUnlocker>();
            t.SetUnlock(TechnologyPrerequisite.People5);
        }
    }

    public void UnRegisterHouse(House house)
    {
        houses.Remove(house);
    }

    private bool IsEligible(Transform trans) {
        return Vector3.Distance(trans.position, transform.position) < 5;
        
    }
}
