using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRandomBody : MonoBehaviour
{
    [SerializeField] private GameObject[] _heads;
    [SerializeField] private GameObject[] _bodies;

    private void Awake()
    {
        RandomPlayer();
    }
    private void RandomPlayer()
    {
        int randomHeadNumber=Random.Range(0, _heads.Length);
        int randomBodyNumber=Random.Range(0, _bodies.Length);

        for (int i = 0; i < _heads.Length; i++)
        {
            _heads[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _bodies.Length; i++)
        {
            _bodies[i].gameObject.SetActive(false);
        }
        _heads[randomHeadNumber].gameObject.SetActive(true);
        _bodies[randomBodyNumber].gameObject.SetActive(true);
    }
}
