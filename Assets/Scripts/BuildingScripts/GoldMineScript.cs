using EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMineScript : MonoBehaviour
{
    
    [Tooltip("Base level for the amount of gold per minute generated by the mine")] [SerializeField]
    private int miningSpeed;

    // mining efficiency modifier which depends on the amount of nearby gold nodes and other nearby gold mines
    [SerializeField] private float miningEfficiency;
    
    // Progress of gold generation 
    private float goldProgress;

    private PlayerStats playerStats;
    private Tick tickComponent;
    private int tickSpeed;

    private List<GameObject> attachedGoldNodes = new List<GameObject>();



    private void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        if (playerStats == null) { Debug.Log("GoldMineScript: PlayerStats component not found"); }

        tickComponent = GameObject.Find("Game Event System").GetComponent<Tick>();
        if (tickComponent == null) { Debug.Log("GoldMineScript: Game Event System component not found"); }

        tickSpeed = tickComponent.GetTickSpeed();
    }

    private void OnEnable()
    {
        GameEvents.current.Tick += Tick;
    }

    private void OnDisable()
    {
        GameEvents.current.Tick -= Tick;
    }

    private void Tick(int tick)
    {
        float goldPerTick = miningEfficiency * miningSpeed / 60f / tickSpeed;
        goldProgress += goldPerTick;
        
        // If one whole unit of gold has been generated, add it to player statistics
        if (goldProgress >= 1)
        {
            int goldToAdd = Mathf.FloorToInt(goldProgress);
            goldProgress -= goldToAdd;
            playerStats.AddGold(goldToAdd); 
        }
    }
    public void AddMiningSpeed(int speed) {
        this.miningSpeed += speed;
    }

    public List<GameObject> GetAttachedGoldNodes() { return attachedGoldNodes; }

    /// <summary>
    /// Define mining efficiency when this mine or other nearby gold mines are built, based on nearby gold nodes and other gold mines
    /// </summary>
    public void DefineMiningEfficiency()
    {
        List<GameObject> neighboringMines = new();

        foreach (GameObject goldnode in attachedGoldNodes)
        {
            GoldNodeScript goldnodescript = goldnode.GetComponent<GoldNodeScript>();
            List<GameObject> attachedMines = goldnodescript.GetAttachedGoldMines();

            foreach (GameObject mine in attachedMines)
            {
                if (!neighboringMines.Contains(mine) && !ReferenceEquals(gameObject, mine) && mine != null) neighboringMines.Add(mine);
            }
        }            

        /*
         * Mining efficiency grows as a square root function of nearby gold nodes
         * and decreases as an inverse square root function of nearby neighboring mines.
         * The total efficiency is the product of these two factors.
         * If the amount of nearby gold nodes and the amount of attached gold mines (including this one)
         * are the same, the resulting mining efficiency coefficient will be 1.00.
         */
        miningEfficiency = (float)Math.Sqrt((float)attachedGoldNodes.Count) * (float)Math.Sqrt(1.0f / ((float)neighboringMines.Count + 1.0f));

    }

    /// <summary>
    /// Adds to the attached gold nodes of this gold mine
    /// </summary>
    /// <param name="goldnode_as_tile">Goldnode as a tile object (not GoldMine prefab) that contains the GoldNodeScript component</param>
    public void AddToAttachedGoldNodes(GameObject goldnode_as_tile)
    {
        if (!attachedGoldNodes.Contains(goldnode_as_tile)) attachedGoldNodes.Add(goldnode_as_tile);
    }

    /// <summary>
    /// Remove references to the gold mine that will be destroyed.
    /// Nearby gold mines will increase their efficiency.
    /// </summary>
    public void OnDestroy()
    {
        foreach (GameObject goldnode in attachedGoldNodes)
        {
            GoldNodeScript nodescript = goldnode.GetComponent<GoldNodeScript>();
            nodescript.RemoveFromAttachedGoldmines(gameObject);
        }
        foreach (GameObject goldnode in attachedGoldNodes)
        {
            GoldNodeScript nodescript = goldnode.GetComponent<GoldNodeScript>();
            nodescript.UpdateAttachedGoldMinesEfficiency();
        }
    }

    public float GetMiningEfficiency() { return miningEfficiency; }

}
