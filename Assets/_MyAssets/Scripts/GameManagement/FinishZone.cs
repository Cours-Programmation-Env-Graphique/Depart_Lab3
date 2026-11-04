using UnityEngine;

public class FinishZone : MonoBehaviour
{
    private bool _isReached;

    private void OnTriggerEnter(Collider other)
    {
        if (_isReached || !other.TryGetComponent<Player>(out _))
        {
            return;
        }

        _isReached = true;
        GameManager.Instance.CompleteLevel();

        // L'objectif est récupéré : il disparaît de la scène
        gameObject.SetActive(false);
    }
}
