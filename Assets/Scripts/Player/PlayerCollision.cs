using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    [SerializeField] private ParticleSystem particles;

    // Acciones de colisi�n con objeto
    public event Action<GameObject> CollisionActionObject; // evento pas�ndole el objeto tocado
    public event Action CollisionAction; // evento para avisar simplemente de una colisi�n

    // Acciones de colisi�n con checks
    public event Action CollisionCheckAction;

    // Acciones de colisi�n con los Power Ups
    public event Action<GameObject> CollisionPowerUpTarget;
    public event Action<GameObject> CollisionPowerUpObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // l�gica al tocar un obst�culo
        if (collision.CompareTag("Obstacle"))
        {
            ParticleCollisionEffect();

            if (collision.TryGetComponent(out Obstacle obstacle)){
                CollisionActionObject?.Invoke(collision.gameObject);
                CollisionAction?.Invoke();
            }
        }

        if (collision.CompareTag("Check"))
        {
            CollisionCheckAction?.Invoke();
        }

        if (collision.CompareTag("PowerUp"))
        {
            CollisionPowerUpTarget?.Invoke(gameObject);
            CollisionPowerUpObject?.Invoke(collision.gameObject);
        }
    }

    private void ParticleCollisionEffect()
    {
        // Establecer la posici�n actual del sistema de part�culas
        particles.transform.position = transform.position;
        particles.gameObject.transform.SetParent(null);

        // Emitir part�culas manualmente en la posici�n actual sin detener el sistema
        particles.Emit(10); // Cambia el n�mero seg�n cu�ntas part�culas deseas emitir
    }

}
