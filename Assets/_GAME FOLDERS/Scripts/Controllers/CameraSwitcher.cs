using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera[] cameras;
    private int currentCameraIndex = 0;
    [SerializeField] private GameObject _camView;

    [SerializeField] private UIController _uiController;
    private void Update()
    {
        if (!GameManager.Instance.IsGameActive)
            return;

        if (Input.GetKeyDown(KeyCode.F) && !_uiController._scope.activeSelf)
        {
            SwitchCamera();
        }
    }
    private void SwitchCamera()
    {
        cameras[currentCameraIndex].gameObject.SetActive(false);

        currentCameraIndex++;

        if (currentCameraIndex >= cameras.Length)
        {
            currentCameraIndex = 0;
        }

        cameras[currentCameraIndex].gameObject.SetActive(true);
        if (currentCameraIndex == 0)
        {
            _camView.SetActive(false);
            _uiController.SetScopeActive(false);
            _uiController.SetCrosshairActive(true);
        }
        else
        {
            _camView.SetActive(true);
            _uiController.SetScopeActive(false);
            _uiController.SetCrosshairActive(false);
        }
    }
}
