using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelAnimationEvents : AnimationEvents
{
    [SerializeField] EelMovement eelMovement;

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

}
