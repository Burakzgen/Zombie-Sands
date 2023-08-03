using UnityEngine;
using DG.Tweening;

public class CameraTransition : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private Transform _startCamPosition;

    [SerializeField] private UIController _uiController;
    private void Start()
    {
        BeginCameraMove();
    }
    private void BeginCameraMove()
    {
        _camera.DOFieldOfView(60, 1.5f);
        _camera.DOFieldOfView(15, 1.5f);
        _camera.transform.DOMove(_targetPosition.position, 2.0f)
            .OnComplete(BeginCameraMoveDelay);
    }
    private void BeginCameraMoveDelay()
    {
        _camera.gameObject.SetActive(false);
        GameManager.Instance.IsGameActive = true;
        GameManager.Instance.HandleStartGame();
        _uiController.SetCrosshairActive(true);
    }
    public void EndGameCameraTransition()
    {
        _camera.gameObject.SetActive(true);
        _uiController.SetScopeActive(false);
        _uiController.SetCrosshairActive(false);
        GameManager.Instance.IsGameActive = false;
        _camera.transform.DOMove(_startCamPosition.position, 2.0f);
    }
}
