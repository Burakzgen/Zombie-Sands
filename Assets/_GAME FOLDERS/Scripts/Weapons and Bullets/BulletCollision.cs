using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ObjectPoolManager.Instance.ReturnToPool("Bullet", gameObject);
    }
}
