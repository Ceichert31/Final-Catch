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
    [SerializeField]
    private AudioPitcherSO spearGunFireAudio;
    
    [SerializeField]
    private HarpoonChargeEffectController chargeEffectController;

    [Header("Harpoon Settings")]
    [SerializeField]
    private float reloadTime = 1.5f;

    [SerializeField]
    private float fireRange = 50f;
    
    [SerializeField]
    private float chargeMeter;

    [SerializeField] private float maxCharge = 2.5f;

    private HarpoonController _harpoonController;

    private AudioSource _source;

    private bool _isCharging;

    private Coroutine _instance = null;

    private void Awake()
    {
        _harpoonController = GetComponentInChildren<HarpoonController>();

        _source = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Start charging attack or fire attack if button isn't held down
    /// </summary>
    void FireHarpoon(InputAction.CallbackContext ctx)
    {
        if (!_harpoonController._CanFire)
            return;

        //Play SFX
        spearGunFireAudio.Play(_source);

        //Start charging harpoon
        if (_instance != null) return;
        
        _isCharging = true;
        _instance = StartCoroutine(ChargeHarpoon());
    }

    /// <summary>
    /// Fire attack after charge is released
    /// </summary>
    /// <param name="ctx"></param>
    private void ReleaseHarpoon(InputAction.CallbackContext ctx) { _isCharging = false; }

    /// <summary>
    /// Adds charge meter and fires when released
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChargeHarpoon()
    {
        chargeMeter = 0;
        while (_isCharging)
        {
            chargeMeter += Time.deltaTime;
            
            chargeEffectController.UpdateCharge(chargeMeter, maxCharge);
            
            yield return null;
        }
        
        //fire after charging
        //Maybe pass through charge amount here
        _harpoonController.FireHarpoon();
        chargeEffectController.ResetCharge();
        _instance = null;
    }

    void StartReload(InputAction.CallbackContext ctx) => _harpoonController.Reload();

    /// <summary>
    /// Subscribes functions to the correct controls
    /// </summary>
    /// <param name="ctx"></param>
    public void InitializeControls(InputEvent ctx)
    {
        ctx.Action.Combat.Disable();

        ctx.Action.Combat.Fire.performed += FireHarpoon;
        ctx.Action.Combat.Fire.canceled += ReleaseHarpoon;

        ctx.Action.Combat.Reload.performed += StartReload;
    }
}
