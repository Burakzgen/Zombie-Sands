using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    //PRIVATE
    Camera _cam;
    Animator _myAnimator;
    private float _fireFreq;                                    // Ate� etme s�kl��� i� kontrolu
    float _camFieldPov;
    private int _remainingBullet;                               // Kalan mermi say�s�
    private bool _isZoom;

    //PUBLIC
    [Header("MAIN SETTINGS")]
    public bool isFire;                                         // Ate� edebilir mi kontrolunu.
    public float fireRate;                                      // Ate� etme s�kl��� d�� kontrolu
    public float fireRange;                                     // Ate� etme menzili
    [SerializeField] private int _totalBullet;                   // Toplam mermi say�s�

    [Header("OTHER SETTINGS")]
    public KeyCode activationKey;
    [SerializeField] private GameObject _cross;
    [SerializeField] private GameObject _scope;
    [SerializeField] private float _approachRange;              // D�rb�n yakla�ma oran�.
    [SerializeField] private bool _isBulletCassing;             // Mermi kovan� olsun mu?
    [SerializeField] private Transform _bulletExitPointCassing; // Mermi kovan� ��k�� poziyonu.
    [SerializeField] private GameObject _bulletCassingObejct;   // Mermi kovan�
    [SerializeField] private Transform _bulletExitPoint;         // Mermi ��k�� noktas�
    [SerializeField] private GameObject _bulletObject;          // Mermi ��k�� noktas�
    [SerializeField] private bool isSniper = false;
    [Header("WEAPON SETTINGS")]
    public int magazineSize;                                    // �arj�r  kapasitesi
    [SerializeField] private string _weaponName;                // Silah ad�
    [SerializeField] private TextMeshProUGUI _totalBulletsText;                 // Toplam mermi say�s� text
    [SerializeField] private TextMeshProUGUI _remainingBulletsText;             // Kalan mermi say�s� text
    public float impactForce;                                   // Darbe g�c� (karakteri itme)

    [Header("SOUNDS")]
    [SerializeField] private AudioSource _fireSound;            // Ate� sesi
    [SerializeField] private AudioSource _reloadSound;          // Silah doldurma sesi
    [SerializeField] private AudioSource _bulletEndingSound;    // Mermi biti� sesi

    [Header("EFFECTS")]
    [SerializeField] private ParticleSystem _fireEffect;
    [SerializeField] private ParticleSystem _bloodEffect;
    [SerializeField] private ParticleSystem _bulletDecalEffect;

    #region UNITY FUNCTIONS
    private void Start()
    {
        _cam = Camera.main;
        _myAnimator = GetComponent<Animator>();
        _camFieldPov = _cam.fieldOfView;
        _isBulletCassing = true;
        AmmoReload();
        AmmoReloadController("Normal");

    }
    private void Update()
    {
        // Mermi atis
        if (Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1))
        {
            if (isFire && Time.time > _fireFreq && _remainingBullet != 0)
            {
                Fire(false);
                _fireFreq = Time.time + fireRate;
            }
            if (_remainingBullet == 0)
                Debug.Log("MERMI BITIS SESI"); //TODO: Mermi biti� sesi eklenecek
        }

        // Mermi Doldurma
        if (Input.GetKey(KeyCode.R))
        {
            if (_remainingBullet < magazineSize && _totalBullet != 0)
            {
                _myAnimator.Play("Reload");
            }
        }
        // Zoom
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _isZoom = true;
            if (isSniper)
                CameraZoom(true);
            //_myAnimator.SetBool("Zoom", true);


        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _isZoom = false;
            if (isSniper)
                CameraZoom(false);
        }

        if (_isZoom)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (isFire && Time.time > _fireFreq && _remainingBullet != 0)
                {
                    Fire(true);
                    _fireFreq = Time.time + fireRate;

                }

                if (_remainingBullet == 0)
                    Debug.Log("MERMI BITIS SESI"); //TODO: Mermi biti� sesi eklenecek
            }
        }
    }

    #endregion

    #region PRIVATE FUNCTIONS
    private void CameraZoom(bool state)
    {
        if (state)
        {
            _cross.SetActive(false);
            _cam.cullingMask = ~(1 << 8);
            _cam.fieldOfView = _approachRange;
            _scope.SetActive(true);
        }
        else
        {
            _scope.SetActive(false);
            _cam.cullingMask = -1;
            _cam.fieldOfView = _camFieldPov;
            _cross.SetActive(true);
        }
    }
    private void Fire(bool isZoom)
    {
        FireController(isZoom);
        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, fireRange))
        {

            if (hit.transform.CompareTag("Enemy"))
            {
                Instantiate(_bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.CompareTag("OverEnemy"))
            {
                Rigidbody rg = hit.transform.gameObject.GetComponent<Rigidbody>();
                rg.AddForce(-hit.normal * 50f);
                Instantiate(_bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
                Instantiate(_bulletDecalEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }

    }
    private void FireController(bool isZoom)
    {
        if (_isBulletCassing)
        {
            GameObject obejct = Instantiate(_bulletCassingObejct, _bulletExitPointCassing.transform.position, _bulletExitPointCassing.transform.rotation);
            Rigidbody rigidbody = obejct.GetComponent<Rigidbody>();
            rigidbody.AddRelativeForce(new Vector3(-10, 1, 0) * 60);
        }

        Instantiate(_bulletObject, _bulletExitPoint.transform.position, _bulletExitPoint.transform.rotation);

        //TODO: Kamera titremesi eklenebilir.
        _fireSound.Play();
        _fireEffect.Play();

        //if (!isZoom)
        //{
        //    _myAnimator.Play("NormalFire");
        //}
        //else
        //    _myAnimator.Play("ZoomFire");

        _remainingBullet--;
        _remainingBulletsText.text = _remainingBullet.ToString();


    }
    // TODO: Mermi Kaydetme Sistemi eklenebilir. Baz� zombilerin �lmesinden sonra otomatik mermi eklemesi yap�labilir.
    private void AmmoReload()
    {
        if (_totalBullet <= magazineSize)
        {
            _remainingBullet = _totalBullet;
            _totalBullet = 0;
        }
        else
        {
            _remainingBullet = magazineSize;
            _totalBullet -= magazineSize;
        }
    }
    private void ChangerReload()
    {
        _reloadSound.Play();
        if (_remainingBullet < magazineSize && _totalBullet != 0)
        {
            if (_remainingBullet != 0)
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
                    int totalValue = _remainingBullet + _totalBullet;

                    if (totalValue > magazineSize)
                    {
                        _remainingBullet = magazineSize;
                        _totalBullet = totalValue - magazineSize;
                    }
                    else
                    {
                        _remainingBullet += _totalBullet;
                        _totalBullet = 0;
                    }
                }
                else
                {
                    _totalBullet -= magazineSize - _remainingBullet;
                    _remainingBullet = magazineSize;
                }
                _totalBulletsText.text = _totalBullet.ToString();
                _remainingBulletsText.text = _remainingBullet.ToString();
                break;
            case "NoBullet":
                if (_totalBullet <= magazineSize)
                {
                    _remainingBullet = _totalBullet;
                    _totalBullet = 0;
                }
                else
                {
                    _totalBullet -= magazineSize;
                    _remainingBullet = magazineSize;
                }
                _totalBulletsText.text = _totalBullet.ToString();
                _remainingBulletsText.text = _remainingBullet.ToString();
                break;
            case "NormalText":
                _totalBulletsText.text = _totalBullet.ToString();
                _remainingBulletsText.text = _remainingBullet.ToString();
                break;
        }
    }
    #endregion
}
