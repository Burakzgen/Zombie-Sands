using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    WeaponController _weaponController;
    [Header("AMMO SETTINGS")]
    [SerializeField] private int _totalBullet;
    private int _currentBullet;
    public int CurrentBullet
    {
        get
        {
            return _currentBullet;
        }
        set
        {
            _currentBullet = value;
        }
    }
    public int magazineSize;
    [SerializeField] private TextMeshProUGUI _totalBulletsText;                 // Toplam mermi sayýsý text
    public TextMeshProUGUI _remainingBulletsText;             // Kalan mermi sayýsý text

    [SerializeField] private AudioSource _reloadSound;          // Silah doldurma sesi

    private void Start()
    {
        _weaponController = GetComponent<WeaponController>();
    }
    private void Update()
    {
        // Mermi Doldurma
        if (Input.GetKey(KeyCode.R))
        {
            if (_currentBullet < magazineSize && _totalBullet != 0)
            {
                _weaponController.WeaponAnimator.Play("Reload");
            }
        }
    }
    public void AmmoReload()
    {
        if (_totalBullet <= magazineSize)
        {
            _currentBullet = _totalBullet;
            _totalBullet = 0;
        }
        else
        {
            _currentBullet = magazineSize;
            _totalBullet -= magazineSize;
        }
        AmmoReloadController("NormalText");
    }
    private void ChangerReload()
    {
        _reloadSound.Play();
        if (_currentBullet < magazineSize && _totalBullet != 0)
        {
            if (_currentBullet != 0)
            {
                AmmoReloadController("Bullet");
            }
            else
            {
                AmmoReloadController("NoBullet");
            }

        }
    }
    public void AmmoReloadController(string type)
    {
        switch (type)
        {
            case "Bullet":
                if (_totalBullet <= magazineSize)
                {
                    int totalValue = _currentBullet + _totalBullet;

                    if (totalValue > magazineSize)
                    {
                        _currentBullet = magazineSize;
                        _totalBullet = totalValue - magazineSize;
                    }
                    else
                    {
                        _currentBullet += _totalBullet;
                        _totalBullet = 0;
                    }
                }
                else
                {
                    _totalBullet -= magazineSize - _currentBullet;
                    _currentBullet = magazineSize;
                }
                _totalBulletsText.text = _totalBullet.ToString();
                _remainingBulletsText.text = _currentBullet.ToString();
                break;
            case "NoBullet":
                if (_totalBullet <= magazineSize)
                {
                    _currentBullet = _totalBullet;
                    _totalBullet = 0;
                }
                else
                {
                    _totalBullet -= magazineSize;
                    _currentBullet = magazineSize;
                }
                _totalBulletsText.text = _totalBullet.ToString();
                _remainingBulletsText.text = _currentBullet.ToString();
                break;
            case "NormalText":
                _totalBulletsText.text = _totalBullet.ToString();
                _remainingBulletsText.text = _currentBullet.ToString();
                break;
        }
    }
}
