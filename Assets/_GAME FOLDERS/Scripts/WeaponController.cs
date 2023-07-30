using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public bool isFire;
    private float _fireFreq;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private GameObject[] weaponObjects;
    Camera _cam;
    private int _currentWeaponIndex;

    [SerializeField] private ParticleSystem _bloodEffect;

    private void Start()
    {
        _currentWeaponIndex = 0;
        _cam = Camera.main;

        for (int i = 0; i < weapons.Length; i++)
        {
            weaponObjects[i].SetActive(i == 0); // Only first weapon is active
        }
    }
    private void Update()
    {
        if (Input.anyKey)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (Input.GetKeyDown(weapons[i].activationKey))
                {
                    ChangeWeapon(i);
                }
            }
        }

        if (Input.GetKey(KeyCode.Mouse0) && isFire && Time.time > _fireFreq)
        {
            Fire();
            _fireFreq = Time.time + weapons[_currentWeaponIndex].reloadTime;
        }
    }
    private void Fire()
    {
        weaponObjects[_currentWeaponIndex].GetComponent<Animator>().Play("WeaponFire");
        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, weapons[_currentWeaponIndex].fireRate))
        {
            //TODO: Fire sesi eklenebilir.
            //TODO: Effectide (mermi cikis-Muzzle effect) eklenebilir.
            if (hit.transform.CompareTag("Enemy"))
            {
                Instantiate(_bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
                Instantiate(weapons[_currentWeaponIndex].bulletDecalEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }

    }
    private void ChangeWeapon(int newWeaponIndex)
    {
        weaponObjects[_currentWeaponIndex].SetActive(false);
        weaponObjects[newWeaponIndex].SetActive(true);
        _currentWeaponIndex = newWeaponIndex;
    }
}
