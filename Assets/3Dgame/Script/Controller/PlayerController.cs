using UnityEngine;
using System.Collections;
using UniRx.Async;
using UniRx.Async.Triggers;
using UniRx;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1.0f; // 移動速度 m/s
                                 // Update is called once per frame
    [SerializeField]
    private GameObject _gameOver; //GameOver時表示オブジェクト

    [SerializeField]
    private GameObject _gameWin; //Game勝利時表示オブジェクト

    [SerializeField]
    private GameObject _start; //スタートボタン用オブジェクト

    [SerializeField]
    private GameObject _help; //ヘルプ用オブジェクト

    [SerializeField]
    private GameObject _enemySP; //敵キャラオブジェクト格納親オブジェクト

    private AsyncCollisionTrigger asyncCollisionTrigger;

    private void Awake()
    {
        GameData gamedata = Resources.Load<GameData>(GameDataManager.Instance.GetScritablePath());

        //ゲームスタートイベントを購読
        GameDataManager.Instance.OnStartEvent
        .Subscribe(_ =>
        {
            GameDataManager.Instance.allDellEnemyS();
            GameDataManager.Instance.setRestTime(GameData.Instance.gameTime);
            GameDataManager.Instance.sendSetParticleEvent();
            _start.SetActive(false);
            _help.SetActive(false);
            _gameOver.SetActive(false);
            _gameWin.SetActive(false);
            GameDataManager.Instance.addEnemyS(_enemySP, gameObject);
            GameSoundManager.Instance.sendSoundPlayBgmEvent();

            Observable.Interval(TimeSpan.FromSeconds(gamedata.enemyExplaceInterval))
                           .Do(x => GameDataManager.Instance.allExplaceEnemyS())
                           .Subscribe();
        });

        //ゲーム終了イベントを購読
        //result
        //true:勝利
        //false:敗北
        GameDataManager.Instance.OnEndEvent
        .Subscribe(result =>
        {
            transform.position = GameDataManager.Instance.getInitPlayerPos();
            if(result)
            {
                _gameWin.SetActive(true);
                GameSoundManager.Instance.sendSoundWinBgmEvent();
                _help.SetActive(true);
            }
            else
            {
                _gameOver.SetActive(true);
                GameSoundManager.Instance.sendSoundLoseBgmEvent();
                _help.SetActive(true);
            }
        });
    }

    private void Start()
    {
        GameDataManager.Instance.setInitPlayerPos(transform.position);

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
            if(!GameDataManager.Instance.getGameOn()) _start.SetActive(true);
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

    /// <summary>
    /// startボタン押下時呼ばれるメソッド
    /// </summary>
    public void startBtn_Click()
    {
        GameDataManager.Instance.setGameOn();
        GameSoundManager.Instance.sendStartBtnSeEvent();
    }
}