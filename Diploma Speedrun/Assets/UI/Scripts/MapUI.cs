using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    public class MapUI : MonoBehaviour
    {
        [SerializeField] private FrontSetConfig _frontsConfig;
        [SerializeField] private WorldState _worldState;
        
        private UIDocument _document;
        
        private List<FrontOnMapUI> _fronts = new(new FrontOnMapUI[Enum.GetNames(typeof(Location)).Length]);
        
        void Awake()
        {
            _document = GetComponent<UIDocument>();
            RefreshFronts();
        }

        private void RefreshFronts()
        {
            for (int i = 0; i < _fronts.Capacity; i++)
            {
                _fronts[i] = (new FrontOnMapUI(
                            _document.rootVisualElement.Q<VisualElement>("front" + i),
                            GetFrontConfig((Location)i),
                            _worldState
                        )
                    );
            }
        }

        //PIECE OF SHIT
        private FrontConfig GetFrontConfig(Location location)
        {
            var currentFront = _fronts.ElementAtOrDefault((int)location)?._frontConfig;
            if (currentFront != null && (_worldState.day - currentFront.day) >= currentFront.stages.Count)
            {
                return currentFront;
            }
            return _frontsConfig.fronts.FirstOrDefault(frontConfig => (frontConfig.location == location && frontConfig.day == _worldState.day));
        }
        
        private bool TryGetFrontConfig(Location location, out FrontConfig frontConfig)
        {
            var currentFront = _fronts.ElementAtOrDefault((int)location)?._frontConfig;
            if (currentFront != null && (_worldState.day - currentFront.day) >= currentFront.stages.Count)
            {
                frontConfig = currentFront;
                return true;
            }

            frontConfig = _frontsConfig.fronts.FirstOrDefault(frontConfig => (frontConfig.location == location && frontConfig.day == _worldState.day));
            return frontConfig != null;
        }
    }
}
