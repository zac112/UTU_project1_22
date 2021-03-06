using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiDiagram : MonoBehaviour
{
    public enum TileType { Grass, Water, Desert, Beach};
    private Dictionary<Vector3, TileType> seeds = new Dictionary<Vector3, TileType>();
    private Dictionary<Vector3,int> forestSeeds = new Dictionary<Vector3,int>();
    private List<Vector3> goldNodeSeeds = new List<Vector3>();
    private List<Vector3> rainSeeds = new List<Vector3>();
    private List<Vector3> cactusSeeds = new List<Vector3>();
    private List<TileType> spawnableGroundSeeds = new List<TileType> { TileType.Grass, TileType.Water, TileType.Desert };
    private int numSeeds = 100;
    private int minDistance = 10;
    private int maxDistance = 100;
    RandomNoise noise;
    System.Random random;

    public void CreateDiagram(int seed)
    {
        noise = new RandomNoise(seed);
        random = new System.Random(seed);
        InitGroundSeeds();
        InitGoldMineSeeds();
        InitForestSeeds();  
        InitRainSeeds();    
        InitCactusSeeds();  
    }

    private void InitGoldMineSeeds(){
        for (int i = 0; i < numSeeds; i++) {
            float r = (float)(random.NextDouble()*(maxDistance - minDistance) + minDistance);
            float[] pos = GetRandomPosOnMap(r);
            Vector3 vector = new Vector3(pos[0], pos[1], 0);
            goldNodeSeeds.Add(vector);  
        }
    }

    private void InitCactusSeeds(){
        for (int i = 0; i < numSeeds; i++) {
            float r = (float)(random.NextDouble() * (maxDistance - minDistance) + minDistance);
            float[] pos = GetRandomPosOnMap(r);
            Vector3 vector = new Vector3(pos[0], pos[1], 0);
            cactusSeeds.Add(vector);  
        }
    }

    private void InitRainSeeds(){
        for (int i = 0; i < numSeeds; i++) {
            float r = (float)(random.NextDouble() * (maxDistance - minDistance) + minDistance);
            float[] pos = GetRandomPosOnMap(r);
            Vector3 vector = new Vector3(pos[0], pos[1], 0);
            rainSeeds.Add(vector);  
        }
    }
    private void InitGroundSeeds(){
        seeds.Add(new Vector3(0, 0, 0), TileType.Grass);        

        for (int i = 0; i < numSeeds; i++) {

            float r = (float)(random.NextDouble() * (maxDistance - minDistance) + minDistance);
            float[] pos = GetRandomPosOnMap(r);
            Vector3 vector = new Vector3(pos[0], pos[1], 0);

            
            seeds.Add(vector, spawnableGroundSeeds[random.Next(0, spawnableGroundSeeds.Count)]);
        }
    }

    private void InitForestSeeds(){
        int minForestSize = 1;
        int maxForestSize = 4;        
        int minDistance = 2;
        int maxDistance = 200;
        for (int i=0; i <numSeeds; i++){
            float r = (float)(random.NextDouble() * (maxDistance - minDistance) + minDistance);
            float[] pos = GetRandomPosOnMap(r);
            forestSeeds.Add(new Vector3(pos[0],pos[1],0), random.Next(minForestSize,maxForestSize));            
        }
    }

    private float[] GetRandomPosOnMap(float distanceFromOrigin){
        float angle = (float)(random.NextDouble() * 2*Mathf.PI);
        float x = distanceFromOrigin*Mathf.Cos(angle);
        float y = distanceFromOrigin*Mathf.Sin(angle);
        return new float[]{x,y};
    }

    public bool HasGoldNode(Vector3 worldPos){
        foreach(Vector3 pos in goldNodeSeeds){
            if (Vector3.Distance(pos,worldPos) < 0.3f) return true;
        }
        return false;
    }

    public bool HasRain(Vector3 worldPos){
        foreach(Vector3 pos in rainSeeds){
            if (Vector3.Distance(pos,worldPos) < 0.3f) return true;
        }
        return false;
    }

    public bool HasCactus(Vector3 worldPos){
        foreach(Vector3 pos in cactusSeeds){
            if (Vector3.Distance(pos,worldPos) < 0.3f) return true;
        }
        return false;
    }

    /*
    Checks whether the given tilemap cell should have forest.
    If tile does not have forest, returns a negative number.
    If tile has forest, returns a float between 0 and 1.
    */
    public float HasForest(Vector3 worldPos){

        foreach (var (pos, dist) in forestSeeds){
            if (Vector3.Distance(worldPos,pos)<=dist) return noise.Noise2D(worldPos.x,worldPos.y);
        }        
        return -1;
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
