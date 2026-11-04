public readonly struct LevelResult
{
    public int LevelIndex { get; }
    public float Duration { get; }
    public int Penalty { get; }

    /// <summary>
    /// Crée le résultat d'un niveau terminé.
    /// </summary>
    /// <param name="levelIndex">Index du niveau dans la liste des scènes du build.</param>
    /// <param name="duration">Temps passé dans le niveau, en secondes.</param>
    /// <param name="penalty">Pénalité accumulée dans le niveau, en secondes.</param>
    public LevelResult(int levelIndex, float duration, int penalty)
    {
        LevelIndex = levelIndex;
        Duration = duration;
        Penalty = penalty;
    }
}
