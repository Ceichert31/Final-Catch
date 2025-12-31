using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelAnimationEvents : AnimationEvents
{
    [SerializeField] EelMovement eelMovement;

    [SerializeField] EelMovementBehavior eelBehavior;


    private void Start()
    {
        //On awake make the slither type allign
        ChangeSlitherType(1);
    }


    /// <summary>
    /// 0 for chain slither type and 1 for allignment slither type
    /// </summary>
    /// <param name="slitherType"></param>\
    public void ChangeSlitherType(int slitherType)
    {
        string slitherTypeString = slitherType == 0 ? "chain" : "allign";


        eelMovement.SetSlither(slitherTypeString);
    }


    /// <summary>
    /// 6: Charge Player
    /// 7: Deassign Charge Method and trigger stop charging
    /// </summary>
    public override void UpdateBossActiveBehavior(int behavior)
    {
        base.UpdateBossActiveBehavior(behavior);

        switch (behavior)
        {
            case 6:
                eelBehavior.OnStartCharge();
                activeBehavior += eelBehavior.EelChargePlayer;
                break;
            case 7:
                bossAnimator.SetTrigger("StopCharging");
                activeBehavior -= eelBehavior.EelChargePlayer;
                break;
        }
    }
}
