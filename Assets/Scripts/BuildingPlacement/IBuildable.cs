using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildable
{
    // TODO: Add method Build() (if we want the script attached to the building decide
    // when the building can be built. For example, should gold mines only be allowed to
    // be built if gold exist within an x radius.)

    /// <summary>
    /// Width of the building (in the axis that faces towards northwest)
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Length of the building (in the axis that faces towards northeast)
    /// </summary>
    public int Length { get; set; }

    
}