using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    //STATIC
    public static GameManager Instance { get; private set; }
    public bool IsGameActive;
    public int currentEnemyCount;
    //PRIVATE
    int _currentWeaponIndex;
    //PUBLIC
    [Header("OTHERS CONTROLS")]

    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private GameObject _gameOverPanelObject;
    [SerializeField] private GameObject _winPanelObject;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Image _damageEffectImage;
    [SerializeField] private TextMeshProUGUI _currentEnemyText;
    [SerializeField] private TextMeshProUGUI _totalEnemyText;

    [Header("WEAPON CONTROLS")]
    [SerializeField] private GameObject[] _weaponsObject;

    [Header("ENEMY CONTROLS")]
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private Transform[] _enemySpawnPoints;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private int _totalEnemyCount;
    [SerializeField] private int _targetEnemyCount;

    [Header("HEALTH CONTROLS")]
    float _health;
    [SerializeField] private Image _healthBar;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
            Instance = this;

    }
    void Start()
    {
        InitialSettings();
    }
    void Update()
    {
        if (!IsGameActive)
            return;

        if (Input.anyKey && !Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < _weaponsObject.Length; i++)
            {
                if (Input.GetKeyDown(_weaponsObject[i].GetComponent<Weapon>().activationKey))
                {
                    ChangeWeapon(i);
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale != 0)
                Pause();
            else
                ResumeGame();
        }
    }
    private void InitialSettings()
    {

        Cursor.lockState = CursorLockMode.Locked;

        //Enemy Count
        currentEnemyCount = 0;
        _totalEnemyText.text = _targetEnemyCount.ToString();
        _currentEnemyText.text = currentEnemyCount.ToString();

        _health = 100;
        _healthBar.fillAmount = 1f;
        _currentWeaponIndex = 0;
        //TODO: Oyun sesi aktif edilebilir.

        //TODO: D��man olu�turma ba�lat�labilir.
        StartCoroutine(SpawnEnemy());
    }
    public void UpdateEnemyCount()
    {
        currentEnemyCount++;
        if (currentEnemyCount >= _targetEnemyCount)
        {
            // WIN
            //TODO: Oyun durdurma i�lemi yap�lacak. Delay eklenebilri
            Win();
        }
        else
            _currentEnemyText.text = currentEnemyCount.ToString();

    }
    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            if (_totalEnemyCount != 0)
            {
                int enemy = Random.Range(0, _enemies.Length);
                int spawnPoints = Random.Range(0, _enemySpawnPoints.Length);

                GameObject obj = ObjectPoolManager.SpawnObject(_enemies[enemy], _enemySpawnPoints[spawnPoints].transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Enemy);
                obj.GetComponent<EnemyController>().SetTarget(_targetPoint);
                _totalEnemyCount--;
            }

        }
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
            DamageEffect();
        }

    }
    private void DamageEffect()
    {
        _damageEffectImage.DOFade(0.65f, 0.5f).SetLoops(2, LoopType.Yoyo);
    }
    private void GameOver()
    {
        //TODO: TimeScale yerine alternatif bir yol yap�lmal�.
        IsGameActive = false;
        StopAllCoroutines();
        Cursor.lockState = CursorLockMode.None;
        //Time.timeScale = 0;
        _cameraManager.EndGameCamEffect();
        _gameOverPanelObject.SetActive(true);
    }
    private void Win()
    {
        //TODO: TimeScale yerine alternatif bir yol yap�lmal�.
        IsGameActive = false;
        StopAllCoroutines();
        Cursor.lockState = CursorLockMode.None;
        //Time.timeScale = 0;
        _cameraManager.EndGameCamEffect();
        _winPanelObject.SetActive(true);
        _currentEnemyText.text = 0.ToString();
    }
    private void Pause()
    {
        Time.timeScale = 0;
        _pausePanel.SetActive(true);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        _pausePanel.SetActive(false);
    }
    private void ChangeWeapon(int newWeaponIndex)
    {
        //TODO: Degi�im sesi eklenebilir.
        _weaponsObject[_currentWeaponIndex].SetActive(false);
        _weaponsObject[newWeaponIndex].SetActive(true);
        _currentWeaponIndex = newWeaponIndex;
        _weaponsObject[_currentWeaponIndex].GetComponent<Weapon>().AmmoReloadController("Normal");
    }
}
