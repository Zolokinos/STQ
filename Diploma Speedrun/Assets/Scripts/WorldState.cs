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

    [NonSerialized] private int _day;
    public int Day
    {
        get
        {
            return _day;
        }
        set
        {
            _day = value;
            DayChanged?.Invoke(this);
        }
    }

    [NonSerialized] public FrontConfig currentFront;
    [NonSerialized] public Dictionary<FrontTag, bool> tags = new();

    public event Action<WorldState> DayChanged;
    
    //NOT WORKING
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
