using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class FrontOnMap : UIView
    {
        private Button _frontButton;
        private FrontConfig _frontConfig;
        private WorldState _worldState;
        
        public FrontOnMap(VisualElement root, FrontConfig frontConfig, WorldState worldState)
        {
            _frontConfig = frontConfig;
            _worldState = worldState;
            
            Initialize(root);
        }

        protected override void SetVisualElements()
        {
            _frontButton = Root.Q<Button>("front-button");

            _frontButton.text = _frontConfig.name;
        }

        public override void Dispose() {}
    }
}