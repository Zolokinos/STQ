using System;
using System.Collections.Generic;
using DefaultNamespace;
using UI;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldState", menuName = "Scriptable Objects/WorldState")]
public class WorldState : ScriptableObject
{
    [SerializeField] private WorldStateInitConfig worldStateInit;

    [SerializeField] public int day = 0;
    [SerializeField] public Dictionary<FrontTag, bool> tags;
    [SerializeField] public FrontConfig currentFront;
    [NonSerialized]
    public List<FrontOnMapUI> frontsUI = new(new FrontOnMapUI[Enum.GetNames(typeof(Location)).Length]);
    
    public WorldState()
    {
        if (worldStateInit?.frontTags != null)
            tags = new(worldStateInit.frontTags);
        else
            tags = new();
    }   
}
