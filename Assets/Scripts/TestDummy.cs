using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummy : MonoBehaviour, IDamageable
{
    public void DealDamage(float damage)
    {
        Debug.Log($"{gameObject.name} took {damage * DamageMultiplier} damage!");
    }

    [SerializeField]
    private float damageMultiplier = 1f;
    
    public float DamageMultiplier => damageMultiplier;
}
