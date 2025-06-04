using System;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldState", menuName = "Scriptable Objects/WorldState")]
public class WorldState : ScriptableObject
{
    [SerializeField] private WorldStateInitConfig worldStateInit;

    [SerializeField] public int day = 0;
    [SerializeField] public Dictionary<FrontTag, bool> frontTags;
    [SerializeField] public FrontConfig currentFront;
    
    public WorldState()
    {
        if (worldStateInit?.frontTags != null)
            frontTags = new(worldStateInit.frontTags);
        else
            frontTags = new();
    }   
}
