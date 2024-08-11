using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoController : MonoBehaviour
{
    public  GameObject enemyNum;

    void Awake()
    {
        //敵数変化イベント
        GameDataManager.Instance.OnVarEnemyEvent
            .Subscribe(n =>
            {
                TextMeshProUGUI t = enemyNum.GetComponent<TextMeshProUGUI>();
                t.SetText(n.ToString());
            });
    }
}
