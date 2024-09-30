using Common;
using Model;
using System.Linq;
using UniRx.Async;
using UniRx.Async.Triggers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;

    private AsyncCollisionTrigger _asyncCollisionTrigger;

    private PlayerDirection _currentPlayerDirection;

    private void Awake()
    {
        //AsyncTrigger（extends MonoBehaviour）を取得する
        _asyncCollisionTrigger = this.GetAsyncCollisionTrigger();
    }

    private bool GetGameOn()
    {
        return GameDataModel.GetGameOn();
    }

    public async UniTask DoAsync()
    {
        // OnCollisionEnterが発生するまで待機する
        // 味方球が、クリーム色のパーティクルに触れると、敵がへります。
        var target = await _asyncCollisionTrigger.OnCollisionEnterAsync();

        if (target.gameObject.name == "Terrain")
        {
            //味方キャラが地面に落ちたら、startボタンを表示
            //if (GetGameOn()) UniRxManager.Instance.SendStartButtonEvent(true);
        }
        else
        {
            if (GetGameOn())
            {
                var enemyParticleS = GameDataModel.GetEnemyParticleS();
                var flag = false;
                int n = GameDataModel.GetEnemyS().Count;
                for (int i = 0; i < n; i++)
                {
                    if (target.gameObject == GameDataModel.GetEnemyS()[i])
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
                }
                else
                {
                    var particleS = GameDataModel.GetPlayerParticleS();

                    var item = particleS
                               .FirstOrDefault(p => p == target.gameObject);

                    if (item != null)
                    {
                        UniRxManager.Instance.SendDelEnemyParticleEvent(item);
                        UniRxManager.Instance.SendVarEnemyEvent(GameDataModel.GetEnemyS().Count - 1);

                        GameSoundManager.Instance.sendPlayerparticleSeEvent();
                    }
                }

            }

        }

        DoAsync().Forget();
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

            if(_currentPlayerDirection != playerDirection)
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
        transform.position = GameDataModel.GetInitPlayerPos();
    }
}
