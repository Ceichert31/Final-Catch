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

    [Header("Harpoon Settings")]
    [SerializeField] private float reloadTime = 1.5f;
    [SerializeField] private float fireRange = 50f;

    private HarpoonController harpoonController;

    private AudioSource source;

    private void Awake()
    {
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

    void StartReload(InputAction.CallbackContext ctx) => harpoonController.Reload();

    /// <summary>
    /// Subscribes functions to the correct controls
    /// </summary>
    /// <param name="ctx"></param>
    public void InitializeControls(InputEvent ctx)
    {
        ctx.Action.Combat.Disable();

        ctx.Action.Combat.Fire.performed += FireHarpoon;

        ctx.Action.Combat.Reload.performed += StartReload;
    }
}
