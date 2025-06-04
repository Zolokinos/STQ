using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "WorldStateInitConfig", menuName = "Scriptable Objects/WorldStateInitConfig")]
    public class WorldStateInitConfig : ScriptableObject
    {
        [SerializedDictionary("Tag", "Value")] [SerializeField]
        public SerializedDictionary<FrontTag, bool> frontTags;
    }
}
