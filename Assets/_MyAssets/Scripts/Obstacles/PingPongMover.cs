using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PingPongMover : MonoBehaviour
{
    [Header("Trajet")]
    [Tooltip("Décalage entre la position de départ et l'autre bout du trajet, en unités")]
    [SerializeField] private Vector3 _offset = new Vector3(6f, 0f, 0f);
    [Tooltip("Vitesse de déplacement le long du trajet, en unités par seconde")]
    [SerializeField] private float _speed = 3f;
    [Tooltip("Décalage dans le temps, en secondes, pour que les obstacles d'une même série ne bougent pas ensemble")]
    [SerializeField] private float _phaseOffset;

    private Rigidbody _rb;
    private Vector3 _startPosition;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _startPosition = _rb.position;
    }

    private void FixedUpdate()
    {
        float length = _offset.magnitude;
        if (length <= 0f)
        {
            return;
        }

        // PingPong fait aller et revenir la distance parcourue entre 0 et la longueur du trajet
        float travelled = Mathf.PingPong((Time.time + _phaseOffset) * _speed, length);
        _rb.MovePosition(_startPosition + _offset * (travelled / length));
    }
}
