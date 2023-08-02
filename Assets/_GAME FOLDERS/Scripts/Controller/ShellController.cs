using System.Collections;
using UnityEngine;

public class ShellController : MonoBehaviour
{
    private AudioSource _fallSound;
    private IEnumerator Start()
    {
        _fallSound = GetComponent<AudioSource>();

        yield return new WaitForSeconds(1f);
        _fallSound.Play();
        if (!_fallSound.isPlaying)
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        yield return new WaitForSeconds(2f);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
        //Destroy(gameObject, 2f);
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        _fallSound.Play();
    //        if (!_fallSound.isPlaying)
    //            StartCoroutine(ReturnToPoolAfterTime());
    //        //Destroy(gameObject, 1f);
    //    }
    //}
}
