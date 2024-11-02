using Controller;
using UniRx;
using UnityEngine;
using View;
using static UnityEngine.Networking.UnityWebRequest;

namespace Presenter
{

    public class DirectionPresenter : MonoBehaviour
    {
        [SerializeField]
        private Transform _targetPosition;

        [SerializeField]
        private EffectView _winEffectView;

        [SerializeField]
        private EffectView _loseEffectView;

        private bool _end;

        private bool _result;

        private void Awake()
        {
            _end = false;

            UniRxManager.Instance.OnEndEvent
              .Subscribe(result =>
              {
                  _end = true;
                  _result = result;

                  if (result)
                  {
                      _winEffectView.gameObject.SetActive(true);
                      return;
                  }
                  _loseEffectView.gameObject.SetActive(true);
              })
              .AddTo(this);

            UniRxManager.Instance.OnInitEvent
            .Subscribe(_ =>
            {
                if (_result)
                {
                    _winEffectView.gameObject.SetActive(false);
                    return;
                }
                _loseEffectView.gameObject.SetActive(false);
            })
            .AddTo(this);

            Observable.EveryUpdate()
              .Subscribe(_ => {
                  if (_end)
                  {
                      transform.position = _targetPosition.position;
                  }
              })
              .AddTo(this);

        }
    }
}