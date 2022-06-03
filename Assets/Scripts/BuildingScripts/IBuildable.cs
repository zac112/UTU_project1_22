using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildable
{
    /*
    // TODO: Add method Build() (if we want the script attached to the building decide
    // when the building can be built. For example, should gold mines only be allowed to
    // be built if gold exist within an x radius.)

    
    int (float?) maxHealth;
    int (float?) currentHealth;
    List<Resources> buildingCost;

    

    CalculateRepairCost() // For each resource pay x amount, depending on currentHealth
    OnDestroy() // Return resources? 
    OnBuild() // ?
    */

    /// <summary>
    /// Width of the building (in the axis that faces towards northwest)
    /// </summary>
    [Tooltip("Width of the building in the axis facing northwest")]
    public int Width { get; set; }

    /// <summary>
    /// Length of the building (in the axis that faces towards northeast)
    /// </summary>
    [Tooltip("Length of the building in the axis facing northeast")]
    public int Length { get; set; }

    /// <summary>
    /// List of tiles this building has occupied
    /// </summary>
    [Tooltip("List of tiles occupied by this building instance")]
    public List<Vector3> OccupiedTiles { get; }

    public void AddToOccupiedTiles(Vector3 tile) 
    {
        OccupiedTiles.Add(tile);
    }
}