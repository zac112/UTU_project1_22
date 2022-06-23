using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] int food;
    [SerializeField] int maxFood;
    // Start is called before the first frame update
    void Start()
    {
        food = 2;        
        foreach(FarmScript farm in FindObjectsOfType<FarmScript>())
        {
            farm.RegisterHouse(this);
        }
        StartCoroutine(Eat());    
    }

    private void OnDestroy()
    {
        foreach (FarmScript farm in FindObjectsOfType<FarmScript>())
        {
            farm.UnRegisterHouse(this);
        }
    }
    public void Feed()
    {
        food = Mathf.Min(food+1, maxFood);
    }

    IEnumerator Eat() 
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            if (food > 0)
            {
                food--;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
