using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    public class InfoUI : MonoBehaviour
    {
        [SerializeField] private WorldState _worldState;
        [SerializeField] private InputActionAsset _inputActions;
        private UIDocument _uiDocument;
        
        private VisualElement _root;
        
        void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;
            
            FillFrontTags();
        }

        private void FillFrontTags()
        {
            var tagsLabel = new Label();
            foreach (FrontTag tag in _worldState.tags.Keys)
            {
                tagsLabel.text += tag.ToString() + ": " + _worldState.tags[tag] + Environment.NewLine;
            }
            var tagsView = _root.Q<ScrollView>("tags-view");
            tagsView.Add(tagsLabel);
        }
    }
}