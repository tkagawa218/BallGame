using Model;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public void InitPlayerPos()
    {
        transform.position = GameDataModel.GetInitPlayerPos();
    }
}
