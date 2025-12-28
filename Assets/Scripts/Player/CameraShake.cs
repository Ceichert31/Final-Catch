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

    IEnumerator Shake(Vector2 data)
    {
        Vector3 startPosition = currentMainCamera.transform.localPosition;

        float elapsedTime = 0;
        float duration = data.x;
        float intensity = data.y;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            Vector3 shakeAmount = Random.insideUnitSphere * (shakeCurve.Evaluate(elapsedTime) * intensity);

            shakeAmount = new (shakeAmount.x, shakeAmount.y, 0);

            currentMainCamera.transform.localPosition = startPosition + shakeAmount;
           

            yield return null;
        }

        currentMainCamera.transform.localPosition = startPosition;
    }

    [ContextMenu("TEST")]
    public void Test()
    {
        StartCoroutine(Shake(new(1f, 2f)));
    }

}
