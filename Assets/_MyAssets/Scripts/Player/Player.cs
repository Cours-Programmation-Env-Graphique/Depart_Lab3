using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    // Délai minimal entre deux sauts : le sol est encore détecté pendant le premier pas de la montée
    private const float JumpCooldown = 0.2f;

    [Header("Déplacement")]
    [Tooltip("Vitesse de déplacement, en unités par seconde (mode par la vitesse)")]
    [SerializeField] private float _moveSpeed = 7f;
    [Tooltip("Vitesse de rotation, en degrés par seconde")]
    [SerializeField] private float _rotateSpeed = 720f;
    [Tooltip("Multiplie la gravité appliquée au joueur")]
    [SerializeField] private float _gravityScale = 2.5f;

    [Header("Mode par la force")]
    [Tooltip("Coché : le joueur est poussé par une force et glisse ; décoché : il est déplacé par sa vitesse")]
    [SerializeField] private bool _useForce;
    [Tooltip("Intensité de la poussée, en newtons (mode par la force)")]
    [SerializeField] private float _moveForce = 40f;

    [Header("Saut")]
    [Tooltip("Vitesse verticale donnée au saut, en unités par seconde")]
    [SerializeField] private float _jumpSpeed = 8f;
    [Tooltip("Rayon de la sphère qui détecte le sol sous les pieds, en unités")]
    [SerializeField] private float _groundCheckRadius = 0.3f;
    [Tooltip("Distance sous les pieds à laquelle le sol est encore détecté, en unités")]
    [SerializeField] private float _groundCheckDistance = 0.2f;

    [Header("Références")]
    [Tooltip("Source des entrées du joueur")]
    [SerializeField] private GameInput _gameInput;

    private Rigidbody _rb;
    private int _groundMask;
    private bool _jumpRequested;
    private float _lastJumpTime = -1f;
    private bool _hasMoved;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        // Le joueur ne doit jamais se détecter lui-même comme un sol
        _groundMask = ~LayerMask.GetMask("Player");
    }

    private void Update()
    {
        // Un appui bref peut tomber entre deux pas de physique : on le lit ici et on le garde en mémoire
        if (_gameInput.WasJumpPressedThisFrame())
        {
            _jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // Défi D2 : le chronomètre du niveau part au premier mouvement, pas au chargement
        if (!_hasMoved && moveDir != Vector3.zero)
        {
            _hasMoved = true;
            GameManager.Instance.StartLevelTimer();
        }

        if (_useForce)
        {
            // AddForce applique déjà la force pendant tout le pas : aucun Δt ici
            _rb.AddForce(moveDir * _moveForce);
        }
        else
        {
            // Une vitesse est déjà en unités par seconde : aucun Δt ici
            Vector3 velocity = moveDir * _moveSpeed;
            velocity.y = _rb.linearVelocity.y;   // la gravité garde l'axe Y
            _rb.linearVelocity = velocity;
        }

        // Unity applique déjà g une fois : on ajoute seulement la différence
        Vector3 extraGravity = Physics.gravity * (_gravityScale - 1f);
        _rb.AddForce(extraGravity, ForceMode.Acceleration);

        if (_jumpRequested && Time.time - _lastJumpTime >= JumpCooldown && IsGrounded())
        {
            // On part d'une vitesse verticale nulle : le saut a la même hauteur en montée de rampe qu'à plat
            Vector3 velocity = _rb.linearVelocity;
            velocity.y = 0f;
            _rb.linearVelocity = velocity;
            _rb.AddForce(Vector3.up * _jumpSpeed, ForceMode.VelocityChange);
            _lastJumpTime = Time.time;
        }

        // L'intention de sauter ne vaut que pour ce pas : aucun saut différé en touchant le sol
        _jumpRequested = false;

        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            _rb.MoveRotation(Quaternion.RotateTowards(
                _rb.rotation,
                targetRotation,
                _rotateSpeed * Time.fixedDeltaTime));
        }
    }

    // Une sphère plus étroite que le collider descend sous les pieds : elle touche le sol et les rampes, pas les murs
    private bool IsGrounded()
    {
        Vector3 origin = _rb.position + Vector3.up * (_groundCheckRadius + 0.1f);
        return Physics.SphereCast(
            origin,
            _groundCheckRadius,
            Vector3.down,
            out RaycastHit _,
            0.1f + _groundCheckDistance,
            _groundMask,
            QueryTriggerInteraction.Ignore);
    }

    /// <summary>
    /// Replace le joueur à une position donnée et annule toute sa vitesse.
    /// </summary>
    /// <param name="position">Position où le joueur réapparaît.</param>
    public void Respawn(Vector3 position)
    {
        _rb.position = position;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _jumpRequested = false;
    }
}
