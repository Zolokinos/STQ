using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    public class FrontUI : MonoBehaviour
    { 
        private UIDocument _uiDocument;

        [SerializeField] private WorldState _worldState;
        private FrontConfig _frontConfig;

        private List<FrontChoiceUI> _choices = new();
        private Button _skip;
        private Label _description;
        private StageConfig _currentStage;
        private VisualElement _root;
        
        void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            Refresh();
        }

        public void Refresh()
        {
            _frontConfig = _worldState.currentFront ?? throw new ArgumentNullException(nameof(_worldState));
            _currentStage = _frontConfig.GetCurrentStage(_worldState) ?? throw new ArgumentNullException(nameof(_frontConfig));
            _root = _uiDocument.rootVisualElement ?? throw new NullReferenceException(nameof(_uiDocument));

            SetVisualElements();
            RegisterButtonCallbacks();
        }

        protected void SetVisualElements()
        {
            _skip = _root.Q<Button>("skip");
            _description = _root.Q<Label>("description");
            _description.text = _currentStage.text;

            foreach (var choice in _currentStage.choices)
            {
                _choices.Add(new FrontChoiceUI(
                    _root,
                    choice,
                    _worldState
                ));
            }
        }

        protected void RegisterButtonCallbacks()
        {
            _skip.RegisterCallback<ClickEvent>(OnSkip);
        }

        private void OnSkip(ClickEvent evt)
        {
            _worldState.Day += 1;
            SceneManager.LoadScene("Map");
        }
    }
}