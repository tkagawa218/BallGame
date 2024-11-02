using UnityEngine;

namespace View
{

    public class PanelView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _help;

        public RectTransform Help => _help;
    }
}
