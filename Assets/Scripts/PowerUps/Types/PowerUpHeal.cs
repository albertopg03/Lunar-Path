using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHeal : MonoBehaviour, IPowerUp
{
    public void Apply(GameObject target)
    {
        Debug.Log("Aplicar efecto de curaci�n a : " + target.name);
    }
}
