using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiDiagram : MonoBehaviour
{
    public enum TileType { Grass, Water, Desert};
    private Dictionary<Vector3, TileType> seeds = new Dictionary<Vector3, TileType>();
    private int numSeeds = 100;
    private int minDistance = 10;
    private int maxDistance = 100;

    private void Awake() {
        CreateDiagram();
    }

    public void CreateDiagram()
    {
        seeds.Add(new Vector3(0, 0, 0), TileType.Grass);        

        for (int i = 0; i < numSeeds; i++) {
            float t = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
            float tt = t * t;
            float x = (1 - tt)/(1+tt);
            float y = (2 * t) / (1 + tt);            
            float r = UnityEngine.Random.Range(minDistance, maxDistance);

            Vector3 vector = new Vector3(x, y, 0);
            vector = Vector3.Scale(vector, new Vector3(r,r,0));

            Array values = Enum.GetValues(typeof(TileType));
            seeds.Add(vector, (TileType)values.GetValue(UnityEngine.Random.Range(0, values.Length)));
        }
    }

    /**
     * Returns the tile type for the given position. Position must be given in world space.
     */
    public TileType GetClosestSeed(Vector3 pos) {
        Vector3 closest = Vector3.positiveInfinity;
        float dist = float.MaxValue;

        foreach (Vector3 v in seeds.Keys)
        {
            if (closest == Vector3.positiveInfinity) {
                dist = Vector3.Distance(pos, v);
                closest = v;
                continue;
            }

            float d = Vector3.Distance(pos, v);
            if (d < dist) {
                closest = v;
                dist = d;
            }
        }

        TileType res;
        if (seeds.TryGetValue(closest, out res))
            return res;
        else
            throw new Exception("No closest seed found in voronoi diagram for position "+pos.ToString());
    }

    public override String ToString() {
        String res = "";
        foreach (Vector3 v in seeds.Keys) {
            TileType t;
            seeds.TryGetValue(v, out t);
            res += v.ToString() + " " + t.ToString()+"\n";
        }
        return res;
    }
}
