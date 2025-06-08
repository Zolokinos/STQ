using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using JetBrains.Annotations;
using NUnit.Framework.Constraints;

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
        
        [field: NonSerialized] public event Action<WorldState> Affected;
        
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
            //DANGEROUS POINT. WHAT SHOULD BE FIRST: Affected or global Bus event?
            Bus<StateChanged<FrontTag>>.Event?.Invoke(worldState);
            Affected?.Invoke(worldState);
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
        public SerializedDictionary<FrontTag, bool> requirements = new();

        [SerializeField] public Location location = new();

        [SerializeField] public List<StageConfig> stages = new();

        [SerializedDictionary("Tag", "Value")] [SerializeField]
        public SerializedDictionary<FrontTag, bool> fiasco = new();
        
        [SerializeField] public string fiascoText;

        [NonSerialized] private bool _isCompleted;
        public bool IsCompleted
        {
            get
            {
                return _isCompleted;
            }
            private set
            {
                _isCompleted = value;
                if (_isCompleted)
                {
                    Completed?.Invoke(this);
                }
            }
        }

        [field: NonSerialized] [CanBeNull] public event Action<WorldState> FiascoAffected;
        [field: NonSerialized] [CanBeNull] public event Action<WorldState> ChoiceAffected;
        [field: NonSerialized] [CanBeNull] public event Action<FrontConfig> Completed;

        public FrontConfig()
        {
            foreach (var stage in stages)
            {
                foreach (var choice in stage.choices)
                {
                    choice.Affected += worldState =>
                    {
                        IsCompleted = true;
                        ChoiceAffected?.Invoke(worldState);
                    };
                }
            }
        }
        
        public StageConfig GetCurrentStage(WorldState worldState)
        {
            int pastDays = worldState.Day - day;
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
            Bus<StateChanged<FrontTag>>.Event?.Invoke(worldState);
            FiascoAffected?.Invoke(worldState);
        }

        public void OnDayChanged(WorldState worldState)
        {
            if (!IsCompleted && GetCurrentStage(worldState) == null)
                FiascoAffect(worldState);
        }
    }
}

