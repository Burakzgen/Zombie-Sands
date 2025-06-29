using UnityEngine;

public class MouseAimController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    [SerializeField] private Transform _playerBody;

    [SerializeField] private float _xMin, _xMax;
    [SerializeField] private float _yMin, _yMax;

    private float _xRotation = 0f;
    private float _yRotation = 0f;
  
    void Update()
    {
        if (!GameManager.Instance.IsGameActive)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _yMin, _yMax);

        _yRotation += mouseX;
        _yRotation = Mathf.Clamp(_yRotation, _xMin, _xMax);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _playerBody.localRotation = Quaternion.Euler(0f, _yRotation, 0f);
    }
}
