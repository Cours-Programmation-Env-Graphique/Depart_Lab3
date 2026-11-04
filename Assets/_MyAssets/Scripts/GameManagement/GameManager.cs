using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private readonly List<LevelResult> _results = new List<LevelResult>();
    private int _penaltyTime;
    private bool _isTimerRunning;
    private float _levelStartTime;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Un GameManager existe déjà : il vient d'un niveau précédent
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("Atteignez l'objectif en touchant le moins d'obstacles.");
    }

    /// <summary>
    /// Démarre le chronomètre du niveau au premier mouvement du joueur ; les appels suivants sont ignorés.
    /// </summary>
    public void StartLevelTimer()
    {
        // Le temps passé immobile au début du niveau ne doit pas compter
        if (_isTimerRunning)
        {
            return;
        }

        _isTimerRunning = true;
        _levelStartTime = Time.time;
    }

    /// <summary>
    /// Ajoute la pénalité de l'obstacle touché au temps du joueur.
    /// </summary>
    /// <param name="penaltySeconds">Pénalité de l'obstacle, en secondes.</param>
    public void RegisterHit(int penaltySeconds)
    {
        _penaltyTime += penaltySeconds;
        Debug.Log($"Pénalité : {_penaltyTime} s");
    }

    /// <summary>
    /// Enregistre le résultat du niveau, puis passe au suivant ou au bilan.
    /// </summary>
    public void CompleteLevel()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        float duration = Time.time - _levelStartTime;
        _results.Add(new LevelResult(levelIndex, duration, _penaltyTime));
        _penaltyTime = 0;

        // Le chronomètre est réarmé : il repartira au premier mouvement du niveau suivant
        _isTimerRunning = false;

        int nextIndex = levelIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            ShowSummary();
        }
    }

    private void ShowSummary()
    {
        float total = 0f;
        foreach (LevelResult result in _results)
        {
            float score = result.Duration + result.Penalty;
            total += score;
            Debug.Log($"Niveau {result.LevelIndex + 1} : {result.Duration:F2} s, "
                + $"pénalité {result.Penalty} s, résultat {score:F2} s");
        }

        Debug.Log($"Résultat total : {total:F2} s");

        // La partie est finie : le joueur disparaît, le reste du monde continue
        Player player = FindAnyObjectByType<Player>();
        if (player != null)
        {
            Destroy(player.gameObject);
        }
    }
}
