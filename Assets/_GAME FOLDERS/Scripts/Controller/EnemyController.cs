using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //PRIVATE
    NavMeshAgent _agent;
    Animator _myAnimator;
    GameManager _gameManager;
    [Header("TARGET CONTROLS")]
    [SerializeField] Transform _target;

    [Header("OTHER SETTINGS")]
    [SerializeField] private float _health;
    [SerializeField] private float _damage;

    [Header("DISSOLVE CONTROLS")]
    [SerializeField] private SkinnedMeshRenderer _skinnedMesh;
    [SerializeField] private MeshRenderer _headMesh;
    private Material _dissolveBodyMaterial;
    private Material _dissolveHeadMaterial;
    [SerializeField] private float DissolveRate = 0.0125f;
    [SerializeField] private float RefleshRate = 0.025f;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _myAnimator = GetComponent<Animator>();

        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        //_isDead = false;

        if (_skinnedMesh != null)
            _dissolveBodyMaterial = _skinnedMesh.material;
        if (_headMesh != null)
            _dissolveHeadMaterial = _headMesh.material;
        if (_target != null)
            _agent.SetDestination(_target.transform.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Home"))
        {
            _gameManager.TakeDamage(_damage);
            _gameManager.UpdateEnemyCount();
            gameObject.GetComponent<CharacterController>().enabled = false;
            Destroy(gameObject, 0.5f);
        }
    }
    public void SetTarget(Transform target)
    {
        _target = target;
    }
    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Dead();
            gameObject.tag = "Untagged";
        }
    }

    private void Dead()
    {
        _gameManager.UpdateEnemyCount();
        _myAnimator.Play("Normal_Shot_Dead");
        _agent.speed = 0;
        StartCoroutine(DissolveCoroutine());
        //TODO: Object pooling kontrolune göre ayarlanacak.
        Destroy(gameObject, 5f);
    }
    private IEnumerator DissolveCoroutine()
    {
        float counter = 0;
        while (_dissolveBodyMaterial.GetFloat("_DissolveAmount") < 1)
        {
            counter += DissolveRate;
            _dissolveBodyMaterial.SetFloat("_DissolveAmount", counter);
            _dissolveHeadMaterial.SetFloat("_DissolveAmount", counter);
            yield return new WaitForSeconds(RefleshRate);
        }


    }

}
