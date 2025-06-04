using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEditor.Analytics;
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
        [SerializeField] private VisualTreeAsset _endMenu;
        
        private UIDocument _document;

        private InfoUI _infoUI;
        private List<FrontOnMapUI> _frontsUI;
        
        void Awake()
        {
            _document = GetComponent<UIDocument>();
            _frontsUI = _worldState.frontsUI;

            FindFiasco();
            
            if (_worldState.day >= 4)
            {
                EndGame();
                return;
            }
            
            RefreshInfo();
            RefreshFronts();
        }
        
        private void RefreshInfo()
        {
            _infoUI = new InfoUI(
                _document.rootVisualElement.Q<VisualElement>("info"),
                _worldState
            );
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
        private void FindFiasco()
        {
            foreach (var frontUI in _frontsUI)
            {
                var currentFront = frontUI?._frontConfig;
                if (currentFront == null)
                    continue;
                if (currentFront.GetCurrentStage(_worldState) == null && !currentFront.IsCompleted)
                    currentFront.FiascoAffect(_worldState);
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
            }
            var  result = _frontsConfig.fronts.FirstOrDefault(
                frontConfig => (frontConfig.location == location && 
                                frontConfig.day == _worldState.day &&
                                frontConfig.AreRequirementsMet(_worldState)
                ));
            return result;
        }

        private void EndGame()
        {
            _document.rootVisualElement.Clear();
            var endMenu = _endMenu.Instantiate();
            var label = endMenu.Q<Label>("end-message");
            if (_worldState.tags[FrontTag.Смерть])
            {
                label.text = "ВАС ОТПРАВИЛИ НА ВОЙНУ";
            }
            else
            {
                label.text = "";
            }
            _document.rootVisualElement.Add(endMenu);
        }
    }
}
