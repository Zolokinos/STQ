using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using DefaultNamespace;

namespace DefaultNamespace
{
    [Serializable]
    public class Choice
    {
        [SerializeField] public String text;

        [SerializedDictionary("Tag", "Value")] [SerializeField]
        public SerializedDictionary<FrontTag, bool> requirements;

        [SerializedDictionary("Tag", "Value")] [SerializeField]
        public SerializedDictionary<FrontTag, bool> effect;
    }


    [Serializable]
    public class StageConfig
    {
        [SerializeField] public String text;
        [SerializeField] public List<Choice> choices;
    }


    [CreateAssetMenu(fileName = "FrontConfig", menuName = "Scriptable Objects/FrontConfig")]
    public class FrontConfig : ScriptableObject
    {
        [SerializeField] public String frontName;
        [SerializeField] public int day;

        [SerializedDictionary("Tag", "Value")] [SerializeField]
        public SerializedDictionary<FrontTag, bool> requirements;

        [SerializeField] public Location location;

        [SerializeField] public List<StageConfig> stages;

        [SerializedDictionary("Tag", "Value")] [SerializeField]
        public SerializedDictionary<FrontTag, bool> fiasco;
        
        [SerializeField] public String fiascoText;
    }
}