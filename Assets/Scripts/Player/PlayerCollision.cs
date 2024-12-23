using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    [SerializeField] private ParticleSystem particles;

    // Acciones de colisión con objeto
    public event Action<GameObject> CollisionActionObject; // evento pasándole el objeto tocado
    public event Action CollisionAction; // evento para avisar simplemente de una colisión

    // Acciones de colisión con checks
    public event Action CollisionCheckAction;

    // Acciones de colisión con los Power Ups
    public event Action<GameObject> CollisionPowerUpTarget;
    public event Action<GameObject> CollisionPowerUpObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // lógica al tocar un obstáculo
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
        // Establecer la posición actual del sistema de partículas
        particles.transform.position = transform.position;
        particles.gameObject.transform.SetParent(null);

        // Emitir partículas manualmente en la posición actual sin detener el sistema
        particles.Emit(10); // Cambia el número según cuántas partículas deseas emitir
    }

}
