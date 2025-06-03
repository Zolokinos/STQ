using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    public class Map : MonoBehaviour
    {
        private UIDocument _document;

        void Awake()
        {
            _document = GetComponent<UIDocument>();
            
        }
    }
}
