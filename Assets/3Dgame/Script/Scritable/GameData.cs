using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create GameData")]
public class GameData : ScriptableObject
{
    private static GameData s_instance = null;
    public static GameData Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = CreateInstance<GameData>();
            }

            return s_instance;
        }
    }

    [TooltipAttribute("敵初期数")] 
    public int enemyInitNum;

    [TooltipAttribute("敵初期ポジション")]
    public Vector3[] enemyPos;

    [TooltipAttribute("敵増減パーティクル用ポジション")]
    public Vector3[] areaPos;

    [TooltipAttribute("敵最大数")]
    public int enemyNumMax;

	public int enemyNumIncreaseRate;

    [TooltipAttribute("ゲーム時間")]
    public int gameTime = 180;

    [TooltipAttribute("敵減パーティクル用ポジション")]
    public int playerAreaNum = 5;

    [TooltipAttribute("敵増パーティクル用ポジション")]
    public int enemyAreaNum = 5;

    [TooltipAttribute("パーティクル配置換えまでの時間間隔")]
    public int particleInterval = 30;

    [TooltipAttribute("敵配置換えまでの時間間隔")]
    public int enemyExplaceInterval = 60;

    [TooltipAttribute("プレイヤー移動範囲")]
    public int minX = 0;
    public int maxX = 390;
    public int minZ = 0;
    public int maxZ = 390;
}
