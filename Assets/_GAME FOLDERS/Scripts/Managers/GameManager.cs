using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //STATIC
    public static bool IsGameActive;
    public static int reamingEnemyCount;
    //PRIVATE
    int _currentWeaponIndex;
    //PUBLIC
    [Header("OTHERS CONTROLS")]
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private GameObject _gameOverObject; 
    [SerializeField] private TextMeshProUGUI _remainingEnemyText;
    [SerializeField] private TextMeshProUGUI _totalEnemyText;

    [Header("WEAPON CONTROLS")]
    [SerializeField] private GameObject[] _weaponsObject;

    [Header("ENEMY CONTROLS")]
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private Transform[] _enemySpawnPoints;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private int _totalEnemyCount;

    [Header("HEALTH CONTROLS")]
    float _health;
    [SerializeField] private Image _healthBar;

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
    }
    private void InitialSettings()
    {
        //Enemy Count
        reamingEnemyCount = _totalEnemyCount;
        _totalEnemyText.text = reamingEnemyCount.ToString();
        _remainingEnemyText.text = _totalEnemyCount.ToString();

        _health = 100;
        _healthBar.fillAmount = 1f;
        _currentWeaponIndex = 0;
        //TODO: Oyun sesi aktif edilebilir.

        //TODO: Düþman oluþturma baþlatýlabilir.
        StartCoroutine(SpawnEnemy());
    }
    public void UpdateEnemyCount()
    {
        reamingEnemyCount--;
        _remainingEnemyText.text = reamingEnemyCount.ToString();
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

                GameObject obj = Instantiate(_enemies[enemy], _enemySpawnPoints[spawnPoints].transform.position, Quaternion.identity);
                obj.GetComponent<EnemyController>().SetTarget(_targetPoint);
                _totalEnemyCount--;
            }

        }
    }
    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health < 0)
        {
            _healthBar.fillAmount = 0;
            GameOver();
        }
        else
        {
            _healthBar.fillAmount -= damage / 100;
        }

    }
    private void GameOver()
    {
        IsGameActive = false;
        _cameraManager.GameOverCamPosition();
        _gameOverObject.SetActive(true);
    }
    private void ChangeWeapon(int newWeaponIndex)
    {
        //TODO: Degiþim sesi eklenebilir.
        _weaponsObject[_currentWeaponIndex].SetActive(false);
        _weaponsObject[newWeaponIndex].SetActive(true);
        _currentWeaponIndex = newWeaponIndex;
        _weaponsObject[_currentWeaponIndex].GetComponent<Weapon>().AmmoReloadController("Normal");
    }
}
