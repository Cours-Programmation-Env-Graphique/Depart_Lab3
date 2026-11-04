using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SlidingGate : MonoBehaviour
{
    [Header("Ouverture")]
    [Tooltip("Profondeur dont la grille s'enfonce dans le sol une fois ouverte, en unités")]
    [SerializeField] private float _openDepth = 4.6f;
    [Tooltip("Vitesse de descente de la grille, en unités par seconde")]
    [SerializeField] private float _openSpeed = 2f;

    private Rigidbody _rb;
    private Vector3 _openPosition;
    private bool _isOpen;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _openPosition = _rb.position + Vector3.down * _openDepth;
    }

    private void FixedUpdate()
    {
        if (!_isOpen)
        {
            return;
        }

        // MoveTowards attend une distance : ici, une vitesse multipliée par le pas de la physique
        Vector3 next = Vector3.MoveTowards(_rb.position, _openPosition, _openSpeed * Time.fixedDeltaTime);
        _rb.MovePosition(next);
    }

    /// <summary>
    /// Ouvre la grille : elle descend dans le sol et libère le passage.
    /// </summary>
    public void Open()
    {
        _isOpen = true;
    }
}
