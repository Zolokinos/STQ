using DefaultNamespace;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class FrontChoiceUI : UIView
    {
        private Choice _choice;
        private WorldState _worldState;
        private VisualTreeAsset _dialogeChoice;

        private Button _choiceButton;

        public FrontChoiceUI(VisualElement root, Choice choice, WorldState worldState, VisualTreeAsset dialogeChoice)
        {
            _choice = choice ??  throw new System.ArgumentNullException(nameof(choice));
            _worldState = worldState ??  throw new System.ArgumentNullException(nameof(worldState));
            _dialogeChoice =  dialogeChoice ??  throw new System.ArgumentNullException(nameof(dialogeChoice));
            
            Initialize(root);
        }

        protected override void SetVisualElements()
        {
            var choice = _dialogeChoice.Instantiate();
            var choiceButton = choice.Q<Button>("choice");
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
            if (_choice.IsAvailable(_worldState))
            {
                _choice.AffectWorldState(_worldState);
                SceneManager.LoadScene("Map");
            }
        }

        public override void Dispose() {}
    }
}