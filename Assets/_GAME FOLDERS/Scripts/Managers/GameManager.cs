using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //STATIC
    //public static bool IsGameActive;
    //PRIVATE
    int _currentWeaponIndex;
    //PUBLIC
    [SerializeField] private GameObject[] weaponsObject;
    void Start()
    {
        InitialSettings();
    }

    void Update()
    {
        if (Input.anyKey&& !Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < weaponsObject.Length; i++)
            {
                if (Input.GetKeyDown(weaponsObject[i].GetComponent<Weapon>().activationKey))
                {
                    ChangeWeapon(i);
                }
            }

        }
    }
    private void InitialSettings()
    {
        //IsGameActive = false;
        _currentWeaponIndex = 0;
        //TODO: Oyun sesi aktif edilebilir.
        //TODO: Düþman oluþturma baþlatýlabilir.
    }
    private void ChangeWeapon(int newWeaponIndex)
    {
        //TODO: Degiþim sesi eklenebilir.
        weaponsObject[_currentWeaponIndex].SetActive(false);
        weaponsObject[newWeaponIndex].SetActive(true);
        _currentWeaponIndex = newWeaponIndex;
        weaponsObject[_currentWeaponIndex].GetComponent<Weapon>().AmmoReloadController("Normal");
    }
}
