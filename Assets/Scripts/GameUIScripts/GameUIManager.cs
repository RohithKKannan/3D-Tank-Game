using UnityEngine;
using UnityEngine.UI;
using BattleTank.Events;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private Slider playerHealthBar;

    private void Start()
    {
        EventService.Instance.OnSetMaxHealthBar += SetMaxHealth;
        EventService.Instance.OnSetPlayerHealthBar += SetPlayerHealth;
    }

    public void ShootFunc()
    {
        EventService.Instance.InvokePlayerShoot();
    }

    public void SetMaxHealth(int _maxHealth)
    {
        playerHealthBar.maxValue = _maxHealth;
    }

    public void SetPlayerHealth(int health)
    {
        playerHealthBar.value = health;
    }

    private void OnDisable()
    {
        EventService.Instance.OnSetMaxHealthBar -= SetMaxHealth;
        EventService.Instance.OnSetPlayerHealthBar -= SetPlayerHealth;
    }
}
