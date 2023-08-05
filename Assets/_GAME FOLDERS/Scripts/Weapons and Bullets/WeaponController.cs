using DG.Tweening;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("CONTROLLERS")]
    [SerializeField] private MouseAimController _cameraController;
    private AmmoController _ammoController;

    [Header("ANIMATORS")]
    Animator _myAnimator;
    public Animator WeaponAnimator { get { return _myAnimator; } }
    [SerializeField] private string _animationStateName;

    [Header("FIRING SETTINGS")]
    [SerializeField] private Transform _bulletExitPoint;
    //[SerializeField] private GameObject _bulletObject;
    [SerializeField] private AudioSource _fireSound;
    [SerializeField] private ParticleSystem _fireEffect;
    public bool isFire;                                         // Ateþ edebilir mi kontrolunu.
    public float fireRate;
    public float damage;
    public float fireRange;
    private float _fireFreq;
    [SerializeField] private bool _isBulletCassing;             // Mermi kovaný olsun mu?
    [SerializeField] private Transform _bulletExitPointCassing; // Mermi kovaný çýkýþ poziyonu.

    [Header("ZOOM SETTINGS")]
    private bool _isAiming;

    [SerializeField] private bool isSniper;
    [SerializeField] private UIController _uiController;
    float _camFieldPov;
    [SerializeField] private float _approachRange;              // Dürbün yaklaþma oraný.

    [Header("SOUND SETTINGS")]
    [SerializeField] private AudioSource _bulletEndingSound;    // Mermi bitiþ sesi

    [Header("EFFECTS")]
    [SerializeField] private ParticleSystem _bloodEffect;
    [SerializeField] private ParticleSystem _bulletDecalEffect;

    [Header("OTHER SETTINGS")]
    Camera _cam;
    [SerializeField] private bool isCameraShake;
    public KeyCode activationKey;


    #region UNITY FUNCTIONS
    private void Awake()
    {
        _ammoController = GetComponent<AmmoController>();
    }
    private void OnEnable()
    {
        _ammoController.AmmoReloadController("NormalText");
    }
    private void Start()
    {
        _cam = Camera.main;
        _myAnimator = GetComponent<Animator>();
        _camFieldPov = _cam.fieldOfView;
        _isBulletCassing = true;
        _ammoController.AmmoReload();

    }
    private void Update()
    {
        if (!GameManager.Instance.IsGameActive)
            return;

        // Mermi atis
        if (Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1))
        {
            AttemptFire();
        }
        // Zoom
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _isAiming = true;

            CameraZoom(true);

        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _isAiming = false;

            CameraZoom(false);
        }

        if (_isAiming)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                AttemptFire();
            }
        }
    }

    #endregion
    private void AttemptFire()
    {
        if (isFire && Time.time > _fireFreq && _ammoController.CurrentBullet != 0)
        {
            Fire();
            _fireFreq = Time.time + fireRate;
        }

        if (_ammoController.CurrentBullet == 0)
            _bulletEndingSound.Play();
    }
    private void Fire()
    {
        FireController();
        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, fireRange))
        {
            HandleHit(hit);
        }

    }
    private void HandleHit(RaycastHit hit)
    {
        if (hit.transform.gameObject.CompareTag("Head"))
        {
            hit.transform.GetComponentInParent<EnemyHealth>().TakeDamage(damage * 2);
            ObjectPoolManager.Instance.SpawnFromPool("BloodEffect", hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if (hit.transform.gameObject.CompareTag("Body"))
        {
            hit.transform.GetComponentInParent<EnemyHealth>().TakeDamage(damage);
            ObjectPoolManager.Instance.SpawnFromPool("BloodEffect", hit.point, Quaternion.LookRotation(hit.normal));
        }
        /* // Duruma gore ilave edilebilir.(Karakteri geri itme ya da nesneleri geri itme gibi)
        else if (hit.transform.CompareTag("Force"))
        {
            Rigidbody rg = hit.transform.gameObject.GetComponent<Rigidbody>();
            rg.AddForce(-hit.normal * 50f);
        }
        */
        else
        {
            GameObject bulletImpact = ObjectPoolManager.Instance.SpawnFromPool("BulletImpactEffect", hit.point, Quaternion.LookRotation(hit.normal));
            bulletImpact.transform.localScale = GameManager.Instance._currentWeaponIndex switch
            {
                0 => new Vector3(0.75f, 0.75f, 0.75f),
                1 => new Vector3(0.5f, 0.5f, 0.5f),
                2 => Vector3.one,
                _ => new Vector3(0.75f, 0.75f, 0.75f),
            };
        }
    }
    private void FireController()
    {
        if (_isBulletCassing)
        {
            GameObject casing = ObjectPoolManager.Instance.SpawnFromPool("Casing", _bulletExitPointCassing.transform.position, _bulletExitPointCassing.transform.rotation);
            Rigidbody rigidbody = casing.GetComponent<Rigidbody>();
            rigidbody.AddRelativeForce(new Vector3(-5f,4f, 5f) * 25);

        }

        ObjectPoolManager.Instance.SpawnFromPool("Bullet", _bulletExitPoint.transform.position, _bulletExitPoint.transform.rotation);

        if (isCameraShake)
        {
            _cam.DOShakePosition(0.2f, 0.05f, 3, 90, true);
        }

        _fireSound.Play();
        _fireEffect.Play();

        _myAnimator.Play(_animationStateName);

        _ammoController.CurrentBullet--;
        _ammoController._remainingBulletsText.text = _ammoController.CurrentBullet.ToString();


    }
    private void CameraZoom(bool state)
    {
        if (state)
        {
            if (isSniper)
            {
                _uiController.SetCrosshairActive(false);
                _uiController.SetScopeActive(true);
                _cam.cullingMask = ~(1 << 8);
                _cameraController.mouseSensitivity = 10;
            }
            _cam.fieldOfView = _approachRange;
        }
        else
        {
            if (isSniper)
            {
                _uiController.SetScopeActive(false);
                _uiController.SetCrosshairActive(true);
                _cam.cullingMask = -1;
                _cameraController.mouseSensitivity = 100;
            }
            _cam.fieldOfView = _camFieldPov;
        }
    }
}
