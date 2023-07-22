using UnityEngine;
using BattleTank.Events;

public class GameUIManager : MonoBehaviour
{
    public void ShootFunc()
    {
        EventService.Instance.InvokePlayerShoot();
    }
}
