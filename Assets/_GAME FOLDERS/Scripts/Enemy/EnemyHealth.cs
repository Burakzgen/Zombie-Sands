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
        // Colliderlarýn kapatýlmasý
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (var item in colliders)
        {
            item.enabled = false;
        }

        GameManager.Instance.UpdateEnemyCount();

        // Animasyon ve effektin oluþmasý
        _enemyAppearance.EnemyAnimator.Play(deadStyle);
        _enemyAppearance.DissolveEffect();

        // Nav mesh sisteminin devre dýþý býrakýlmasý
        _enemyMovement.Agent.enabled = false;

        StartCoroutine(HelperMethods.DoAfterDelay(() => gameObject.SetActive(false), 5f));
    }
}
