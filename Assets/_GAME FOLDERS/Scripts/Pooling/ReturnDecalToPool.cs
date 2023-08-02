using System.Collections;
using UnityEngine;

public class ReturnDecalToPool : MonoBehaviour {

	public float lifeTime = 5.0f;

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(lifeTime);
		ObjectPoolManager.Instance.ReturnToPool("BulletImpactEffect", gameObject);
    }
}
