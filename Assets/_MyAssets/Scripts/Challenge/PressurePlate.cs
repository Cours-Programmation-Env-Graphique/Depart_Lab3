using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Mécanisme")]
    [Tooltip("Caisse qu'il faut pousser sur la plaque")]
    [SerializeField] private Collider _expectedCrate;
    [Tooltip("Grille qui s'ouvre quand la caisse arrive sur la plaque")]
    [SerializeField] private SlidingGate _gate;

    private bool _isActivated;

    private void OnTriggerEnter(Collider other)
    {
        // Seule la caisse prévue ouvre la grille : le joueur sur la plaque ne suffit pas
        if (_isActivated || other != _expectedCrate)
        {
            return;
        }

        _isActivated = true;
        _gate.Open();
    }
}
