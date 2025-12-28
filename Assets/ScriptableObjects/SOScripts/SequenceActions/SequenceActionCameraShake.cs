using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sequencer Actions/Camera Shake")]
public class SequenceActionCameraShake : SequencerAction
{
    [SerializeField] private Vector2EventChannel shake_EventChannel;

    [SerializeField] private Vector2Event shakeDurationAndIntensity;

    public override IEnumerator StartSequence(Sequencer ctx)
    {
        shake_EventChannel.CallEvent(shakeDurationAndIntensity);

        yield return null;
    }
}
