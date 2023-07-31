using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent _agent;
    Animator _myAnimator;
    [SerializeField] Transform _target;
    [SerializeField] private float _health;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _myAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!GameManager.IsGameActive)
            return;

        if (_target != null)
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
        //TODO: Dusman sayýsýný güncelleme yapýlacak.
        _myAnimator.Play("Normal_Shot_Dead");
        //TODO: Object pooling kontrolune göre ayarlanacak.
        Destroy(gameObject, 5f);
    }
}
