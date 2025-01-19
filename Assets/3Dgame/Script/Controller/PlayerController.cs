using Common;
using Manager;
using Model;
using System.Linq;
using System.Threading;
using UniRx.Async;
using UniRx.Async.Triggers;
using UnityEngine;

namespace Controller
{

    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rigidbody;

        private AsyncCollisionTrigger _asyncCollisionTrigger;
        private AsyncTriggerTrigger _asyncTriggerTrigger;

        private PlayerDirection _currentPlayerDirection;

        private void Awake()
        {
            _asyncTriggerTrigger = this.GetAsyncTriggerTrigger();
        }

        private bool GetGameOn()
        {
            return GameDataModel.GetGameOn();
        }

        public async UniTask DoAsync(CancellationTokenSource c)
        {
            var token = c.Token;

            _asyncCollisionTrigger = this.GetAsyncCollisionTrigger();

            var target = await _asyncCollisionTrigger.OnCollisionEnterAsync(token);

            if (GetGameOn())
            {
                var enemyParticleS = GameDataModel.GetEnemyParticleS();
                var flag = false;
                var n = GameDataModel.GetEnemyS().Count;
                for (int i = 0; i < n; i++)
                {
                    if (target.gameObject == GameDataModel.GetEnemyS()[i].gameObject)
                    {
                        //味方キャラが敵キャラに接触
                        flag = true;
                        break;
                    }
                }

                if (flag)
                {
                    //味方キャラが敵キャに接触
                    GameDataModel.SetGameOff();
                    UniRxManager.Instance.SendEndEvent(false);
                }
            }

            c.Cancel();

            c = new CancellationTokenSource();

            await DoAsync(c);
        }

        public async UniTask DoParticleAsync(CancellationTokenSource c)
        {
            var token = c.Token;

            // 味方球が、クリーム色のパーティクルに触れると、敵がへります。
            var target = await _asyncTriggerTrigger.OnTriggerEnterAsync(token);

            var particleS = GameDataModel.GetPlayerParticleS();

            var item = particleS
                       .FirstOrDefault(p => p == target.gameObject);

            var gamedata = GameDataModel.GetGameData();

            if (item != null)
            {
                UniRxManager.Instance.SendDelEnemyParticleEvent(item);
                UniRxManager.Instance.SendVarEnemyEvent(GameDataModel.GetEnemyS().Count - gamedata.enemyNumIncreaseRate);
                GameSoundManager.Instance.sendPlayerparticleSeEvent();
            }

            c.Cancel();

            c = new CancellationTokenSource();

            await DoParticleAsync(c);
        }

        public void Move(float force, PlayerDirection playerDirection)
        {
            if (GetGameOn())
            {
                if (playerDirection == PlayerDirection.NONE)
                {
                    _rigidbody.velocity = Vector3.zero;
                    return;
                }

                Vector3 pos = Vector3.zero;

                if (_currentPlayerDirection != playerDirection)
                {
                    _rigidbody.velocity = Vector3.zero;
                }

                switch (playerDirection)
                {
                    case PlayerDirection.UPARROW:
                        pos.z = force;
                        break;

                    case PlayerDirection.DOWNARROW:
                        pos.z = -force;
                        break;

                    case PlayerDirection.LEFTARROW:
                        pos.x = -force;
                        break;

                    case PlayerDirection.RIGHTARROW:
                        pos.x = force;
                        break;
                }


                _rigidbody.AddForce(pos, ForceMode.Impulse);
                _currentPlayerDirection = playerDirection;
            }
        }

        public void InitPlayerPos()
        {
            _rigidbody.isKinematic = true;
            transform.position = GameDataModel.GetInitPlayerPos();
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.isKinematic = false;
        }
    }
}