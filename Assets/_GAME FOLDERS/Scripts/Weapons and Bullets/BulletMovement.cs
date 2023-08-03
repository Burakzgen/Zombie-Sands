using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        BulletMove();
        ReturnBulletToPool();
    }
    private void OnDisable()
    {
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
    private void BulletMove()
    {
        rb.velocity = transform.forward * 150f;
    }
    private void ReturnBulletToPool()
    {
        StartCoroutine(HelperMethods.DoAfterDelay(() =>
        {
            ObjectPoolManager.Instance.ReturnToPool("Bullet", gameObject);
        }, 2f));
    }
}
