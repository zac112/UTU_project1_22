using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageHouse1Script : MonoBehaviour, IBuildable
{
    private int _width;
    public int Width { get => _width; set => Width = _width; }

    private int _length;
    public int Length { get => _length; set => Length = _length; }
}
