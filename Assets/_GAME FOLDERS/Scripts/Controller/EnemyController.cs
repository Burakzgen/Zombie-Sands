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
    [SerializeField] private string[] _randomWalkAnimation;
    [Header("ENEMY SETTINGS")]
    [SerializeField] private GameObject[] _heads;
    [SerializeField] private GameObject[] _bodies;
    int randomBodyNumber;
    int randomHeadNumber;
    [Header("DISSOLVE CONTROLS")]
    private Material _dissolveBodyMaterial;
    private Material _dissolveHeadMaterial;
    [SerializeField] private float DissolveRate = 0.0125f;
    [SerializeField] private float RefleshRate = 0.025f;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _myAnimator = GetComponent<Animator>();

        randomHeadNumber = Random.Range(0, _heads.Length);
        randomBodyNumber = Random.Range(0, _bodies.Length);

        int index = Random.Range(0, _randomWalkAnimation.Length);
        _myAnimator.Play(_randomWalkAnimation[index]);

        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        //_isDead = false;
        RandomPlayer();
        _dissolveBodyMaterial = _bodies[randomBodyNumber].GetComponent<SkinnedMeshRenderer>().material;
        _dissolveHeadMaterial = _heads[randomHeadNumber].GetComponent<MeshRenderer>().material;

        if (_target != null)
            _agent.SetDestination(_target.transform.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Home"))
        {
            _gameManager.TakeDamage(_damage);
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
            if (damage >= 100)
                Dead("Head_Shot");
            else
                Dead("Normal_Shot_Dead");
        }
    }

    private void Dead(string deadStyle)
    {
        _gameManager.UpdateEnemyCount();
        _myAnimator.Play(deadStyle);
        StartCoroutine(DissolveCoroutine());
        _agent.enabled = false;
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
    private void RandomPlayer()
    {
        for (int i = 0; i < _heads.Length; i++)
        {
            _heads[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _bodies.Length; i++)
        {
            _bodies[i].gameObject.SetActive(false);
        }
        _heads[randomHeadNumber].gameObject.SetActive(true);
        _bodies[randomBodyNumber].gameObject.SetActive(true);
    }

}
