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
        //AsyncTrigger�iextends MonoBehaviour�j���擾����
        _asyncCollisionTrigger = this.GetAsyncCollisionTrigger();
    }

    private bool GetGameOn()
    {
        return GameDataModel.GetGameOn();
    }

    public async UniTask DoAsync()
    {
        // OnCollisionEnter����������܂őҋ@����
        // ���������A�N���[���F�̃p�[�e�B�N���ɐG���ƁA�G���ւ�܂��B
        var target = await _asyncCollisionTrigger.OnCollisionEnterAsync();

        if (target.gameObject.name == "Terrain")
        {
            //�����L�������n�ʂɗ�������Astart�{�^����\��
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
                        //�����L�������G�L�����ɐڐG
                        flag = true;
                        break;
                    }
                }

                if (flag)
                {
                    //�����L�������G�L���ɐڐG
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
