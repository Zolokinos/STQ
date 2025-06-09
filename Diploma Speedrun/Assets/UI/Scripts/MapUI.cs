using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
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

        private FrontUI _frontUI;
        private InfoUI _infoUI;
        private List<LocationOnMapUI> _locationsUI;
        
        void Awake()
        {
            DontDestroyOnLoad(this);
            foreach (var front in _frontsConfig.fronts)
            {
                front.INIT();
            }
            _document = GetComponent<UIDocument>();
            _endMenu = Resources.Load<VisualTreeAsset>("end-menu");
            
            _locationsUI = new List<LocationOnMapUI>(Enum.GetValues(typeof(Location)).Length);
            foreach (var location in Enum.GetValues(typeof(Location)))
            {
                var locationUI = _document.rootVisualElement.Q<VisualElement>("location" + (int)location);
                _locationsUI.Add(new LocationOnMapUI(locationUI, _worldState, (Location) location, _frontsConfig));
            }
            
            _infoUI = new InfoUI(_document.rootVisualElement.Q<VisualElement>("info"), _worldState);
            InitFrontsConfig();
        }

        //should be placed somewhere else...
        void InitFrontsConfig()
        {
            var endGameFront = _frontsConfig.fronts.Find(front => front.name == "Призыв");
            //dirty. It should be global event (Bus), but now KISS
            endGameFront.FiascoAffected += EndGame;
            //It is much worse
            Bus<FrontLoad>.Event += LoadFront;
        }

        //PIECE OF CODE
        private void LoadFront(bool loadFront)
        {
            var root = _document.rootVisualElement.Q<VisualElement>("root");
            var front = _document.rootVisualElement.Q<VisualElement>("front");
            if (loadFront)
            {
                _frontUI = new FrontUI(front, _worldState, _worldState.currentFront);
                front?.RemoveFromClassList("hidden");
                root?.AddToClassList("hidden");
            }
            else
            {
                front?.Clear();
                _frontUI = null;
                front?.AddToClassList("hidden");
                root?.RemoveFromClassList("hidden");
            }
        }
        
        private void EndGame(WorldState worldState)
        {
            _document.rootVisualElement.Clear();
            var endMenu = _endMenu.Instantiate();
            endMenu.contentContainer.style.flexGrow = 1f;
            var label = endMenu.Q<Label>("end-message");
            label.text = "ВАС ПРИЗВАЛИ НА ВОЙНУ";
            _document.rootVisualElement.Add(endMenu);
        }
    }
}
