using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    NavMeshAgent _agent;
    Transform _target;
    public NavMeshAgent Agent { get { return _agent; } }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        //_target = GameObject.FindWithTag("Home").transform; // Alternatif yontem
        if (_target != null)
            _agent.SetDestination(_target.transform.position);
    }
    /*
    // Belirli bir s�re sonra tekrar denemek i�in yap�ld�. 
    IEnumerator FindTarget()
    {
        while (_target == null)
        {
            _target = GameObject.FindWithTag("Home").transform;

            if (_target == null)
            {
                yield return new WaitForSeconds(5);
            }
        }
    }
    */
    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
