using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingEffect: MonoBehaviour
{
    Transform _transform;

    void Start()
    {
        _transform = GetComponent<Transform>();
        StartCoroutine(ApplyBreathingEffect());
    }

    IEnumerator ApplyBreathingEffect()
    {
        while (true)
        {
            float duration = Random.Range(0.5f, 1.5f);

            _transform.DOScale(1.1f, duration);
            yield return new WaitForSeconds(duration);

            _transform.DOScale(1f, duration);
            yield return new WaitForSeconds(duration);
        }
    }
}

