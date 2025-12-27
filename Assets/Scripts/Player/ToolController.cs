using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class ToolController : MonoBehaviour
{
    private GameObject fishingRod;

    private GameObject harpoonGun;

    private Animator fishRodAnimator;

    //[SerializeField] private Animator xrayAnimator;

    //private StencilController stencilController;

    InputEvent mainAction;

    private void Start()
    {
        fishingRod = transform.GetChild(0).gameObject;

        harpoonGun = transform.GetChild(1).gameObject;

        fishRodAnimator = transform.GetChild(0).GetComponent<Animator>();

        //stencilController = GetComponentInChildren<StencilController>();
    }

    public void EnableFishingPoleModel(VoidEvent ctx)
    {
        fishingRod.SetActive(true);

        fishRodAnimator.SetTrigger("Pickup");
    }

    public void InitControls(InputEvent ctx)
    {
        ctx.Action.Movement.SwitchStencils.performed += DebugSwitchTools;

        mainAction = ctx;
    }

    //Debug
    bool temp;
    void DebugSwitchTools(InputAction.CallbackContext ctx)
    {
        temp = !temp;
        SwitchTools(new(temp));
    }

    public void SwitchTools(BoolEvent ctx)
    {
        //Combat mode
        if (ctx.Value)
        {
            fishingRod.SetActive(false);
            harpoonGun.SetActive(true);
            //stencilController.UpdateStencil(1, true);
            mainAction.Action.Combat.Enable();
            mainAction.Action.Fishing.Disable();
        }
        //Fishing mode
        else 
        {
            fishingRod.SetActive(true);
            harpoonGun.SetActive(false);
            //stencilController.UpdateStencil(1, false);
            mainAction.Action.Combat.Disable();
            mainAction.Action.Fishing.Enable();
        } 
            
    }
}
