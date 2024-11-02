using UnityEngine;
using UniRx.Async;
using UniRx;
using Controller;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Manager
{

    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerController _playerController;

        [SerializeField]
        private float _force = 1.0f; // 移動速度 m/s
                                     // Update is called once per frame


        private void Awake()
        {
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

        private void Start()
        {
            _playerController.DoAsync().Forget();
            _playerController.DoParticleAsync().Forget();
        }
    }
}