using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    public class Map : MonoBehaviour
    {
        [SerializeField] private FrontSetConfig frontSet;
        [SerializeField] private WorldState worldState;
        
        private UIDocument _document;
        
        private const int FRONTS_AMOUNT = 1;
        private List<FrontOnMapUI> _fronts = new List<FrontOnMapUI>(FRONTS_AMOUNT);
        
        void Awake()
        {
            _document = GetComponent<UIDocument>();
            InitFronts();
        }

        private void InitFronts()
        {
            for (int i = 0; i < _fronts.Capacity; i++)
            {
                _fronts[i] = new FrontOnMapUI(
                    _document.rootVisualElement.Q<VisualElement>("front" + i),
                    GetFrontConfig(i),
                    worldState
                );
            }
        }

        private FrontConfig GetFrontConfig(int locationNumber)
        {
            return frontSet.fronts[locationNumber];
        }
    }
}
