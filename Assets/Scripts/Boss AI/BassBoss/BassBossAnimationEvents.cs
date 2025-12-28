using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BassSounds
{
    Bite,
    Swim,
    Explode
}

[System.Serializable]
public enum BassParticles
{
    Stagger,
    Explode,
}

public class BassBossAnimationEvents : AnimationEvents
{
    private BassBossMoveBehavoir bassBossMoveBehavior;

    [SerializeField] private Vector2EventChannel cameraShakeChannel;

    [Header("Particles")]
    [SerializeField] private ParticleSystem staggerParticle;
    [SerializeField] private ParticleSystem explosionParticle;

    [Header("Audio References")]
    [SerializeField] private AudioPitcherSO swimAudio;
    [SerializeField] private AudioPitcherSO biteAudio;
    [SerializeField] private AudioPitcherSO explodeAudio;

    private IProjectileSpawner projectileSpawner;
    private AudioSource mainSource;

    private void Start()
    {
        bassBossMoveBehavior = (BassBossMoveBehavoir)bossWalkBehavior;

        mainSource = GetComponent<AudioSource>();

        transform.GetChild(0).TryGetComponent(out projectileSpawner);
    }


    /// <summary>
    /// 6: Constantly move toward Player
    /// 7: Deassign Constant movment Method
    /// </summary>
    public override void UpdateBossActiveBehavior(int behavior)
    {
        base.UpdateBossActiveBehavior(behavior);

        switch (behavior)
        {
            case 6:
                activeBehavior += bassBossMoveBehavior.ConstantMovement;
                break;
            case 7:
                activeBehavior -= bassBossMoveBehavior.ConstantMovement;
                break;
        }
    }

    public void PlayParticle(BassParticles particle)
    {
        switch (particle)
        {
            case BassParticles.Stagger:
                staggerParticle.Play();
                break;
            case BassParticles.Explode:
                explosionParticle.Play();
                break;
        }
    }

    public void PlayAudio(BassSounds audio)
    {
        switch (audio)
        {
            case BassSounds.Swim:
                swimAudio.Play(mainSource);
                break;
            case BassSounds.Bite:
                biteAudio.Play(mainSource);
                break;
            case BassSounds.Explode:
                explodeAudio.Play(mainSource);
                break;
        }
    }

    public void ShakeCamera(float duration)
    {
        cameraShakeChannel.CallEvent(new Vector2Event(new(duration, 1f)));
    }
    public void FireProjectiles(int amount)
    {
        projectileSpawner.Spawn(amount);
    }
}
