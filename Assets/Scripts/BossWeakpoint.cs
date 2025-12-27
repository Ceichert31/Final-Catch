using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakpoint : MonoBehaviour, IDamageable
{
    [Header("Weakpoint Settings")]
    [SerializeField] private float weakpointDamageMultiplier = 1.5f;

    [SerializeField] private FloatEventChannel cameraShakeChannel;

    [SerializeField] private FloatEvent cameraShakeDuration;

    [SerializeField] private Transform harpoonHolder;

    private ParticleSystem bloodSprayParticle;

    private BossHealth health;
    private FloatEvent damageEvent;
    void Awake()
    {
        health = transform.parent.GetComponent<BossHealth>();

        bloodSprayParticle = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    public void DealDamage(float damage)
    {
        damageEvent.FloatValue = damage;
        health.UpdateHealth(damageEvent);
        bloodSprayParticle.Play();
        cameraShakeChannel.CallEvent(cameraShakeDuration);
    }
    public float DamageMultiplier => weakpointDamageMultiplier;
}
