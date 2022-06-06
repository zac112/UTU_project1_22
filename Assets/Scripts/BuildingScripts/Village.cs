using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village
{
    private static int static_id = 1;
    private int id;  // needed to find a specific village
    public int ID
    { get => id; set => ID = id; }

    private List<Building> buildings;

    public List<Building> Buildings
        {
        get => buildings; set => Buildings = buildings;
        }



    // Start is called before the first frame update
    public Village()
    {
        id = static_id;
        static_id++;
    }

}
