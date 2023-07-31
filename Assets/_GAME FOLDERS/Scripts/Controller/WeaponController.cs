using TMPro;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    /*
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
                    if (weapons[_currentWeaponIndex].totalBullet <= weapons[_currentWeaponIndex].magazineSize)
                    {
                    }
                    else
                    {
                        weapons[_currentWeaponIndex].totalBullet -= _bulletFired;
                        _remainingBullet = weapons[_currentWeaponIndex].magazineSize;
                    }

                    _bulletFired = weapons[_currentWeaponIndex].magazineSize - _remainingBullet;


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
    
   
    private void ChangeBulletText(int newWeaponIndex)
    {
        _totalBulletsText.text = weapons[newWeaponIndex].totalBullet.ToString();
        _remainingBulletsText.text = _remainingBullet.ToString();
    }
    */
}
