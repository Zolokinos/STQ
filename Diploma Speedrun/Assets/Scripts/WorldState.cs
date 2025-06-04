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

    [NonSerialized] public int day = 0;
    [NonSerialized] public FrontConfig currentFront;
    [NonSerialized] public Dictionary<FrontTag, bool> tags = new();
    [NonSerialized]
    public List<FrontOnMapUI> frontsUI = new(new FrontOnMapUI[Enum.GetNames(typeof(Location)).Length]);
    
    public WorldState()
    {
        foreach (FrontTag tag in Enum.GetValues(typeof(FrontTag)))
        {
            tags[tag] = false;
        }

        if (worldStateInit?.frontTags != null)
        {
            foreach (FrontTag tag in worldStateInit.frontTags.Keys)
            {
                tags[tag] = worldStateInit.frontTags[tag];
            }
        }
    }
}
