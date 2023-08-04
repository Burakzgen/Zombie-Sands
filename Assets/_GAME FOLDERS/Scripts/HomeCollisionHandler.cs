using UnityEngine;

public class HomeCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Basic_Zombie") || other.gameObject.CompareTag("Normal_Zombie") || other.gameObject.CompareTag("Hard_Zombie"))
        {
            float damage = other.gameObject.GetComponent<EnemyHealth>().Damage;
            GameManager.Instance.TakeDamage(damage);
            other.gameObject.SetActive(false);
            //ObjectPoolManager.Instance.ReturnToPool(other.gameObject.tag, other.gameObject);
        }
    }
}
