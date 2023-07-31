using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent _agent;
    Animator _myAnimator;
    [SerializeField] GameObject _target;
    [SerializeField] private float _health;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _myAnimator=GetComponent<Animator>();
    }
    private void Update()
    {
        _agent.SetDestination(_target.transform.position);  
    }
    public void SetTarget(GameObject target)
    {
        _target = target;
    }
    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health<=0)
        {
            Dead();
            gameObject.tag = "Untagged";
        }
    }

    private void Dead()
    {
        //TODO: Dusman say�s�n� g�ncelleme yap�lacak.
        _myAnimator.Play("Normal_Shot_Dead");
        //TODO: Object pooling kontrolune g�re ayarlanacak.
        Destroy(gameObject, 5f);
    }
}
