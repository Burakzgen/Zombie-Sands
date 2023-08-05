using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] private float bulletSpeed = 100f;
    [SerializeField] private float accuracy = 0.5f;


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
        //rb.velocity = transform.forward * bulletSpeed;

        // Mermilerde hafif bir sapma
        Vector3 shootDirection = transform.forward;

        shootDirection += new Vector3(HelperMethods.GetFloat(-accuracy, accuracy), HelperMethods.GetFloat(-accuracy, accuracy), 0);

        rb.velocity = shootDirection * bulletSpeed;
    }
    private void ReturnBulletToPool()
    {
        StartCoroutine(HelperMethods.DoAfterDelay(() =>
        {
            ObjectPoolManager.Instance.ReturnToPool("Bullet", gameObject);
        }, 2f));
    }
}
