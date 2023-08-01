using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private Transform _startCamPosition;
    [SerializeField] private GameObject _crosshair;
    private void Start()
    {
        StartCamPosition();
    }
    private void StartCamPosition()
    {
        _camera.DOFieldOfView(60, 1.5f);
        _camera.DOFieldOfView(15, 1.5f);
        _camera.transform.DOMove(_targetPosition.position, 2.0f)
            .OnComplete(StartGame);
        //_camera.transform.DORotateQuaternion(_targeRotation, 2.0f)
    }
    public void GameOverCamPosition()
    {
        _camera.gameObject.SetActive(true);
        _camera.transform.DOMove(_startCamPosition.position, 2.0f)
            .OnComplete(GameOver);
    }
    private void StartGame()
    {
        _camera.gameObject.SetActive(false);
        GameManager.IsGameActive = true;
        _crosshair.gameObject.SetActive(true);
    }
    private void GameOver()
    {
        _crosshair.gameObject.SetActive(false);
        GameManager.IsGameActive = false;
    }
}
