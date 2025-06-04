using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    public class MapUI : MonoBehaviour
    {
        [SerializeField] private FrontSetConfig _frontsConfig;
        [SerializeField] private WorldState _worldState;
        
        private UIDocument _document;

        private List<FrontOnMapUI> _frontsUI;
        
        void Awake()
        {
            _document = GetComponent<UIDocument>();
            _frontsUI = _worldState.frontsUI;
            RefreshFronts();
        }

        private void RefreshFronts()
        {
            for (int i = 0; i < _frontsUI.Capacity; i++)
            {
                _frontsUI[i] = (new FrontOnMapUI(
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
            var currentFront = _frontsUI.ElementAtOrDefault((int)location)?._frontConfig;
            if (currentFront != null && !currentFront.IsCompleted)
            {
                if (currentFront.GetCurrentStage(_worldState) != null)
                    return currentFront;
                currentFront.FiascoAffect(_worldState);
            }
            var  result = _frontsConfig.fronts.FirstOrDefault(
                frontConfig => (frontConfig.location == location && 
                                frontConfig.day == _worldState.day &&
                                frontConfig.AreRequirementsMet(_worldState)
                ));
            return result;
        }
    }
}
