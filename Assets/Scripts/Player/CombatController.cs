using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GrappleSurface
{
    normal,
    damageable,
    weakPoint,
}
public class CombatController : MonoBehaviour
{
    [Header("Scriptable Object Reference")]
    [SerializeField] private AudioPitcherSO spearGunFireAudio;

    [Header("Hook Settings")]
    [SerializeField] private float parryDelay = 0.8f;

    [Header("Harpoon Settings")]
    [SerializeField] private float reloadTime = 1.5f;
    [SerializeField] private float fireRange = 50f;

    private HarpoonController harpoonController;

    private bool canParry = true;

    private Animator hookAnimator;

    private AudioSource source;

    private void Awake()
    {
        hookAnimator = transform.GetChild(1).GetComponent<Animator>();

        harpoonController = GetComponentInChildren<HarpoonController>();

        source = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Fires raycast to detect if there is a weakpoint
    /// </summary>
    void FireHarpoon(InputAction.CallbackContext ctx)
    {
        if (!harpoonController._CanFire) return;

        //Play SFX
        spearGunFireAudio.Play(source);

        //Fire physics based harpoon projectile
        harpoonController.FireHarpoon();
    }

    /// <summary>
    /// Plays the parry animation
    /// </summary>
    /// <param name="ctx"></param>
    void Parry(InputAction.CallbackContext ctx) 
    {
        if (harpoonController._Reloading) return;

        //Time gate to prevent queued parries
        if (canParry)
        {
            canParry = false;
            
            hookAnimator.SetTrigger("Parry");

            //Delay
            Invoke(nameof(ParryDelay), parryDelay);
        }
    }   

    void ParryDelay() => canParry = true;

    void StartReload(InputAction.CallbackContext ctx) => harpoonController.Reload();

    /// <summary>
    /// Subscribes functions to the correct controls
    /// </summary>
    /// <param name="ctx"></param>
    public void InitializeControls(InputEvent ctx)
    {
        ctx.Action.Combat.Disable();

        //ctx.Action.Combat.ReelIn.performed += Parry;

        ctx.Action.Combat.Fire.performed += FireHarpoon;

        ctx.Action.Combat.Reload.performed += StartReload;
    }
}
