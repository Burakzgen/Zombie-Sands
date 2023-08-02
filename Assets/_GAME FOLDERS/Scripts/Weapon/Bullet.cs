using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.velocity = transform.forward * 3;
        rb.rotation = Quaternion.identity;
        StartCoroutine(ReturnToPoolAfterTime(2f));
    }


    private void OnTriggerEnter(Collider other)
    {
        //ObjectPoolManager.ReturnObjectToPool(gameObject);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    IEnumerator ReturnToPoolAfterTime(float _delayTime)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _delayTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

}
