using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class FrontInfoUI : UIView
    {
        private WorldState _worldState;
        private FrontConfig _frontConfig;
        private VisualTreeAsset _assetUI;

        private Label _description;
        private Button _goButton;

        public FrontInfoUI(VisualElement root, WorldState worldState, FrontConfig frontConfig)
        {
            _worldState = worldState ?? throw new ArgumentNullException(nameof(worldState));
            _frontConfig = frontConfig ?? throw new ArgumentNullException(nameof(frontConfig));
            
            Initialize(root);
        }
        
        protected override void SetVisualElements()
        {
            _assetUI = Resources.Load<VisualTreeAsset>("UI/front-info");
            var ui = _assetUI.Instantiate();
            Root.Add(ui);
            
            _description = ui.Q<Label>("description");
            _goButton = ui.Q<Button>("go");
            _goButton.SetEnabled(!_frontConfig.IsCompleted);
            _description.text = _frontConfig.frontName + "\n" + _frontConfig.GetCurrentStage(_worldState).text;
        }

        protected override void RegisterButtonCallbacks()
        {
            _goButton.RegisterCallback<ClickEvent>(LoadFront);
        }

        private void LoadFront(ClickEvent evt)
        {
            _worldState.currentFront = _frontConfig;
            SceneManager.LoadScene("Front", LoadSceneMode.Additive);
        }
        
        public override void Dispose() {}
    }
}