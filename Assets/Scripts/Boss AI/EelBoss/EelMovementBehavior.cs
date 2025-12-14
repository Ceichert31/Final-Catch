using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelperMethods;

public class EelMovementBehavior : MonoBehaviour, IBossWalkBehavior
{
    [SerializeField] float speed = 10f;
    Transform bossTransform => transform.parent;
    [SerializeField] Transform headSpot;

    [SerializeField] float coilDistance = 20f;
    [SerializeField] float coilSpeed = 100f;

    public void MoveBehavior()
    {
        //if the boss is close enough it needs to coil around the player, otherwise it is going to just go towards the player
        if(Util.DistanceNoY(GameManager.Instance.Player.transform.position, headSpot.position) < coilDistance)
        {
            CoilBehavior();
            return;
        }

        bossTransform.position += bossTransform.forward * (speed * Time.deltaTime);
    }

    public void TeleportBehavior()
    {

    }

    /// <summary>
    /// Eel will coil the player between attacks if close enough
    /// </summary>
    void CoilBehavior()
    {
        bossTransform.RotateAround(GameManager.Instance.Player.transform.position, Vector3.up, coilSpeed * Time.deltaTime);
    }
}
