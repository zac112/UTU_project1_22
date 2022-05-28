using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageHouse1Script : MonoBehaviour, IBuildable
{
    [Tooltip("Width of the building in the axis facing northwest")]
    [SerializeField] private int _width;
    public int Width { get => _width; set => Width = _width; }

    [Tooltip("Length of the building in the axis facing northeast")]
    [SerializeField] private int _length;
    public int Length { get => _length; set => Length = _length; }
}
