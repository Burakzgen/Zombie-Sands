using System.Collections;
using UnityEngine;

public class EnemyAppearance : MonoBehaviour
{
    Animator _myAnimator;
    private string[] _randomWalkAnimation = new string[4] { "Normal_Walk", "Dumb_Walk_1", "Dumb_Walk_2", "Dumb_Walk_3" }; // Duruma gore cesitlilik olabilir.

    [Header("HEAD & BODY CONTROLS")]
    [SerializeField] private GameObject[] _heads;
    [SerializeField] private GameObject[] _bodies;
    int _randomBodyNumber;
    int _randomHeadNumber;

    [Header("DISSOLVE CONTROLS")]
    private Material _dissolveBodyMaterial;
    private Material _dissolveHeadMaterial;
    [SerializeField] private float DissolveRate = 0.0125f;
    [SerializeField] private float RefleshRate = 0.025f;
    public Animator EnemyAnimator
    {
        get { return _myAnimator; }
    }

    private void Start()
    {
        _myAnimator = GetComponent<Animator>();

        _randomHeadNumber = Random.Range(0, _heads.Length);
        _randomBodyNumber = Random.Range(0, _bodies.Length);

        //SetRandomWalkAnimation();

        RandomPlayer();

        _dissolveHeadMaterial = _heads[_randomHeadNumber].GetComponent<MeshRenderer>().material;
        _dissolveBodyMaterial = _bodies[_randomBodyNumber].GetComponent<SkinnedMeshRenderer>().material;
    }

    // Animator uzerinden caigiriliacak (Rastgele animasyonlar gelinsin istenirse bunu aktif edebilriiz )
    public void SetRandomWalkAnimation()
    {
        int index = Random.Range(0, _randomWalkAnimation.Length);
        _myAnimator.Play(_randomWalkAnimation[index]);
    }
    public void DissolveEffect()
    {
        StartCoroutine(DissolveCoroutine());
    }

    private void RandomPlayer()
    {
        for (int i = 0; i < _heads.Length; i++)
        {
            _heads[i].SetActive(false);
        }

        for (int i = 0; i < _bodies.Length; i++)
        {
            _bodies[i].SetActive(false);
        }
        _heads[_randomHeadNumber].SetActive(true);
        _bodies[_randomBodyNumber].SetActive(true);
    }
    private IEnumerator DissolveCoroutine()
    {
        float counter = 0;
        while (_dissolveBodyMaterial.GetFloat("_DissolveAmount") < 1)
        {
            counter += DissolveRate;
            _dissolveBodyMaterial.SetFloat("_DissolveAmount", counter);
            _dissolveHeadMaterial.SetFloat("_DissolveAmount", counter);
            yield return new WaitForSeconds(RefleshRate);
        }


    }
}
