using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    public class MapUI : MonoBehaviour
    {
        private UIDocument _document;
        private VisualTreeAsset _endMenu;
        
        [SerializeField] private FrontSetConfig _frontsConfig;
        [SerializeField] private WorldState _worldState;

        private InfoUI _infoUI;
        private List<LocationOnMapUI> _locationsUI;
        
        void Awake()
        {
            _document = GetComponent<UIDocument>();
            _locationsUI = new List<LocationOnMapUI>(Enum.GetValues(typeof(Location)).Length);
            foreach (var location in Enum.GetValues(typeof(Location)))
            {
                var locationUI = _document.rootVisualElement.Q<VisualElement>("location" + (int)location);
                _locationsUI.Add(new LocationOnMapUI(locationUI, _worldState, (Location) location, _frontsConfig));
            }
            _endMenu = Resources.Load<VisualTreeAsset>("UI/end-menu");
            
            _infoUI = new InfoUI(_document.rootVisualElement.Q<VisualElement>("info"), _worldState);
            InitFrontsConfig();
        }

        //should be placed somewhere else...
        void InitFrontsConfig()
        {
            var endGameFront = _frontsConfig.fronts.Find(front => front.name == "Призыв");
            //dirty. It should be global event (Bus), but now KISS
            endGameFront.FiascoAffected += EndGame;
        }
        
        void Update()
        {
            return;
        }
        
        private void EndGame(WorldState worldState)
        {
            _document.rootVisualElement.Clear();
            var endMenu = _endMenu.Instantiate();
            var label = endMenu.Q<Label>("end-message");
            label.text = "ВАС ПРИЗВАЛИ НА ВОЙНУ";
            _document.rootVisualElement.Add(endMenu);
        }
    }
}
