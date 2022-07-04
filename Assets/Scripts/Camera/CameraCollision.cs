using UnityEngine;

public class CameraCollision : MonoBehaviour
{

    [SerializeField] private float _minDistance = 0.2f;
    [SerializeField] private float _maxDistance = 2.0f;
    [SerializeField] private float _smooth = 5.0f;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private float _distance;

    private void Awake()
    {
        _direction = transform.localPosition.normalized;
        _distance = transform.localPosition.magnitude;
    }

    private void FixedUpdate()
    {
        Vector3 desiredCameraPos = transform.parent.TransformPoint(_direction * _maxDistance);
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit))
        {
            _distance = Mathf.Clamp(hit.distance * 0.85f, _minDistance, _maxDistance);
        }
        else
        {
            _distance = _maxDistance;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, _direction * _distance, Time.deltaTime * _smooth);
    }
}
