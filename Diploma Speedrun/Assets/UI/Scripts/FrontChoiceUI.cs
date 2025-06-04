using System.Drawing;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Color = System.Drawing.Color;

namespace UI
{
    public class FrontChoiceUI : UIView
    {
        private Choice _choice;
        private WorldState _worldState;
        private VisualTreeAsset _dialogeChoice;

        private Button _choiceButton;
        private bool _isAvailable;

        public delegate void ChoiceHandler(Choice choice);
        public event ChoiceHandler MadenChoice;
        
        public FrontChoiceUI(VisualElement root, Choice choice, WorldState worldState, VisualTreeAsset dialogeChoice)
        {
            _choice = choice ??  throw new System.ArgumentNullException(nameof(choice));
            _worldState = worldState ??  throw new System.ArgumentNullException(nameof(worldState));
            _dialogeChoice =  dialogeChoice ??  throw new System.ArgumentNullException(nameof(dialogeChoice));
            _isAvailable = _choice.IsAvailable(_worldState);
            
            Initialize(root);
        }

        protected override void SetVisualElements()
        {
            var choice = _dialogeChoice.Instantiate();
            var choiceButton = choice.Q<Button>("choice");
            choice.SetEnabled(_isAvailable);
            choiceButton.text = _choice.text;
            _choiceButton = choiceButton;
            Root.Add(choice);
        }

        protected override void RegisterButtonCallbacks()
        {
            _choiceButton.RegisterCallback<ClickEvent>(TryMakeChoice);
        }

        private void TryMakeChoice(ClickEvent evt)
        {
            if (_isAvailable)
            {
                _choice.Affect(_worldState);
                _worldState.day += 1;
                MadenChoice?.Invoke(_choice);
                SceneManager.LoadScene("Map");
            }
        }

        public override void Dispose() {}
    }
}