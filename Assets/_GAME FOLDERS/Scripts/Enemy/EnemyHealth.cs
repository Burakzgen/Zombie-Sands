using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _damage;
    private EnemyAppearance _enemyAppearance;
    private EnemyMovement _enemyMovement;

    public float Damage { get { return _damage; } }
    private void Start()
    {
        _enemyAppearance = GetComponent<EnemyAppearance>();
        _enemyMovement = GetComponent<EnemyMovement>();
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
        GameManager.Instance.UpdateEnemyCount();
        _enemyAppearance.EnemyAnimator.Play(deadStyle);
        _enemyAppearance.DissolveEffect();
        _enemyMovement.Agent.enabled = false;

        StartCoroutine(HelperMethods.DoAfterDelay(() => ObjectPoolManager.Instance.ReturnToPool(gameObject.tag, gameObject), 5f));
    }
}
