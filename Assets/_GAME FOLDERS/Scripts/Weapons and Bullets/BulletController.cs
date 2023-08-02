using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.velocity = transform.forward * 150f;
        rb.rotation = Quaternion.identity;
        StartCoroutine(HelperMethods.DoAfterDelay(() =>
        {
            ObjectPoolManager.Instance.ReturnToPool("Bullet", gameObject);
        }, 2f));
    }
    private void OnTriggerEnter(Collider other)
    {
        ObjectPoolManager.Instance.ReturnToPool("Bullet", gameObject);
    }

    //IEnumerator ReturnToPoolAfterTime(float _delayTime)
    //{
    //    float elapsedTime = 0f;
    //    while (elapsedTime < _delayTime)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //    ObjectPoolManager.Instance.ReturnToPool("Bullet", gameObject);
    //}

}
