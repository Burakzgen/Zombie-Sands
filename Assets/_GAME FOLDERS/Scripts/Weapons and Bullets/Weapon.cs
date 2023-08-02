using UnityEngine;
using TMPro;
using DG.Tweening;

public class Weapon : MonoBehaviour
{
    //PRIVATE
    Camera _cam;
    Animator _myAnimator;
    private float _fireFreq;                                    // Ateþ etme sýklýðý iç kontrolu
    float _camFieldPov;
    private int _currentBullet;                               // Kalan mermi sayýsý
    private bool _isZoom;

    //PUBLIC
    [Header("MAIN SETTINGS")]
    public bool isFire;                                         // Ateþ edebilir mi kontrolunu.
    public float fireRate;                                      // Ateþ etme sýklýðý dýþ kontrolu
    public float fireRange;                                     // Ateþ etme menzili
    [SerializeField] private int _totalBullet;                   // Toplam mermi sayýsý
    [SerializeField] private MouseAimController _cameraController;

    [Header("OTHER SETTINGS")]
    public KeyCode activationKey;
    [SerializeField] private GameObject _cross;
    [SerializeField] private GameObject _scope;
    [SerializeField] private float _approachRange;              // Dürbün yaklaþma oraný.
    [SerializeField] private bool _isBulletCassing;             // Mermi kovaný olsun mu?
    [SerializeField] private Transform _bulletExitPointCassing; // Mermi kovaný çýkýþ poziyonu.
    [SerializeField] private GameObject _bulletCassingObejct;   // Mermi kovaný
    [SerializeField] private Transform _bulletExitPoint;         // Mermi çýkýþ noktasý
    [SerializeField] private GameObject _bulletObject;          // Mermi çýkýþ noktasý
    [SerializeField] private bool isSniper = false;
    [SerializeField] private bool isCameraShake = false;
    [SerializeField] private string _animationStateName;
    [Header("WEAPON SETTINGS")]
    public int magazineSize;                                    // Þarjör  kapasitesi
    //[SerializeField] private string _weaponName;                // Silah adý (PlayerPrefs için tutulancak)
    [SerializeField] private TextMeshProUGUI _totalBulletsText;                 // Toplam mermi sayýsý text
    [SerializeField] private TextMeshProUGUI _remainingBulletsText;             // Kalan mermi sayýsý text
    public float damage;                                   // Darbe gücü (karakteri itme)

    [Header("SOUNDS")]
    [SerializeField] private AudioSource _fireSound;            // Ateþ sesi
    [SerializeField] private AudioSource _reloadSound;          // Silah doldurma sesi
    [SerializeField] private AudioSource _bulletEndingSound;    // Mermi bitiþ sesi

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

    }
    private void OnEnable()
    {
        AmmoReloadController("NormalText");
    }
    private void Update()
    {
        if (!GameManager.Instance.IsGameActive)
            return;

        // Mermi atis
        if (Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1))
        {
            if (isFire && Time.time > _fireFreq && _currentBullet != 0)
            {
                Fire();
                _fireFreq = Time.time + fireRate;
            }
            if (_currentBullet == 0)
                _bulletEndingSound.Play();
        }

        // Mermi Doldurma
        if (Input.GetKey(KeyCode.R))
        {
            if (_currentBullet < magazineSize && _totalBullet != 0)
            {
                _myAnimator.Play("Reload");
            }
        }
        // Zoom
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _isZoom = true;

            CameraZoom(true);

        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _isZoom = false;

            CameraZoom(false);
        }

        if (_isZoom)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (isFire && Time.time > _fireFreq && _currentBullet != 0)
                {
                    Fire();
                    _fireFreq = Time.time + fireRate;

                }

                if (_currentBullet == 0)
                    _bulletEndingSound.Play();
            }
        }
    }

    #endregion

    #region PRIVATE FUNCTIONS
    private void CameraZoom(bool state)
    {
        if (state)
        {
            if (isSniper)
            {
                _cross.SetActive(false);
                _scope.SetActive(true);
                _cam.cullingMask = ~(1 << 8);
                _cameraController.mouseSensitivity = 10;
            }
            _cam.fieldOfView = _approachRange;
        }
        else
        {
            if (isSniper)
            {
                _scope.SetActive(false);
                _cross.SetActive(true);
                _cam.cullingMask = -1;
                _cameraController.mouseSensitivity = 100;
            }
            _cam.fieldOfView = _camFieldPov;
        }
    }
    private void Fire()
    {
        FireController();
        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, fireRange))
        {
            if (hit.transform.gameObject.CompareTag("Head"))
            {
                hit.transform.GetComponentInParent<EnemyController>().TakeDamage(damage * 2);
                ObjectPoolManager.Instance.SpawnFromPool("BloodEffect", hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.gameObject.CompareTag("Body"))
            {
                hit.transform.GetComponentInParent<EnemyController>().TakeDamage(damage);
                ObjectPoolManager.Instance.SpawnFromPool("BloodEffect", hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.CompareTag("OverEnemy")) // Duruma gore ilave yapilabilir
            {
                hit.transform.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
                Rigidbody rg = hit.transform.gameObject.GetComponent<Rigidbody>();
                rg.AddForce(-hit.normal * 50f);
                ObjectPoolManager.Instance.SpawnFromPool("BloodEffect", hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
                ObjectPoolManager.Instance.SpawnFromPool("BulletImpactEffect", hit.point, Quaternion.LookRotation(hit.normal));
        }

    }
    private void FireController()
    {
        if (_isBulletCassing)
        {
            GameObject casing = ObjectPoolManager.Instance.SpawnFromPool("Casing", _bulletExitPointCassing.transform.position, _bulletExitPointCassing.transform.rotation);
            Rigidbody rigidbody = casing.GetComponent<Rigidbody>();
            rigidbody.AddRelativeForce(new Vector3(-10, 1, 0) * 125);

        }
        ObjectPoolManager.Instance.SpawnFromPool("Bullet", _bulletExitPoint.transform.position, _bulletExitPoint.transform.rotation);

        if (isCameraShake)
        {
            _cam.DOShakePosition(0.2f, 0.05f, 3, 90, true);
        }

        _fireSound.Play();
        _fireEffect.Play();

        _myAnimator.Play(_animationStateName);

        _currentBullet--;
        _remainingBulletsText.text = _currentBullet.ToString();


    }
    // TODO: Mermi Kaydetme Sistemi eklenebilir. Bazý zombilerin ölmesinden sonra otomatik mermi eklemesi yapýlabilir.
    private void AmmoReload()
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
    // Animasyon üzerinden kontrol ediliyor.
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
    #endregion
}
