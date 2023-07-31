using TMPro;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    Camera _cam;
    [Header("WEAPON SETTINGS")]
    private int _currentWeaponIndex;
    private float _fireFreq;
    private int _remainingBullet;
    private int _bulletFired;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private GameObject[] weaponsObject;
    public bool isFire;
    [SerializeField] private TextMeshProUGUI _totalBulletsText;
    [SerializeField] private TextMeshProUGUI _remainingBulletsText;
    [Header("PARTICAL SYSTEM")]
    [SerializeField] private ParticleSystem _bloodEffect;

    private void Start()
    {
        _currentWeaponIndex = 0;

        _remainingBullet = weapons[_currentWeaponIndex].magazineSize;

        ChangeBulletText(0);
        _cam = Camera.main;

        for (int i = 0; i < weapons.Length; i++)
        {
            weaponsObject[i].SetActive(i == 0); // Only first weapon is active
        }
    }
    private void Update()
    {
        // Silah degistirme
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
        // Mermi doldurma
        if (Input.GetKey(KeyCode.R))
        {
            if (_remainingBullet < weapons[_currentWeaponIndex].magazineSize && weapons[_currentWeaponIndex].totalBullet != 0)
            {
                if (_remainingBullet != 0)
                {
                    _bulletFired = weapons[_currentWeaponIndex].magazineSize - _remainingBullet;
                    weapons[_currentWeaponIndex].totalBullet -= _bulletFired;
                    _remainingBullet = weapons[_currentWeaponIndex].magazineSize;

                    ChangeBulletText(_currentWeaponIndex);
                }
                else
                {
                    if (weapons[_currentWeaponIndex].totalBullet <= weapons[_currentWeaponIndex].magazineSize)
                    {
                        _remainingBullet = weapons[_currentWeaponIndex].totalBullet;
                        weapons[_currentWeaponIndex].totalBullet = 0;
                    }
                    else
                    {
                        weapons[_currentWeaponIndex].totalBullet -= weapons[_currentWeaponIndex].magazineSize;
                        _remainingBullet = weapons[_currentWeaponIndex].magazineSize;
                    }

                    ChangeBulletText(_currentWeaponIndex);
                }

                //TODO: Animasyon eklenebilir. Sarjor degisim animasyonu(silah yana kayabilir)
            }
        }
        // Mermi atis
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (isFire && Time.time > _fireFreq && _remainingBullet != 0)
            {
                Fire();
                _fireFreq = Time.time + weapons[_currentWeaponIndex].reloadTime;
            }
            else
            {
                //TODO: Mermi bitis sesi eklenebilir.
            }
        }
    }
    private void Fire()
    {
        //TODO: Fire sesi eklenebilir.
        //TODO: Effectide (mermi cikis-Muzzle effect) eklenebilir.

        weaponsObject[_currentWeaponIndex].GetComponent<Animator>().Play(weapons[_currentWeaponIndex].fireAnimation);

        _remainingBullet--;
        _remainingBulletsText.text = _remainingBullet.ToString();

        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, weapons[_currentWeaponIndex].fireRate))
        {

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
        weaponsObject[_currentWeaponIndex].SetActive(false);
        weaponsObject[newWeaponIndex].SetActive(true);
        _currentWeaponIndex = newWeaponIndex;
        _remainingBullet = weapons[_currentWeaponIndex].magazineSize;
        ChangeBulletText(_currentWeaponIndex);
    }
    private void ChangeBulletText(int newWeaponIndex)
    {
        _totalBulletsText.text = weapons[newWeaponIndex].totalBullet.ToString();
        _remainingBulletsText.text = _remainingBullet.ToString();
    }
}
