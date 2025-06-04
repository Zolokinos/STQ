using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "FrontSetConfig", menuName = "Scriptable Objects/FrontSetConfig")]
    public class FrontSetConfig : ScriptableObject
    {
        [SerializeField] public List<FrontConfig> fronts;
    }
}

