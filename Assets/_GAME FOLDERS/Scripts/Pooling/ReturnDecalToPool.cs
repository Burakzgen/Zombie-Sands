using System.Collections;
using UnityEngine;

public class ReturnDecalToPool : MonoBehaviour
{
    private float _lifeTime;
    private void OnEnable()
    {
        _lifeTime = GameManager.Instance._currentWeaponIndex switch
        {
            0 => 2.2f,
            1 => 1f,
            2 => 3f,
            _ => 2.75f,
        };
        StartCoroutine(ReturnPool());
    }
    private IEnumerator ReturnPool()
    {
        yield return new WaitForSeconds(_lifeTime);
        ObjectPoolManager.Instance.ReturnToPool("BulletImpactEffect", gameObject);
    }
}
