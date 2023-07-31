using UnityEngine;

public class ShellController : MonoBehaviour
{
    private AudioSource _fallSound;

    private void Start()
    {
        _fallSound = GetComponent<AudioSource>();
        Destroy(gameObject, 2f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _fallSound.Play();
            if (!_fallSound.isPlaying)
                Destroy(gameObject, 1f);
        }
    }

}
