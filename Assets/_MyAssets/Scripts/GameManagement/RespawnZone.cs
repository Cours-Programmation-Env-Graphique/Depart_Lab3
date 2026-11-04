using UnityEngine;

public class RespawnZone : MonoBehaviour
{
    [Header("Réapparition")]
    [Tooltip("Point où le joueur est replacé après une chute dans la zone")]
    [SerializeField] private Transform _respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        // Aucune pénalité : la chute coûte déjà du temps au joueur
        if (other.TryGetComponent<Player>(out Player player))
        {
            player.Respawn(_respawnPoint.position);
        }
    }
}
