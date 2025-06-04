using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class InfoUI : UIView
    {
        private WorldState _worldState;
        private VisualTreeAsset _dialogUI;
        private Button _skipButton;
        private ScrollView _tagsView;
        
        public InfoUI(VisualElement root, WorldState worldState)
        {
            _worldState = worldState;
            Initialize(root);
        }

        protected override void SetVisualElements()
        {
            _skipButton = Root.Q<Button>("skip");
            _tagsView = Root.Q<ScrollView>("tags-view");
            FillFrontTags();
        }

        private void FillFrontTags()
        {
            var tagsLabel = new Label();
            tagsLabel.style.backgroundColor = Color.white;
            foreach (FrontTag tag in _worldState.tags.Keys)
            {
                tagsLabel.text += tag.ToString() + ": " + _worldState.tags[tag] + Environment.NewLine;
            }
            _tagsView.visible = !string.IsNullOrEmpty(tagsLabel.text);
            _tagsView.Add(tagsLabel);
        }
        
        protected override void RegisterButtonCallbacks()
        {
            _skipButton.RegisterCallback<ClickEvent>(OnSkip);
        }

        private void OnSkip(ClickEvent evt)
        {
            _worldState.day += 1;
            SceneManager.LoadScene("Map");
        }

        public override void Dispose() {}
    }
}