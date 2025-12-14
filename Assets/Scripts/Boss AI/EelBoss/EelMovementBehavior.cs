using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelMovementBehavior : MonoBehaviour, IBossWalkBehavior
{
    [SerializeField] float speed = 10f;
    Transform bossTransform => transform.parent;

    public void MoveBehavior()
    {
        bossTransform.position += bossTransform.forward * (speed * Time.deltaTime);
    }

    public void TeleportBehavior()
    {

    }
}
