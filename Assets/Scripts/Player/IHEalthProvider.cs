using System;

public interface IHEalthProvider
{
    int CurrentLives { get; }
    event Action<int> OnLivesChanged;
}
