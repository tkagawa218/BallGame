using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField]
    private ButtonView _buttonView;

    public Button StartButton => _buttonView.StartButton;

    private void Awake()
    {
        _buttonView.StartButton.onClick.AsObservable()
        .Subscribe(_ =>
        {
            UniRxManager.Instance.SendStartEvent();
        });
    }

    public void SetStartActive(bool b)
    {
        _buttonView.StartButton.gameObject.SetActive(b);
    }
}
