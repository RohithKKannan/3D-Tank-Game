using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BattleTank.Events;

[RequireComponent(typeof(Animator))]
public class GameUIManager : MonoBehaviour
{
    private Animator canvasAnimator;

    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private GameObject pausePanel;

    private void Awake()
    {
        canvasAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        EventService.Instance.OnSetMaxHealthBar += SetMaxHealth;
        EventService.Instance.OnSetPlayerHealthBar += SetPlayerHealth;
        EventService.Instance.OnGameOver += StartGameOver;
    }

    public void ShootFunc()
    {
        EventService.Instance.InvokePlayerShoot();
    }

    public void SetMaxHealth(int _maxHealth)
    {
        playerHealthBar.maxValue = _maxHealth;
    }

    public void SetPlayerHealth(float health)
    {
        Debug.Log("Set player health called for : " + health);
        playerHealthBar.value = health;
    }

    public void StartGameOver()
    {
        canvasAnimator.SetTrigger("GameOver");
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDisable()
    {
        EventService.Instance.OnSetMaxHealthBar -= SetMaxHealth;
        EventService.Instance.OnSetPlayerHealthBar -= SetPlayerHealth;
        EventService.Instance.OnGameOver -= StartGameOver;
    }
}
