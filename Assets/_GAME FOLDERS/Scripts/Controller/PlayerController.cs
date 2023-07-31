using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Transform chest;

    void Start()
    {
        chest = GetComponent<Transform>();
        StartCoroutine(Breathe());
    }

    IEnumerator Breathe()
    {
        while (GameManager.IsGameActive)
        {
            float duration = Random.Range(0.5f, 1.5f); 

            chest.DOScale(1.1f, duration);
            yield return new WaitForSeconds(duration);

            chest.DOScale(1f, duration);
            yield return new WaitForSeconds(duration);
        }
    }
}

