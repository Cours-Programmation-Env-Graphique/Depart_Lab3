using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;

    // Awake et non Start : Player interroge GameInput dès le premier Update
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
    }

    // L'Input System alloue des ressources natives à libérer explicitement
    private void OnDestroy()
    {
        _playerInputActions.Player.Disable();
        _playerInputActions.Dispose();
    }

    /// <summary>
    /// Direction voulue par le joueur, normalisée.
    /// </summary>
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }

    /// <summary>
    /// Indique si le bouton de saut vient d'être enfoncé, pendant cette image seulement.
    /// </summary>
    public bool WasJumpPressedThisFrame()
    {
        // Seul l'instant de l'appui compte : maintenir le bouton ne relance pas de saut
        return _playerInputActions.Player.Jump.WasPressedThisFrame();
    }
}
