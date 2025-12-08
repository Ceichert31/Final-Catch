using HelperMethods;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



[CreateAssetMenu(fileName = "AttackState", menuName = "BossStates/Attack")]
public class AttackState : AIState, IAttackState
{
    [SerializeField] Animator bossAnimator;

    string currentAttack;

    class AttackInfo
    {
        public List<string> attackList = new List<string>();
        public List<string> previousAttackList = new List<string>();
        public string previousAttack;
        public int attackType;
        public int previousAttackType;
        public int storedIndex;
    }

    //previous attack info
    AttackInfo attackInfo = new AttackInfo();

    bool attacking;

    private bool called = false;

    protected override bool Called 
    {
        get { return called; }
        set { called = value;  } 
    }

    public override void InitalizeState(BossAI ctx)
    {
        bossAnimator = bossTransform.GetComponent<Animator>();

        if (ctx.BossInformation.meleeAttacks.Count == 0)
        {
            throw new System.Exception("I has no attacks :(");
        }
        Debug.Log("EnteredAttack");
    }


    public override void EnterState(BossAI ctx)
    {
        if(bossAnimator == null)
        {
            throw new System.Exception("You did not initalize");
        }

        float temp = Util.DistanceNoY(bossTransform.position, Player);
        Debug.Log(temp);

        //If we have no ranged attacks, then just generate a melee attack
        if(ctx.BossInformation.rangedAttacks.Count == 0)
        {
            Debug.Log("You have no ranged attacks");
            attackInfo.attackList = ctx.BossInformation.meleeAttacks;
            attackInfo.attackType = 0;
            GenerateAttack(attackInfo.attackList);
        }
        else
        {
            bool close = Util.DistanceNoY(bossTransform.position, Player) <= ctx.BossInformation.meleeDistance;

            attackInfo.attackType = close ? 0 : 1;

            attackInfo.attackList = close ? ctx.BossInformation.meleeAttacks : ctx.BossInformation.rangedAttacks;

            GenerateAttack(attackInfo.attackList);
        }

        ExecuteAttack();
    }

    public override void ExecuteState(BossAI ctx)
    {
        if(attacking)
        {
            return;
        }

        ctx.SwitchState(States.WalkState);
    }

    public override void ExitState(BossAI ctx)
    {
        attacking = false;
    }

    void GenerateAttack(List<string> attacks)
    {
        int randomAttack = Random.Range(0, attacks.Count);

        attackInfo.storedIndex = randomAttack;

        currentAttack = attacks[randomAttack];
    }

    void ExecuteAttack()
    {
        attacking = true;
        Debug.Log(currentAttack);
        bossAnimator.SetTrigger(currentAttack);

        if(attackInfo.attackList.Count == 0)
        {
            Debug.LogError("bro aint got no attacks");
        }

        ReplaceAndRemoveAttack();
    }

    void ReplaceAndRemoveAttack()
    {
        //Current Bug: it needs to know what list to add it back to(FIXED)

        //THis needs a refactor.

        if (attackInfo.previousAttack == null)
        {
            attackInfo.previousAttack = currentAttack;
            attackInfo.storedIndex = attackInfo.attackList.IndexOf(currentAttack);
            attackInfo.attackList.Remove(currentAttack);
            attackInfo.previousAttackList = attackInfo.attackList;
            attackInfo.previousAttackType = attackInfo.attackType;
            return;
        }

        //Take the current attack out of the list, put the previous
        if (attackInfo.previousAttackType == attackInfo.attackType)
        {
            attackInfo.attackList.Insert(attackInfo.storedIndex, attackInfo.previousAttack);
        }
        else
        {
            attackInfo.previousAttackList.Insert(attackInfo.storedIndex, attackInfo.previousAttack);
        }
        attackInfo.storedIndex = attackInfo.attackList.IndexOf(currentAttack);
        attackInfo.previousAttack = currentAttack;
        attackInfo.attackList.Remove(currentAttack);
        attackInfo.previousAttackList = attackInfo.attackList;
        attackInfo.previousAttackType = attackInfo.attackType;
    }

    public bool Attacking
    {
        get { return attacking; }
        set { attacking = value; }
    }
}
