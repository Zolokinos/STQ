using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class FrontOnMapUI : UIView
    {
        private Button _frontButton;
        public FrontConfig _frontConfig;
        private WorldState _worldState;
        
        public FrontOnMapUI(VisualElement root, FrontConfig frontConfig, WorldState worldState)
        {
            _frontConfig = frontConfig;
            HideOnAwake = frontConfig == null;
            _worldState = worldState;
            
            Initialize(root);
        }

        protected override void SetVisualElements()
        {
            if (_frontConfig == null)
                return;
            
            _frontButton = Root.Q<Button>("front-button");

            _frontButton.text = _frontConfig.frontName;
        }

        protected override void RegisterButtonCallbacks()
        {
            if (_frontConfig == null)
                return;
            
            _frontButton.RegisterCallback<ClickEvent>(LoadFront);
        }

        private void LoadFront(ClickEvent evt)
        {
            _worldState.currentFront = _frontConfig;
            SceneManager.LoadScene("Front");
        }
        public override void Dispose() {}
    }
}