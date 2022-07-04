using UnityEngine;
using UnityEngine.UI;

public class CanvasRotator : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private bool _grabMainCameraOnStart = true;
    [SerializeField] private Vector3 _offsetDirection = Vector3.back;
    [SerializeField] private Vector3 _up = Vector3.up;
    [SerializeField] private Text _scoreUI;

    protected GameObject _parentContainer;
    private Transform _transform;

    public void SetCamera(Camera mainCamera)
    {
        _mainCamera = mainCamera;
    }

    public void SetText(string text)
    {
        _scoreUI.text = text;
    }

    protected virtual void Awake()
    {
        _transform = transform;
    }

    protected virtual void Update()
    {
        if (_mainCamera == null)
            return;

        _transform.LookAt(_transform.position + _mainCamera.transform.rotation * _offsetDirection, _mainCamera.transform.rotation * _up);
    }
}
