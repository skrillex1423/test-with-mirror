using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _cameraMoveSpeed = 120f;
    [SerializeField] private Camera _camera;
    public float _maxClampAngle = 50f;
    public float _minClampAngle = -30f;
    public float _inputSensitivity = 150f;

    public float _mouseX;
    public float _mouseY;
    private float _rotY = 0.0f;
    private float _rotX = 0.0f;
    private Transform _target;

    public Camera Camera => _camera;

    private void Awake()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        _rotY = rot.y;
        _rotX = rot.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        _mouseX = 0;
        _mouseY = 0;
    }

    void Update()
    {
        _mouseX = Input.GetAxis("Mouse X");
        _mouseY = Input.GetAxis("Mouse Y");

        _rotY += _mouseX * _inputSensitivity * Time.deltaTime;
        _rotX += -_mouseY * _inputSensitivity * Time.deltaTime;

        _rotX = Mathf.Clamp(_rotX, _minClampAngle, _maxClampAngle);

        Quaternion localRotation = Quaternion.Euler(_rotX, _rotY, 0.0f);
        transform.rotation = localRotation;
    }

    void LateUpdate()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        float step = _cameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _target.position, step);
    }

    public void SetupTarget(Transform target)
    {
        _target = target;
    }
}
