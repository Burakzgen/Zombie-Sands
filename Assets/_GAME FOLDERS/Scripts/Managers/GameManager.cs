using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //STATIC
    public static bool IsGameActive;
    //PRIVATE
    int _currentWeaponIndex;
    //PUBLIC
    [Header("WEAPON CONTROLS")]
    [SerializeField] private GameObject[] _weaponsObject;
    [Header("ENEMY CONTROLS")]
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private Transform[] _enemySpawnPoints;
    [SerializeField] private Transform _targetPoint;
    void Start()
    {
        InitialSettings();
    }
    void Update()
    {
        if (!IsGameActive)
            return;

        if (Input.anyKey&& !Input.GetKey(KeyCode.Mouse1))
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
    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(8f);
            int enemy = Random.Range(0, _enemies.Length);
            int spawnPoints = Random.Range(0, _enemySpawnPoints.Length);

            GameObject obj = Instantiate(_enemies[enemy], _enemySpawnPoints[spawnPoints].transform.position, Quaternion.identity);
            obj.GetComponent<EnemyController>().SetTarget(_targetPoint);
        }
    }
    private void InitialSettings()
    {
        //IsGameActive = false;
        _currentWeaponIndex = 0;
        //TODO: Oyun sesi aktif edilebilir.
        //TODO: Düþman oluþturma baþlatýlabilir.
        StartCoroutine(SpawnEnemy());
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
