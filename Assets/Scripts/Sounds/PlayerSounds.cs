using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MelenitasDev.SoundsGood;

public class PlayerSounds : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth subjectPlayerHealth;
    [SerializeField] private PlayerCollision subjectPlayerCollision;

    private Sound damageSound;
    private Sound checkSound;

    private void OnEnable()
    {
        subjectPlayerHealth.OnDamage += PlayDamageSound;
        subjectPlayerCollision.CollisionCheckAction += PlayCheckTouchedSound;
    }

    private void OnDisable()
    {
        subjectPlayerHealth.OnDamage -= PlayDamageSound;
        subjectPlayerCollision.CollisionCheckAction -= PlayCheckTouchedSound;
    }

    private void Start()
    {
        damageSound = new Sound(SFX.damage);
        damageSound.SetVolume(0.5f).SetRandomPitch().SetRandomClip(true).SetPosition(transform.position);

        checkSound = new Sound(SFX.checktouched);
        checkSound.SetVolume(0.5f).SetRandomPitch().SetRandomClip(true).SetPosition(transform.position);
    }

    private void PlayDamageSound()
    {
        damageSound.Play();
    }

    private void PlayCheckTouchedSound()
    {
        checkSound.Play();
    }
}
