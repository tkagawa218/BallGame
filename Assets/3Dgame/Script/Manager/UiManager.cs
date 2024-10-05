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
        private UITimerController _uiTimerController;

        [SerializeField]
        private EnemyInfoController _enemyInfoController;
        

        private void Awake()
        {
            UniRxManager.Instance.OnSetTimeEvent
            .Subscribe(t =>
            {
                _uiTimerController.SetTime(t);
            })
            .AddTo(this);

            UniRxManager.Instance.OnStartButtonEvent
            .Subscribe(b =>
            {
                _buttonController.SetStartActive(b);
            })
            .AddTo(this);

            _buttonController.StartButton.OnClickAsObservable()
            .Subscribe(c =>
            {
                UniRxManager.Instance.SendStartEvent();
            })
            .AddTo(this);


            UniRxManager.Instance.OnVarEnemyEvent
            .Subscribe(num =>
            {
                _enemyInfoController.SetEnemyNum(num);
            })
            .AddTo(this);
        }
    }
}
