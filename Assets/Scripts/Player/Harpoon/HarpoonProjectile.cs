using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HarpoonProjectile : MonoBehaviour
{
    [SerializeField] private float projectileDamage;

    private float DamageModifier => GameManager.Instance.PlayerDamage;

    [SerializeField] private float projectileSpeed = 20f;

    [SerializeField] private float projectileLifetime = 6f;

    [SerializeField] private int damageLayer = 8;
    [SerializeField] private int weakPointLayer = 9;

    [Tooltip("How long until the harpoon destroys itself after hitting boss")]
    [SerializeField] private float destroyTime = 1.5f;

    [SerializeField] private AudioPitcherSO hitAudio;
    
    private Vector3 targetDirection;

    private float currentTime;

    private Rigidbody rb;

    private AudioSource source;

    private ParticleSystem bloodSpray;

    private bool cannotDestroy;

    public void Init(float speed, float lifetime, Vector3 direction, float damage)
    {
        projectileSpeed = speed;
        projectileLifetime = lifetime;
        targetDirection = direction;
        projectileDamage = damage + DamageModifier;
    }

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();

        source = GetComponent<AudioSource>();

        bloodSpray = GetComponentInChildren<ParticleSystem>();

        currentTime = Time.time + projectileLifetime;
    }
    private void FixedUpdate()
    {
        if (cannotDestroy) return;

        rb.velocity = targetDirection * projectileSpeed;
    }
    private void Update()
    {
        if (Time.time > currentTime && !cannotDestroy)
            Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.DealDamage(projectileDamage);
            transform.parent = collision.transform;
        }
        
        if (cannotDestroy) return;

        //If harpoon doesn't hit collide-able layer destroy
        //Otherwise play hit sound
        if (collision.gameObject.layer != damageLayer)
            Destroy(gameObject);
        else
        {
            hitAudio.Play(source);
            bloodSpray.Play();
            Invoke(nameof(Destroy), destroyTime);
        }   
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    //Interface implementation
    public bool IsParried => cannotDestroy;

    public float ProjectileDamage => projectileDamage;

    public void DeleteProjectile()
    {
        rb.velocity = Vector3.zero;
        cannotDestroy = true;
        rb.isKinematic = true;
    }
}