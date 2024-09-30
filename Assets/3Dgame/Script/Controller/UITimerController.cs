using Controller;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using View;

namespace Controller
{

    public class UITimerController : MonoBehaviour
    {
        [SerializeField]
        private UITimerView _uiTimerView;

        private string timeFormat(int time)
        {
            int minutes = 0;
            int seconds = time;
            while (seconds >= 60)
            {
                minutes++;
                seconds = seconds - 60;
            }

            return minutes.ToString("00") + ":" + seconds.ToString("00");
        }

        public void SetTime(int time)
        {
            _uiTimerView.SetText(timeFormat(time));
        }
    }
}
