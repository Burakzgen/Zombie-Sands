using System.Collections;
using UnityEngine;

public class ReturnCasingToPool : MonoBehaviour
{
    private AudioSource _fallSound;
    private IEnumerator Start()
    {
        _fallSound = GetComponent<AudioSource>();

        yield return new WaitForSeconds(1f);
        _fallSound.Play();

        if (!_fallSound.isPlaying)
            ObjectPoolManager.Instance.ReturnToPool("Casing", gameObject);

        yield return new WaitForSeconds(2f);
        ObjectPoolManager.Instance.ReturnToPool("Casing", gameObject);
    }
}
