using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnParticalesToPool : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
