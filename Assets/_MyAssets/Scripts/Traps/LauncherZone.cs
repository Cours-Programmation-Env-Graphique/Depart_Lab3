using System.Collections.Generic;
using UnityEngine;

public class LauncherZone : MonoBehaviour
{
    [Header("Projectiles")]
    [Tooltip("Objets lancés quand le joueur entre dans la zone")]
    [SerializeField] private List<Rigidbody> _projectiles = new List<Rigidbody>();

    [Header("Lancer")]
    [Tooltip("Direction du lancer, dans le repère du monde ; le code la ramène à une longueur de 1")]
    [SerializeField] private Vector3 _launchDirection = Vector3.forward;
    [Tooltip("Impulsion donnée à chaque projectile au déclenchement, en N·s")]
    [SerializeField] private float _launchImpulse = 10f;

    private bool _isTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (_isTriggered || !other.TryGetComponent<Player>(out _))
        {
            return;
        }

        _isTriggered = true;

        // Normalisée une fois : seule l'impulsion règle la force du lancer, quelle que soit la direction saisie
        Vector3 direction = _launchDirection.normalized;
        foreach (Rigidbody projectile in _projectiles)
        {
            projectile.isKinematic = false;
            projectile.AddForce(direction * _launchImpulse, ForceMode.Impulse);
        }
    }
}
