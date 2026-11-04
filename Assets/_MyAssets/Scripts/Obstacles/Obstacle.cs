using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Même délai pour tous les obstacles du jeu : défini une seule fois, ici
    private const float ReactivationDelay = 3f;

    [Header("Apparence")]
    [Tooltip("Matériau appliqué à l'obstacle une fois touché")]
    [SerializeField] private Material _hitMaterial;

    [Header("Pénalité")]
    [Tooltip("Secondes de pénalité quand le joueur touche cet obstacle")]
    [SerializeField] private int _penaltySeconds = 1;

    private Renderer _renderer;
    private Material _originalMaterial;
    private bool _wasHit;
    private float _hitTime;

    private void Awake()
    {
        // Le modèle importé est un enfant du conteneur : son Renderer n'est pas sur l'objet lui-même
        _renderer = GetComponentInChildren<Renderer>();
        _originalMaterial = _renderer.sharedMaterial;
    }

    private void Update()
    {
        if (!_wasHit || Time.time - _hitTime < ReactivationDelay)
        {
            return;
        }

        // Le délai est écoulé : l'obstacle reprend son apparence et peut de nouveau pénaliser
        _wasHit = false;
        _renderer.sharedMaterial = _originalMaterial;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_wasHit || !collision.gameObject.TryGetComponent<Player>(out _))
        {
            return;
        }

        _wasHit = true;
        _hitTime = Time.time;
        _renderer.sharedMaterial = _hitMaterial;
        GameManager.Instance.RegisterHit(_penaltySeconds);
    }
}
