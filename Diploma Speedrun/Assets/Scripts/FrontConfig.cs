using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using DefaultNamespace;

namespace DefaultNamespace
{
    [Serializable]
    public class Choice
    {
        [SerializeField] public string text;

        [SerializedDictionary("Tag", "Value")] [SerializeField]
        public SerializedDictionary<FrontTag, bool> requirements;

        [SerializedDictionary("Tag", "Value")] [SerializeField]
        public SerializedDictionary<FrontTag, bool> effect;

        public bool IsAvailable(WorldState worldState)
        {
            foreach (var tag in requirements.Keys)
            {
                if (worldState.frontTags.TryGetValue(tag, out var tagValue))
                {
                    if (requirements[tag] != tagValue)
                        return false;
                }
                else
                {
                    throw new KeyNotFoundException(worldState + ": not found tag " + tag);
                }
            }
            return true;
        }

        public void AffectWorldState(WorldState worldState)
        {
            foreach (var tag in effect.Keys)
                worldState.frontTags[tag] = effect[tag];
            worldState.day += 1;
        }
    }


    [Serializable]
    public class StageConfig
    {
        [SerializeField] public string text;
        [SerializeField] public List<Choice> choices;
    }


    [CreateAssetMenu(fileName = "FrontConfig", menuName = "Scriptable Objects/FrontConfig")]
    public class FrontConfig : ScriptableObject
    {
        [SerializeField] public string frontName;
        [SerializeField] public int day;

        [SerializedDictionary("Tag", "Value")] [SerializeField]
        public SerializedDictionary<FrontTag, bool> requirements;

        [SerializeField] public Location location;

        [SerializeField] public List<StageConfig> stages;

        [SerializedDictionary("Tag", "Value")] [SerializeField]
        public SerializedDictionary<FrontTag, bool> fiasco;
        
        [SerializeField] public string fiascoText;

        public StageConfig GetCurrentStage(WorldState worldState)
        {
            int pastDays = worldState.day - day;
            return stages.ElementAtOrDefault(pastDays);
        }
    }
}

