using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rotator : MonoBehaviour
{
    [Header("Rotation")]
    [Tooltip("Vitesse de rotation autour de Y, en degrés par seconde")]
    [SerializeField] private float _degreesPerSecond = 90f;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float angle = _degreesPerSecond * Time.fixedDeltaTime;
        Quaternion step = Quaternion.Euler(0f, angle, 0f);
        _rb.MoveRotation(_rb.rotation * step);
    }
}
