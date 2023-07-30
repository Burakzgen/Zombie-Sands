using TMPro;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    Camera _cam;
    [Header("WEAPON SETTINGS")]
    private int _currentWeaponIndex;

    private float _fireFreq;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private GameObject[] weaponObjects;
    public bool isFire;
    [SerializeField] private TextMeshProUGUI _totalBulletsText;
    [SerializeField] private TextMeshProUGUI _remainingBulletsText;
    [Header("PARTICAL SYSTEM")]
    [SerializeField] private ParticleSystem _bloodEffect;

    private void Start()
    {
        _currentWeaponIndex = 0;
        ChangeText(0);
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

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (isFire && Time.time > _fireFreq && weapons[_currentWeaponIndex].remainingBullet != 0)
            {
                Fire();
                _fireFreq = Time.time + weapons[_currentWeaponIndex].reloadTime;
            }
            else
            {
                //TODO: Mermi bitis sesi eklenecek.
            }
        }
    }
    private void Fire()
    {
        //TODO: Fire sesi eklenebilir.
        //TODO: Effectide (mermi cikis-Muzzle effect) eklenebilir.

        weaponObjects[_currentWeaponIndex].GetComponent<Animator>().Play(weapons[_currentWeaponIndex].fireAnimation);

        weapons[_currentWeaponIndex].remainingBullet--;
        _remainingBulletsText.text = weapons[_currentWeaponIndex].remainingBullet.ToString();

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
        weaponObjects[_currentWeaponIndex].SetActive(false);
        weaponObjects[newWeaponIndex].SetActive(true);
        _currentWeaponIndex = newWeaponIndex;
        ChangeText(_currentWeaponIndex);
    }
    private void ChangeText(int newWeaponIndex)
    {
        _totalBulletsText.text = weapons[newWeaponIndex].totalBullet.ToString();
        _remainingBulletsText.text = weapons[newWeaponIndex].remainingBullet.ToString();
    }
}
