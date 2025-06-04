using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    public class FrontUI : MonoBehaviour
    { 
        [SerializeField] private VisualTreeAsset _dialogeChoice;
        [SerializeField] private StyleSheet _styleSheet;
        private UIDocument _uiDocument;

        [SerializeField] private WorldState _worldState;
        private FrontConfig _frontConfig;

        private List<FrontChoiceUI> _choices = new();
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
        }

        protected void SetVisualElements()
        {
            var label = _root.Q<Label>("description");
            label.text = _currentStage.text;

            foreach (var choice in _currentStage.choices)
            {
                var frontChoiceUI = new FrontChoiceUI(
                    _root,
                    choice,
                    _worldState,
                    _dialogeChoice
                );
                frontChoiceUI.MadenChoice += (choice) => _frontConfig.IsCompleted = true;
                _choices.Add(frontChoiceUI);
            }
        }

        /*
        private void AddChoiceButton(int choiceNumber)
        {
            var choice = _dialogeChoice.Instantiate();
            var choiceButton = choice.Q<Button>("choice");
            /*if (!_currentStage.choices[choiceNumber].IsAvailable(_worldState))
            {
                choiceButton.styleSheets.Add(_styleSheet);
            }#1#
            choiceButton.text = _currentStage.choices[choiceNumber].text;
            _choices.Add(choiceButton);
            _root.Add(choice); 
        }*/

    }
}