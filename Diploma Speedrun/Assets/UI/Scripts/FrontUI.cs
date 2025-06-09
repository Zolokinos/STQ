using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class FrontUI : UIView
    { 
        private WorldState _worldState;
        private FrontConfig _frontConfig;

        private VisualTreeAsset _assetUI;
        
        private List<FrontChoiceUI> _choices = new();
        private Button _skip;
        private Label _description;
        private StageConfig _currentStage;
        
        public FrontUI(VisualElement root, WorldState worldState, FrontConfig frontConfig)
        { 
            _assetUI = Resources.Load<VisualTreeAsset>("front");
            _worldState = worldState;
            _frontConfig = frontConfig;
            _currentStage = _frontConfig.GetCurrentStage(_worldState) ?? throw new ArgumentNullException(nameof(_frontConfig));
            
            Initialize(root);
        }

        protected override void SetVisualElements()
        {
            _assetUI.CloneTree(Root);
            _skip = Root.Q<Button>("skip");
            _description = Root.Q<Label>("description");
            _description.text = _currentStage.text;

            foreach (var choice in _currentStage.choices)
            {
                _choices.Add(new FrontChoiceUI(
                    //should be refactored
                    Root.Q<VisualElement>("root"),
                    choice,
                    _worldState
                ));
            }
        }

        protected override void RegisterButtonCallbacks()
        {
            _skip.RegisterCallback<ClickEvent>(OnSkip);
        }

        private void OnSkip(ClickEvent evt)
        {
            _worldState.Day += 1;
            Bus<FrontLoad>.Event?.Invoke(false);
        }

        public override void Dispose() {}
    }
}