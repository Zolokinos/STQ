using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class FrontListUI : UIView
    {
        private WorldState _worldState;
        //should it store the fronts?..
        private List<FrontConfig> _currentFronts;
        private VisualTreeAsset _assetUI;
        
        private List<FrontInfoUI> _frontInfos = new();
        
        private ScrollView _frontsView;

        public FrontListUI(VisualElement root, WorldState worldState, IEnumerable<FrontConfig> frontConfigs)
        {
            _worldState = worldState;
            _currentFronts = frontConfigs.ToList();
            _assetUI = Resources.Load<VisualTreeAsset>("UI/front-list");
            
            HideOnAwake = true;
            Initialize(root);
        }

        public void Update(WorldState worldState, IEnumerable<FrontConfig> frontConfigs)
        {
            _worldState = worldState;
            _currentFronts = frontConfigs.ToList();
            UpdateFrontInfos();
        }

        protected override void SetVisualElements()
        {
            var ui = _assetUI.Instantiate();
            Root.Add(ui);
            
            _frontsView = ui.Q<ScrollView>("fronts-view");
            UpdateFrontInfos();
        }

        private void UpdateFrontInfos()
        {
            _frontInfos.Clear();
            _frontsView.Clear();
            foreach (var frontConfig in _currentFronts)
            {
                _frontInfos.Add(new FrontInfoUI(_frontsView, _worldState, frontConfig));
            }
        }
        
        protected override void RegisterButtonCallbacks()
        {
            Root.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        }

        private void OnMouseLeave(MouseLeaveEvent evt)
        {
            Hide();
            Debug.Log("HIDE");
        }

        public override void Dispose() {}
    }
}