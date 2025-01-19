using UnityEngine;
using UniRx;
using Controller;
using UniRx.Triggers;

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

        [SerializeField]
        private PanelController _panelController;

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

            _buttonController.ReturnButton.OnClickAsObservable()
            .Subscribe(c =>
            {
                _panelController.SetHelpActive(true);
                _buttonController.SetStartActive(true);
                _buttonController.SetReturnActive(false);
                _buttonController.SetMouseActive(false);
                UniRxManager.Instance.SendInitEvent();
            })
            .AddTo(this);

            _buttonController.MouseButton.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ =>
            {
                UniRxManager.Instance.SendChangeCameraDirMouseEvent(true);
            })
            .AddTo(this);

            _buttonController.MouseButton.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonUp(0))
            .Subscribe(c =>
            {
                UniRxManager.Instance.SendChangeCameraDirMouseEvent(false);
            })
            .AddTo(this);

            UniRxManager.Instance.OnChangeEnemyNumEvent
            .Subscribe(num =>
            {
                _enemyInfoController.SetEnemyNum(num);
            })
            .AddTo(this);

            UniRxManager.Instance.OnStartEvent
            .Subscribe(_ =>
            {
                _panelController.SetHelpActive(false);
                _buttonController.SetStartActive(false);
                _buttonController.SetReturnActive(false);
                _buttonController.SetMouseActive(true);
            })
            .AddTo(this);

            UniRxManager.Instance.OnEndEvent
            .Subscribe(result =>
            {
                _buttonController.SetReturnActive(true);
                _buttonController.SetMouseActive(false);
            })
            .AddTo(this);
        }
    }
}