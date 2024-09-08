using UnityEngine;
using UniRx;
using Controller;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Manager
{


    public class UiManager : MonoBehaviour
    {
        [SerializeField]
        private ButtonController _buttonController;

        [SerializeField]
        private TimerController _timerController;

        private void Awake()
        {
            UniRxManager.Instance.OnSetTimeEvent
            .Subscribe(t =>
            {
                _timerController.SetTime(t);
            })
            .AddTo(this);

            UniRxManager.Instance.OnStartButtonEvent
            .Subscribe(b =>
            {
                _buttonController.SetStartActive(b);
            })
            .AddTo(this);
        }
    }
}
