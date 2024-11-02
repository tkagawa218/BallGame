using UnityEngine;
using UnityEngine.UI;
using View;

public class ButtonController : MonoBehaviour
{
    [SerializeField]
    private ButtonView _buttonView;

    public Button StartButton => _buttonView.StartButton;
    public Button ReturnButton => _buttonView.ReturnButton;


    public void SetStartActive(bool b)
    {
        _buttonView.StartButton.gameObject.SetActive(b);
    }
    public void SetReturnActive(bool b)
    {
        _buttonView.ReturnButton.gameObject.SetActive(b);
    }
}
