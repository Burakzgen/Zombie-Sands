using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _targetPosition;
    private void Start()
    {
        _camera.DOFieldOfView(60, 1.5f);
        _camera.transform.DOMove(_targetPosition.position, 2.0f)
            .OnComplete(StartGame);
        //_camera.transform.DORotateQuaternion(_targeRotation, 2.0f)
    }
    private void StartGame()
    {
        _camera.gameObject.SetActive(false);
        GameManager.IsGameActive = true;
    }
}
