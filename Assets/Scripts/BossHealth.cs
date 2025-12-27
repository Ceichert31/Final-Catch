using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour, IDamageable
{
    [Header("Scriptable Object Reference")]
    [SerializeField] private FloatEventChannel healthUI_EventChannel;
    [SerializeField] private FloatEventChannel maxHealthUI_EventChannel;
    [SerializeField] private VoidEventChannel return_EventChannel;

    [Header("Boss Health Settings")]
    [SerializeField] private int bossMaxHealth = 100;

    [SerializeField] private float weakPointMultiplier = 2f;

    [SerializeField] private GameObject parentObject;

    private FloatEvent currentHealth;

    private FloatEvent damageEvent;

    [Header("Death Variables")]

    [SerializeField] Animator bossAnimator;

    private Sequencer deathSequencer;

    bool isDead;
    
    //The amount of damage hitting just the boss does
    public float DamageMultiplier { get; } = 1f;
    private void Start()
    {
        //Set health to max
        currentHealth.FloatValue = bossMaxHealth;

        //Initialize max health
        maxHealthUI_EventChannel.CallEvent(currentHealth);

        //Start whatever sequence this boss has
        gameObject.GetComponent<Sequencer>().InitializeSequence();

        deathSequencer = transform.GetChild(0).GetComponent<Sequencer>();

        isDead = false;
    }

    /// <summary>
    /// Updates the boss's health with input value
    /// </summary>
    /// <param name="ctx"></param>
    public void UpdateHealth(FloatEvent ctx)
    {
        if (currentHealth.FloatValue <= 0)
        {
            return;
        }
        
        currentHealth.FloatValue -= ctx.FloatValue;

        //Updates health UI
        healthUI_EventChannel.CallEvent(currentHealth);

        //Fish health hits zero
        if(isDead)
        {
            return;
        }

        if (currentHealth.FloatValue <= 0)
        {
            //Triggers A death animation
            isDead = true;
            bossAnimator.SetTrigger("Death");
        }
    }

    /// <summary>
    /// Calls disableBoss from void event channel
    /// </summary>
    /// <param name="ctx"></param>
    public void DisableBoss(VoidEvent ctx)
    {
        DisableBoss();
    }

    /// <summary>
    /// Destroys boss if player dies
    /// </summary>
    /// <param name="ctx"></param>
    void DisableBoss()
    {
        Destroy(parentObject);
    }


    /// <summary>
    /// Death Event Order:
    /// 1. Death animation is triggered whenever the enemy looses all of its health
    /// 2. Death animation calls an animation event that then calls this function
    /// 3. Death Sequencer goes throught its processes
    /// 4. Player is transitioned back to overworld
    /// 5. Boss is disabled
    /// </summary>
    public void ProcedeWithDeath()
    {
        deathSequencer.InitializeSequence();

        //Returns player
        return_EventChannel.CallEvent(new());
    }

    public void DealDamage(float damage)
    {
        damageEvent.FloatValue = damage;
        UpdateHealth(damageEvent);
    }
}


