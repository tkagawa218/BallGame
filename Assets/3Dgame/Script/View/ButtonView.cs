using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{

    public class ButtonView : MonoBehaviour
    {
        [SerializeField]
        private Button _startButton;

        [SerializeField]
        private Button _returnButton;

        [SerializeField]
        private Button _mouseButton;

        public Button StartButton => _startButton;
        public Button ReturnButton => _returnButton;
        public Button MouseButton => _mouseButton;
    }
}
