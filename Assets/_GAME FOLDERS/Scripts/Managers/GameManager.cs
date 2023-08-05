using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.AI;

public class GameManager : Singleton<GameManager>
{
    public bool IsGameActive;
    public int CurrentEnemyCount;

    [Header("Private Controls")]
    [HideInInspector] public int _currentWeaponIndex;
    private float _health;

    [Header("General Controls")]
    [SerializeField] private CameraTransition _cameraTransitionManager;
    [SerializeField] private GameObject _gameOverPanelObject;
    [SerializeField] private GameObject _winPanelObject;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _gameWindowPanel;
    float _spawnDuration = 3;
    [Header("UI Controls")]
    [SerializeField] private Image _damageEffectImage;
    [SerializeField] private Image _healthBar;
    [SerializeField] private TextMeshProUGUI _currentEnemyText;
    [SerializeField] private TextMeshProUGUI _totalEnemyText;

    [Header("Weapon Controls")]
    [SerializeField] private GameObject[] _weaponsObject;
    [SerializeField] private AudioSource _weaponChanger;

    [Header("Enemy Controls")]
    [SerializeField] private GameObject _basicZombie, _normalZombie, _hardZombie;
    [SerializeField] private Transform[] _enemySpawnPoints;
    [SerializeField] private Transform _targetPoint;
    private int _totalEnemyCount;
    private int _targetEnemyCount;
    private int _currentWave;

    [Header("GameMode Controls")]
    [SerializeField] private TextMeshProUGUI _timerText;
    private float _timer;
    private bool _isTimeMode;
    void Start()
    {
        InitializeGame();
    }
    void Update()
    {
        if (!IsGameActive)
            return;

        HandleInput();

        if (_isTimeMode)
            Timer();
    }
    private void HandleInput()
    {
        if (Input.anyKey && !Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < _weaponsObject.Length; i++)
            {
                if (Input.GetKeyDown(_weaponsObject[i].GetComponent<WeaponController>().activationKey))
                {
                    ChangeWeapon(i);
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale != 0)
                PauseGame();
            else
                ResumeGame();
        }
    }
    private void Timer()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            GameOver();
        }
        else
        {
            _timerText.text = Mathf.Round(_timer).ToString();
        }
    }
    private void ModeController()
    {
        _isTimeMode = PlayerPrefs.GetInt("GameMode") == 1;

        if (_isTimeMode)
        {
            _timerText.gameObject.SetActive(true);
            _spawnDuration = 0.5f;
            _timer = 60f;
        }
        else
        {
            _timerText.gameObject.SetActive(false);
            _spawnDuration = 2.2f;
        }
    }
    private void InitializeGame()
    {
        if (_isTimeMode)
        {
            _totalEnemyCount = HelperMethods.GetInt(30, 50);
            float number = Mathf.Round(_totalEnemyCount * 0.75f);
            _targetEnemyCount = (int)HelperMethods.GetFloat(15, number);
        }
        else
        {
            _totalEnemyCount = HelperMethods.GetInt(15, 30);
            float number = Mathf.Round(_totalEnemyCount * 0.75f);
            _targetEnemyCount = (int)HelperMethods.GetFloat(10, number);
        }

        SetCursorState(CursorLockMode.Locked);

        //Enemy Count
        CurrentEnemyCount = 0;

        _totalEnemyText.text = _targetEnemyCount.ToString();
        _currentEnemyText.text = CurrentEnemyCount.ToString();

        _health = 100;
        _healthBar.fillAmount = 1f;
        _currentWeaponIndex = 0;
        //TODO: Oyun sesi aktif edilebilir.

    }
    public void HandleStartGame()
    {
        ModeController();
        StartCoroutine(SpawnEnemyCoroutine());
    }
    IEnumerator SpawnEnemyCoroutine()
    {
        float difficultyIncreaseTimer = 0f;
        float difficultyIncreaseThreshold = 20f;

        while (IsGameActive && _totalEnemyCount > 0)
        {

            difficultyIncreaseTimer += _spawnDuration;

            if (difficultyIncreaseTimer >= difficultyIncreaseThreshold)
            {
                _currentWave += 5;
                difficultyIncreaseTimer = 0f;
            }

            SpawnEnemy();
            yield return new WaitForSeconds(_spawnDuration);
        }
    }
    private void SpawnEnemy()
    {
        int spawnPointIndex = HelperMethods.GetInt(0, _enemySpawnPoints.Length);
        GameObject chosenZombiePoolObject;
        float randomWeight = HelperMethods.GetFloat(0f, 1f);

        /* if (randomWeight < 0.5f) // %50 olasýlýk
             chosenZombiePoolName = "Basic_Zombie";
         else if (randomWeight < 0.8f) // %30 olasýlýk
             chosenZombiePoolName = "Normal_Zombie";
         else // %20 olasýlýk
             chosenZombiePoolName = "Hard_Zombie";

         GameObject chosenZombie = ObjectPoolManager.Instance.SpawnFromPool(chosenZombiePoolName, _enemySpawnPoints[spawnPointIndex].transform.position, Quaternion.identity, _enemySpawnPoints[spawnPointIndex]); */

        if (randomWeight < 0.5f) // %50 olasýlýk
            chosenZombiePoolObject = _basicZombie;
        else if (randomWeight < 0.8f) // %30 olasýlýk
            chosenZombiePoolObject = _normalZombie;
        else // %20 olasýlýk
            chosenZombiePoolObject = _hardZombie;

        // Duruma gore warp yapýlabilir. 
        GameObject chosenZombie = Instantiate(chosenZombiePoolObject, _enemySpawnPoints[spawnPointIndex].transform.position, Quaternion.identity, _enemySpawnPoints[spawnPointIndex]);
        IncreaseEnemyStats(chosenZombie);
        chosenZombie.GetComponent<EnemyMovement>().SetTarget(_targetPoint);
        _totalEnemyCount--;
    }
    private void IncreaseEnemyStats(GameObject enemy)
    {
        // Mevcut dalgaya göre caný ve hýzýnýn artýrýlmasý
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        enemyHealth.Health += _currentWave * 5;
        NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        navMeshAgent.speed += _currentWave * 0.075f;

        Animator animator = enemy.GetComponent<Animator>();
        animator.speed = navMeshAgent.speed;
    }
    public void UpdateEnemyCount()
    {
        CurrentEnemyCount++;
        if (CurrentEnemyCount >= _targetEnemyCount)
        {
            // 1 Saniye Delay
            _currentEnemyText.text = CurrentEnemyCount.ToString();
            StartCoroutine(HelperMethods.DoAfterDelay(() => { GameWin(); }, 1f));
        }
        else
            _currentEnemyText.text = CurrentEnemyCount.ToString();

    }
    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            _healthBar.fillAmount = 0;
            GameOver();
        }
        else
        {
            _healthBar.fillAmount -= damage / 100;
            ShowDamageEffect();
        }

    }
    private void ShowDamageEffect()
    {
        _damageEffectImage.DOFade(0.65f, 0.5f).SetLoops(2, LoopType.Yoyo);
    }
    private void GameOver()
    {
        GameCompleted(_gameOverPanelObject);
    }
    private void GameWin()
    {
        GameCompleted(_winPanelObject);
    }
    private void GameCompleted(GameObject panel)
    {
        IsGameActive = false;
        StopAllCoroutines();
        DisableEnemies();
        SetCursorState(CursorLockMode.None);
        _cameraTransitionManager.EndGameCameraTransition();
        panel.SetActive(true);
        _gameWindowPanel.SetActive(false);
    }
    private void PauseGame()
    {
        Time.timeScale = 0;
        SetCursorState(CursorLockMode.None);
        _pausePanel.SetActive(true);
        _gameWindowPanel.SetActive(false);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        SetCursorState(CursorLockMode.Locked);
        _pausePanel.SetActive(false);
        _gameWindowPanel.SetActive(true);
    }
    private void DisableEnemies()
    {
        NavMeshAgent[] enemies = GameObject.FindObjectsOfType<NavMeshAgent>();

        foreach (var item in enemies)
        {
            item.enabled = false;
            item.gameObject.GetComponent<Animator>().Play("Idle");
        }
    }
    private void ChangeWeapon(int weaponIndex)
    {
        if (weaponIndex == _currentWeaponIndex)
            return;

        _weaponChanger.Play();
        _weaponsObject[_currentWeaponIndex].SetActive(false);
        _weaponsObject[weaponIndex].SetActive(true);
        _currentWeaponIndex = weaponIndex;
        _weaponsObject[_currentWeaponIndex].GetComponent<AmmoController>().AmmoReloadController("Normal");
    }
    private void SetCursorState(CursorLockMode cursorLockMode)
    {
        Cursor.lockState = cursorLockMode;
    }
}
