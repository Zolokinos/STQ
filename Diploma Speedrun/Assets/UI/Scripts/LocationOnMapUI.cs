using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    //It keeps current fronts info. It should be changed, because I mix UI logic and Game logic
    public class LocationOnMapUI : UIView
    {
        private Button _button;
        private WorldState _worldState;
        private Location _location;
        private HashSet<FrontConfig> _frontConfigs;
        private HashSet<FrontConfig> _currentFronts = new();
        private FrontListUI _frontList;
        
        public LocationOnMapUI(VisualElement root, WorldState worldState, Location location, FrontSetConfig frontSetConfig)
        {
            _worldState = worldState;
            _worldState.DayChanged += OnDayChanged;
            _location = location;
            _frontConfigs = frontSetConfig.fronts.Where(front => front.location == _location).ToHashSet();
            //should be renamed: adds fronts to the _currentFronts
            AppendTodaysFronts();
           
            Initialize(root);
        }

        public void Update()
        {
            _frontList.Update(_worldState, _currentFronts);
            if (_currentFronts.Any())
                Show();
            else
                Hide();
        }

        private void OnDayChanged(WorldState worldState)
        {
            AppendTodaysFronts();
            var copy = new HashSet<FrontConfig>(_currentFronts);
            foreach (var front in copy)
            {
                front.OnDayChanged(worldState);
            }

            Update();
        }

        private void AppendTodaysFronts()
        {
            _currentFronts.UnionWith(_frontConfigs.Where(front =>
            {
                if (front.day == _worldState.Day &&
                    front.AreRequirementsMet(_worldState))
                {
                    front.Completed += OnFrontCompleted;
                    return true;
                }
                return false;
            }).ToHashSet());
        }

        private void OnFrontCompleted(FrontConfig frontConfig)
        {
            frontConfig.Completed -= OnFrontCompleted;
            _currentFronts.Remove(frontConfig);
            Update();
        }
        
        protected override void SetVisualElements()
        {
            HideOnAwake = !_currentFronts.Any();
            if (!_frontConfigs.Any())
                return;
            
            var frontListRoot = new VisualElement();
            Root.Add(frontListRoot);
            frontListRoot.AddToClassList("absolute");
            _frontList = new FrontListUI(frontListRoot, _worldState, _currentFronts);
            _button = Root.Q<Button>("location-button");
            _button.text = "Here some fronts!";
        }

        protected override void RegisterButtonCallbacks()
        {
            if (!_frontConfigs.Any())
                return;
            
            _button.RegisterCallback<MouseEnterEvent>(ShowLocationInfo);
        }

        private void ShowLocationInfo(MouseEnterEvent evt)
        {
            _frontList.Show();
            _frontList.Root.style.top = evt.localMousePosition.y;
            _frontList.Root.style.left = evt.localMousePosition.x;
            Debug.Log("SHOW");
        }
        
        public override void Dispose() {}
    }
}