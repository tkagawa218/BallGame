using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace View
{

    public class GameView : MonoBehaviour
    {
        private const float BgImageAlphaOn = 0.4f;

        private const float BgImageAlphaOff = 0.0f;

        [SerializeField]
        private EnemyManager _enemyPrefab;

        [SerializeField]
        private Image _bgImage; //背景用オブジェクト

        public EnemyManager EnemyPrefab => _enemyPrefab;
        public Image BgImage => _bgImage;

        public void SetBgImageAlpha(bool on)
        {
            Color color = _bgImage.color;
            color.r = 0.8f;
            color.g = 0.3f;
            color.b = 0.1f;
            color.a = on ? BgImageAlphaOn : BgImageAlphaOff;
            _bgImage.color = color;
        }
    }
}