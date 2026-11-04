using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StickOnImpact : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _isStuck;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Le joueur ne fige pas le glaçon : il continue sa chute jusqu'à la glace
        if (_isStuck || collision.gameObject.TryGetComponent<Player>(out _))
        {
            return;
        }

        // Planté dans la glace, le glaçon devient un obstacle fixe que le joueur ne peut plus pousser
        _isStuck = true;
        _rb.isKinematic = true;
    }
}
