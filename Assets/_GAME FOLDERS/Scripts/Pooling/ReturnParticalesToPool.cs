using UnityEngine;

public class ReturnParticalesToPool : MonoBehaviour
{
    [SerializeField] private string _particalTag;
    private void OnParticleSystemStopped()
    {
        ObjectPoolManager.Instance.ReturnToPool(_particalTag, gameObject);
    }
}
