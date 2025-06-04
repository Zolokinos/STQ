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
                if (worldState.tags.TryGetValue(tag, out var tagValue))
                {
                    if (requirements[tag] != tagValue)
                        return false;
                }
            }
            return true;
        }

        public void Affect(WorldState worldState)
        {
            foreach (var tag in effect.Keys)
                worldState.tags[tag] = effect[tag];
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

        [field: NonSerialized] public bool IsCompleted { get; set; }
        
        public StageConfig GetCurrentStage(WorldState worldState)
        {
            int pastDays = worldState.day - day;
            return stages.ElementAtOrDefault(pastDays);
        }
        
        public bool AreRequirementsMet(WorldState worldState)
        {
            foreach (var tag in requirements.Keys)
            {
                if (worldState.tags.TryGetValue(tag, out var tagValue))
                {
                    if (requirements[tag] != tagValue)
                        return false;
                }
            }
            return true;
        }

        public void FiascoAffect(WorldState worldState)
        {
            foreach (var tag in fiasco.Keys)
                worldState.tags[tag] = fiasco[tag];
            IsCompleted = true;
        }
    }
}

