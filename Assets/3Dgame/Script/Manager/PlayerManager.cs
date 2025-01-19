using UnityEngine;
using UniRx.Async;
using UniRx;
using Controller;
using System.Threading;

namespace Manager
{

    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerController _playerController;

        [SerializeField]
        private float _force = 1.0f; // 移動速度 m/s
                                     // Update is called once per frame

        private CancellationTokenSource _cancellationTokenSource = null;
        private CancellationTokenSource _cancellationTokenSourceParticle = null;

        private void Awake()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationTokenSourceParticle = new CancellationTokenSource();

            UniRxManager.Instance.OnPlayerDirectionEvent
            .Subscribe(playerDirection =>
            {
                _playerController.Move(_force, playerDirection);
            });

            UniRxManager.Instance.OnInitEvent
            .Subscribe(_ =>
            {
                _playerController.InitPlayerPos();
            })
            .AddTo(this);
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSourceParticle.Cancel();
        }

        private void Start()
        {
            _playerController.DoAsync(_cancellationTokenSource).Forget();
            _playerController.DoParticleAsync(_cancellationTokenSourceParticle).Forget();
        }
    }
}