using UnityEngine;
using BattleTank.Events;

public class ShootButtonScript : MonoBehaviour
{
    public void ShootFunc()
    {
        EventService.Instance.InvokePlayerShoot();
    }
}
