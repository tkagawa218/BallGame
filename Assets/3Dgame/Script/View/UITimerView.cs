using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{

    public class UITimerView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _timeText;

        public TextMeshProUGUI TimeText => _timeText;

        public void SetText(string text)
        {
            _timeText.text = text;
        }
    }
}
