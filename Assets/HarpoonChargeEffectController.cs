using UnityEngine;

public class HarpoonChargeEffectController : MonoBehaviour
{
    [SerializeField] Material fillMaterial;
    
    [SerializeField]
    private Gradient fillGradient;

    private void Start()
    {
        fillMaterial.SetFloat("_FillAmount", 0);
        fillMaterial.SetColor("_FillColor", Color.black);
    }

    /// <summary>
    /// Updates visual charge amount and color
    /// </summary>
    /// <param name="charge">The current charge amount</param>
    /// <param name="maxCharge">The max charge amount till full charge</param>
    public void UpdateCharge(float charge, float maxCharge)
    {
        fillMaterial.SetFloat("_FillAmount", charge / maxCharge);
        fillMaterial.SetColor("_FillColor", fillGradient.Evaluate(charge / maxCharge));
    }

    public void ResetCharge()
    {
        fillMaterial.SetFloat("_FillAmount", 0);
        fillMaterial.SetColor("_FillColor", Color.black);
    }
}
