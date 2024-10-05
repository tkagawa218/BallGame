using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Controller
{


    public class EnemyInfoController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _enemyNum;

        public void SetEnemyNum(int num)
        {
            _enemyNum.SetText(num.ToString());
        }
    }
}
