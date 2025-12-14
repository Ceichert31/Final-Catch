using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SlitherBehavior();

public class EelMovement : MonoBehaviour
{
    private SlitherBehavior slitherBehavior;

    [SerializeField] Transform[] eelBones;
    [SerializeField] Vector3[] previousPositions = new Vector3[15];
    [SerializeField] Transform ORB;

    [SerializeField] float boneLength;
    [SerializeField] float followTime;

    [Header("Idle Anim Stats")]
    [SerializeField] float amplitude = 2.0f;
    [SerializeField] float freqency = 1.0f;

    private int interatable;

    private void Start()
    {
        previousPositions = new Vector3[eelBones.Length];
        for (int i = 1; i < eelBones.Length; i++)
        {
            previousPositions[i] = eelBones[i].position;
        }
    }

    private void Update()
    {
        UnParent();
        ORBRotation();
    }

    //Nesisary Unparent to get desired behavior
    void UnParent()
    {
        for(interatable = 1; interatable < eelBones.Length; interatable++)
        {
            eelBones[interatable].position = previousPositions[interatable];

            slitherBehavior();


            previousPositions[interatable] = eelBones[interatable].position;
        }
    }

    //Chain Slither for normal snake like movement
    void ChainSlither()
    {
        if (Vector3.Distance(eelBones[interatable].position, eelBones[interatable - 1].position) > boneLength)
        {
            eelBones[interatable].position = Vector3.Lerp(eelBones[interatable].position, eelBones[interatable - 1].position, Time.deltaTime * followTime);

            AllignRotations();
        }
    }

    //AllignmentSlither for alligning the body
    void AllignmentSlither()
    {
        Vector3 targetPosition = eelBones[interatable - 1].position - ORB.forward * boneLength;
        eelBones[interatable].position = Vector3.Lerp(eelBones[interatable].position, targetPosition, Time.deltaTime * followTime);

        AllignRotations();
    }

    void AllignRotations()
    {
        //Allign Rotations
        Vector3 boneDirection = (eelBones[interatable].position - eelBones[interatable - 1].position).normalized;

        Quaternion targetDirection = Quaternion.LookRotation(eelBones[interatable].forward, boneDirection);
        eelBones[interatable].rotation = Quaternion.Slerp(eelBones[interatable].rotation, targetDirection, Time.deltaTime * followTime);
    }

    void ORBRotation()
    {
        //float sinWave = Mathf.Sin(freqency * Time.time) * amplitude;

        //ORB.eulerAngles = Vector3.up * sinWave;

        /*float sinWave = Mathf.Sin(freqency * Time.time + Mathf.PerlinNoise(Time.time * 0.1f, 0) * 0.5f) * amplitude;
        ORB.localRotation = Quaternion.Euler(0, sinWave, 0);*/
    }


    public void SetSlither(string slither)
    {
        slitherBehavior = null;

        switch(slither.ToLower())
        {
            case "chain":
                slitherBehavior += ChainSlither;
                    break;
            case "allign":
                slitherBehavior += AllignmentSlither;
                break;
            default:
                Debug.LogError("What u mean u do not want chain or allign slither");
                break;
        }
    }
}
