using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent _agent;
    Animator _myAnimator;
    [SerializeField] Transform _target;
    [SerializeField] private float _health;
    [SerializeField] private float _damage;
    bool _isDead;
    GameManager _gameManager;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _myAnimator = GetComponent<Animator>();

        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _isDead = false;
    }
    private void Update()
    {
        if (!GameManager.IsGameActive)
            return;

        if (_isDead)
            return;

        if (_target != null )
            _agent.SetDestination(_target.transform.position);
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
        _isDead = true;
        _agent.speed = 0;
        //TODO: Object pooling kontrolune göre ayarlanacak.
        Destroy(gameObject, 5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Home"))
        {
            _gameManager.TakeDamage(_damage);
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
    }

}
