using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingBossMoveBehavior : MonoBehaviour, IBossWalkBehavior
{
    private Vector3 Target => GameManager.Instance.Player.transform.position;

    [SerializeField] private float bossMoveSpeed = 20f;
    
    public void MoveBehavior()
    {
        Vector3 targetDir = (Target - transform.position).normalized;
        
        transform.position = Vector3.MoveTowards(transform.position, targetDir, bossMoveSpeed * Time.deltaTime);
    }

    public void TeleportBehavior()
    {
        throw new System.NotImplementedException();
    }
}
