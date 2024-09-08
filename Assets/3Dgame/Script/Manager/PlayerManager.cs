using UnityEngine;
using UniRx.Async;
using UniRx.Async.Triggers;
using UniRx;
using System;
using Model;
using UnityEditor;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private float _speed = 1.0f; // 移動速度 m/s
                                 // Update is called once per frame
    [SerializeField]
    private GameObject _enemySP; //敵キャラオブジェクト格納親オブジェクト

    private AsyncCollisionTrigger asyncCollisionTrigger;

    private void Awake()
    {
        var gamedata = GameDataModel.GetGameData();

        UniRxManager.Instance.OnEndEvent
        .Subscribe(result =>
        {
            _playerController.InitPlayerPos();
        });
    }

    private void Start()
    {

        //AsyncTrigger（extends MonoBehaviour）を取得する
        asyncCollisionTrigger = this.GetAsyncCollisionTrigger();

        DoAsync().Forget();
    }

    private void Update()
    {
        gameObject.transform.eulerAngles = Vector3.zero;

        if(GameDataManager.Instance.getGameOn())
        {
            Vector3 pos = transform.position;
            float moveLength = _speed * Time.smoothDeltaTime;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if(pos.z + moveLength > GameDataManager.Instance.getMinZ() 
                && pos.z + moveLength < GameDataManager.Instance.getMaxZ())
                {
                    pos.z += moveLength;
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if(pos.z - moveLength > GameDataManager.Instance.getMinZ()
                && pos.z - moveLength < GameDataManager.Instance.getMaxZ())
                {
                    pos.z -= moveLength;
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                if(pos.x - moveLength > GameDataManager.Instance.getMinX()
                && pos.x - moveLength < GameDataManager.Instance.getMaxX())
                {
                    pos.x -= moveLength;
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (pos.x - moveLength > GameDataManager.Instance.getMinX()
                && pos.x - moveLength < GameDataManager.Instance.getMaxX())
                {
                    pos.x += moveLength;
                }
            }
            transform.position = pos;
        }
    }

    async UniTask DoAsync()
    {
         // OnCollisionEnterが発生するまで待機する
        var target = await asyncCollisionTrigger.OnCollisionEnterAsync();

        if(target.gameObject.name == "Terrain")
        {
            //味方キャラが地面に落ちたら、startボタンを表示
            if(!GameDataManager.Instance.getGameOn()) UniRxManager.Instance.SendStartButtonEvent(true);
        }
        else
        {
            if (GameDataManager.Instance.getGameOn())
            {
                int n = GameDataManager.Instance.getEnemyS().Count;
                bool flag = false;
                for (int i = 0; i < n; i++)
                {
                    if (target.gameObject.name == GameDataManager.Instance.getEnemyS()[i].name)
                    {
                        //味方キャラが敵キャラが接触
                        flag = true;
                        break;
                    }
                }

                if (flag)
                {
                    //味方キャラが敵キャラが接触
                    GameDataManager.Instance.setGameOff(false);
                }
                else
                {
                    n = GameDataManager.Instance.getPlayerParticleS().Count;
                    flag = false;
                    GameObject item = null;
                    for (int i = 0; i < n; i++)
                    {
                        if (target.gameObject.name == GameDataManager.Instance.getPlayerParticleS()[i].name)
                        {
                            //味方キャラが敵キャラ減少トリガー用パーティクルに接触
                            flag = true;
                            item = GameDataManager.Instance.getPlayerParticleS()[i];
                            break;
                        }
                    }

                    if (flag)
                    {
                        //味方キャラが敵キャラ減少トリガー用パーティクルに接触
                        GameDataManager.Instance.DellAnyPlayerParticleS(item);
                        GameDataManager.Instance.DelEnemyS();
                        GameSoundManager.Instance.sendPlayerparticleSeEvent();
                    }

                }

            }

        }
        DoAsync().Forget();


        // OnCollisionExitが発生するまで待つ
        //await asyncCollisionTrigger.OnCollisionExitAsync();

        //Debug.Log("Bye!");
    }
}