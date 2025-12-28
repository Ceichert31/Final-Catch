using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Screen Shake Settings")]
    [SerializeField] private AnimationCurve shakeCurve;

    private Camera currentMainCamera;

    private void Awake()
    {
        currentMainCamera = Camera.main;
    }

    public void SetCurrentMainCamera(CameraEvent ctx)
    {
        //Set new current camera
        currentMainCamera = ctx.Value;
    }

    public void StartShaking(Vector2Event ctx)
    {
        currentMainCamera.transform.DOShakePosition(ctx.Value.x, ctx.Value.y);
    }
}
