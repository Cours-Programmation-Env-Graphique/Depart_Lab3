using System.Collections.Generic;
using UnityEngine;

public class TrapZone : MonoBehaviour
{
    [Header("Pièges")]
    [Tooltip("Pièges qui tombent quand le joueur entre dans la zone")]
    [SerializeField] private List<Rigidbody> _traps = new List<Rigidbody>();
    [Tooltip("Impulsion vers le bas au déclenchement, en N·s")]
    [SerializeField] private float _dropImpulse = 10f;

    private bool _isTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (_isTriggered || !other.TryGetComponent<Player>(out _))
        {
            return;
        }

        _isTriggered = true;
        foreach (Rigidbody trap in _traps)
        {
            trap.isKinematic = false;
            trap.AddForce(Vector3.down * _dropImpulse, ForceMode.Impulse);
        }
    }
}
