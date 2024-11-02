using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Controller
{

    public class PanelController : MonoBehaviour
    {
        [SerializeField]
        private PanelView _panelView;

        public RectTransform Help => _panelView.Help;


        public void SetHelpActive(bool b)
        {
            _panelView.Help.gameObject.SetActive(b);
        }
    }
}
