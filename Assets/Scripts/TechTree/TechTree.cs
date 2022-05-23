using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TechTree
{
    private Node start;

    public TechTree(string startNodeName, string startNodeDescription)
    {
        start = new Node(0, startNodeName, startNodeDescription);
    }
    
    // Tech tree node class
    public class Node
    {
        private Node[] children;
        private Node parent;
    
        // Information about the node, including its cost, name and description
        private bool Unlocked;
        private int CostGold;
        private string Name;
        private string Description;
        
        // If this value is true, unlocking one child node will permanently remove all other child nodes
        // This can be used to pick "paths" of the tech tree that can't be changed later during gameplay
        private bool CollapseChildren;
    
        // Constructor
        public Node(int costGold, string name, string description)
        {
            CostGold = costGold;
            Name = name;
            Description = description;
        }
    
        // Get all children of the node
        public Node[] GetChildren()
        {
            return children;
        }
    
        // Add a new child node
        // Returns the child that was added to the tree
        public Node AddChild(Node child)
        {
            children.Append(child);
            child.parent = this;
            return child;
        }
    
        // Unlock the node
        // Returns a boolean telling whether the unlock was successful or not
        public bool Unlock()
        {
            if (parent.Unlocked)
            {
                Unlocked = true;
                
                // Collapse children of parent node
                if (parent != null && parent.CollapseChildren)
                {
                    parent.children = new[] {this};
                }
            }
    
            return Unlocked;
        }
    }
}
