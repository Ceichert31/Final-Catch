using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossPosture : MonoBehaviour
{
    //[SerializeField] FloatEventChannel maxPosture_EventChannel;
    //[SerializeField] FloatEventChannel posture_EventChannel;
    [SerializeField] VoidEventChannel stagger_EventChannel;

    [SerializeField] float maxPosture = 100;

    private FloatEvent currentPosture;

    // Start is called before the first frame update
    void Start()
    {
        currentPosture.FloatValue = 0f;

        //Calls the method on the maxPosture float event listener to update the posture bar to be equal to the max posture
        //maxPosture_EventChannel.CallEvent(new(maxPosture));
    }

    // Update is called once per frame
    public void UpdatePosture(float damage)
    {
        
        //Adds the posture from our currentPosture FloatEvent
        currentPosture.FloatValue += damage;

        //If the posture is broken, call the method on the stagger event channel
        if (currentPosture.FloatValue >= maxPosture)
        {
            currentPosture.FloatValue = maxPosture;
            stagger_EventChannel.CallEvent(new());
        }

        //Calls the method on the Float Event Listener for the posture to be updated for the UI
        //posture_EventChannel.CallEvent(currentPosture);
        //Debug.Log(currentPosture.FloatValue);
    }

    public void ResetPosture()
    {
        currentPosture.FloatValue = 0;
        //posture_EventChannel.CallEvent(currentPosture);
    }

    public void CallResetPosture(VoidEvent ctx)
    {
        ResetPosture();
    }

    public void CallUpdatePosture(FloatEvent ctx)
    {
        UpdatePosture(ctx.FloatValue);
    }

    /// <summary>
    /// Coroutine that is a timer
    /// This Timer is used by the stagger state in order to determine if the boss is still stagered
    /// If the timmer is up it will break out of the this coroutine
    /// </summary>
    /// <returns></returns>
    
}
