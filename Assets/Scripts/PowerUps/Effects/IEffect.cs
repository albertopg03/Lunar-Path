using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
    string Id { get; }
    void Apply(GameObject target);
    void Revert(GameObject target);
}
