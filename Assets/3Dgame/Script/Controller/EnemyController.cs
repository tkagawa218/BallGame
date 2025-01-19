using Manager;
using Model;
using System.Threading;
using UniRx.Async;
using UniRx.Async.Triggers;
using UnityEngine;


namespace Controller
{

    public class EnemyController : MonoBehaviour
    {
        private AsyncTriggerTrigger _asyncTriggerTrigger;

        private GameData _gamedata;

        private void Awake()
        {
            _asyncTriggerTrigger = this.GetAsyncTriggerTrigger();

            _gamedata = GameDataModel.GetGameData();
        }

        public async UniTask DoAsync(CancellationTokenSource c)
        {
            var token = c.Token;

            // OnCollisionEnterが発生するまで待機する
            // 敵球が、茶色のパーティクルに触れると、敵が増えます。
            var target = await _asyncTriggerTrigger.OnTriggerEnterAsync(token);

            var enemyParticleS = GameDataModel.GetEnemyParticleS();
            bool flag = false;
            GameObject item = null;
            foreach (var enemyParticle in enemyParticleS)
            {
                if (target.gameObject == enemyParticle)
                {
                    flag = true;
                    item = enemyParticle;
                    break;
                }
            }

            if (flag)
            {
                UniRxManager.Instance.SendDelEnemyParticleEvent(item);
                UniRxManager.Instance.SendVarEnemyEvent(GameDataModel.GetEnemyS().Count + _gamedata.enemyNumIncreaseRate);
            }

            c.Cancel();

            c = new CancellationTokenSource();
            DoAsync(c).Forget();
        }
    }
}
