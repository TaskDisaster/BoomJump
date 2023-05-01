using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolController : AIController
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called on ce per frame
    public override void Update()
    {
        base.Update();
    }

    public override void MakeDecisions()
    {
        switch (currentState)
        {
            case AIState.Idle:
                // Do nothing
                DoIdleState();
                // Check for transition
                ChangeState(AIState.Patrol);
                break;

            case AIState.Patrol:
                // Do work
                DoPatrolState();
                // Check for transitions
                if (CanHear(target))
                {
                    ChangeState(AIState.Scan);
                }
                if (CanSee(target))
                {
                    ChangeState(AIState.Attack);
                }
                break;

            case AIState.Attack:
                // Do work
                DoAttackState();
                // Check for transition
                if (!CanSee(target) || target == null)
                {
                    ChangeState(AIState.Idle);
                }
                break;

            case AIState.Scan:
                // Do work
                DoScanState();
                // Check for transitions
                if (CanSee(target))
                {
                    ChangeState(AIState.Attack);
                }
                if (!CanHear(target))
                {
                    ChangeState(AIState.Idle);
                }
                break;
        }
    }

    protected override void DoAttackState()
    {
        if (!IsDistanceLessThan(target, 5))
        {
            Seek(target);
        }
        Shoot();
    }
}
