using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Technology", menuName = "Technology", order = 3)]
public class Technology : ScriptableObject
{
    public List<TechnologyPrerequisite> prereqs;
    public TechnologyPrerequisite unlocksPreq;
    public string techName;
    public int cost;
    public Sprite image;


}
