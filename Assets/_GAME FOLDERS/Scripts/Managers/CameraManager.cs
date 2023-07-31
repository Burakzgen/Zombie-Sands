using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _targetPosition;
    private void Start()
    {
        _camera.transform.DOMove(_targetPosition.position, 2.0f).OnComplete(() => _camera.gameObject.SetActive(false));
        //_camera.transform.DORotateQuaternion(_targeRotation, 2.0f)
    }
}
