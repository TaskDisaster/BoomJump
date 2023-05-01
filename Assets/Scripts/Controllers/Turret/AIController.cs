using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    #region Variables
    public enum AIState { Idle, Guard, Chase, Flee, Patrol, Attack, Scan, BackToPost, };
    public bool isLooping;
    public bool isSeeing = true;
    public bool isHearing = true;
    public AIState currentState;
    public float fleeDistance;
    public List<Transform> waypoints = new List<Transform>();
    public float waypointStopDistance;
    private int currentWaypoint = 0;
    public int hearingDistance;
    public float fieldOfView;
    public float maxViewDistance;
    public GameObject target;
    #endregion

    // Start is called before the first frame update
    public override void Start()
    {
        // Set the current state to Idle
        currentState = AIState.Idle;

        // Add itself to the list of enemies
        GameManager.Instance.ai.Add(this);
    }

    // Update is called once per frame
    public override void Update()
    {
        if (target == null)
        {
            TargetPlayerOne();
        }

        MakeDecisions();
    }

    public void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.ai != null)
            {
                GameManager.Instance.ai.Remove(this);
            }
        }
    }

    // Commands that do everything
    public virtual void MakeDecisions()
    {

    }

    // Changes the states of currentState
    public virtual void ChangeState(AIState newState)
    {
        // Change the current state
        currentState = newState;
    }

    public bool IsDistanceLessThan(GameObject target, float distance)
    {
        if (Vector3.Distance(pawn.transform.position, target.transform.position) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanHear(GameObject target)
    {
        // If isHearing false, return false
        if (isHearing == false)
        {
            return false;
        }
        // Get the target's NoiseMaker
        NoiseMaker noiseMaker = target.GetComponent<NoiseMaker>();

        // If they don't have onem they can't make noise, so return false
        if (noiseMaker == false)
        {
            return false;
        }

        // If they are making 0 noise then they also can't be heard
        if (noiseMaker.volumeDistance <= 0)
        {
            return false;
        }

        // If they are making noise, add the volumeDistance in the nopisemaker to the hearingDistance of this Ai
        float totalDistance = noiseMaker.volumeDistance + hearingDistance;
        // If the distnace between our pawn and target is closer than this...
        if (Vector3.Distance(pawn.transform.position, target.transform.position) <= totalDistance)
        {
            // ... then we can hear the target
            //UnityEngine.Debug.LogWarning("I HEAR YOU");
            return true;
        }
        else
        {
            //UnityEngine.Debug.LogWarning("I CANNOT HEAR YOU");
            return false;
        }
    }

    public bool CanSee(GameObject target)
    {
        // If isSeeing false, return false
        if (isSeeing == false)
        {
            return false;
        }

        // Find the vector from the agent to the target
        Vector3 agentToTargetVector = target.transform.position - transform.position;
        agentToTargetVector.Normalize();
        // Find the angle between the direction our agent is facing (forward in local space) and the vector to the target.
        float angleToTarget = Vector3.Angle(agentToTargetVector, pawn.transform.forward);

        // If that angle is out of our field of view
        if (angleToTarget >= fieldOfView)
        {
            return false;

        }

        // If the target is further than our maxViewDistance
        if (!IsDistanceLessThan(target, maxViewDistance))
        {
            return false;
        }

        // Checking for target collider
        Collider targetCollider = target.GetComponent<Collider>();

        // Raycast to make sure nothing is blocking view
        Ray ray = new Ray(transform.position, agentToTargetVector);
        RaycastHit hitInfo;

        // If the raycast doesn't hit anything return nothing
        if (!Physics.Raycast(ray, out hitInfo, maxViewDistance))
        {
            UnityEngine.Debug.LogWarning("NOTHING TO SEE HERE");
            return false;
        }

        // if our raycast hits nothing, we can't see them
        if (!Physics.Raycast(transform.position, agentToTargetVector, maxViewDistance))
        {
            UnityEngine.Debug.LogWarning("NOTHING NOTHING TO SEE HERE");
            return false;
        }

        // If the target collider is hit, then we are seen
        if (hitInfo.collider == targetCollider)
        {
            // I SEE YOU
            //UnityEngine.Debug.LogWarning("I SEE YOU");
            return true;
        }

        // If I cannot see you, then something is blocking me
        //UnityEngine.Debug.LogWarning("BLOCKED");
        return false;

    }

    public void TargetPlayerOne()
    {
        // If the GameManager exists
        if (GameManager.Instance != null)
        {
            // And there are players in it
            if (GameManager.Instance.playerController1 != null)
            {
                // The target
                target = GameManager.Instance.playerController1.pawn.gameObject;
            }
        }
    }

    public void ResetGun()
    {
        TurretPawn turret = GetComponent<TurretPawn>();
        turret.ResetGun();
    }

    #region States
    protected virtual void DoPatrolState()
    {
        ResetGun();
        Patrol();
    }

    protected virtual void DoScanState()
    {
        Seek(target.transform);
    }

    protected virtual void DoChaseState()
    {
        Seek(target);
    }

    protected virtual void DoIdleState()
    {
        // Do nothing :)
        ResetGun();
    }

    protected virtual void DoAttackState()
    {
        Seek(target);
        Shoot();
    }

    protected virtual void DoFleeState()
    {
        Flee();
    }
    #endregion States

    #region Behavior
    public virtual void Seek(GameObject target)
    {
        // RotateTowards the Function
        pawn.RotateTowards(target.transform.position);
        // Rotate Gun to
        pawn.RotateGunTo(target.transform.position);
        // Move Forward
        pawn.MoveForward();
    }

    public void Seek(Transform targetTransform)
    {
        // Seek the position of our target Transform
        pawn.RotateTowards(targetTransform.position);
        // Move Forward
        pawn.MoveForward();
    }

    public void Seek(Pawn targetPawn)
    {
        // Seek that pawn's transform!
        pawn.RotateTowards(targetPawn.transform.position);
        // Rotate Gun to
        pawn.RotateGunTo(target.transform.position);
        // Move Forward
        pawn.MoveForward();
    }

    public void Seek(Vector3 targetPosition)
    {
        // Rotate towards the position
        pawn.RotateTowards(targetPosition);
        // Rotate Gun to
        pawn.RotateGunTo(target.transform.position);
        // Move Forward
        pawn.MoveForward();
    }

    public void Shoot()
    {
        // Tell the pawn to shoot
        pawn.Shoot();
    }

    public void Flee()
    {
        // Find the Vector to our target
        Vector3 vectorToTarget = target.transform.position - pawn.transform.position;

        // Find the Vector away from our target by multiplying by -1
        Vector3 vectorAwayFromTarget = -vectorToTarget;

        // Find the distance the target is from the pawn
        float targetDistance = Vector3.Distance(target.transform.position, pawn.transform.position);

        // Get the percentage of our FleeDistance
        float percentOfFleeDistance = targetDistance / fleeDistance;

        // Clamp the distance of FleeDistance to between 0 and 1
        percentOfFleeDistance = Mathf.Clamp01(percentOfFleeDistance);

        // Flip the percentage of percentofFleeDistance
        float flippedPercentOfFleeDistance = 1 - percentOfFleeDistance;

        // Find the vector we could travel down in order to flee
        Vector3 fleeVector = vectorAwayFromTarget.normalized * fleeDistance * flippedPercentOfFleeDistance;

        // Seek the point that is "fleeVector" away from our current position
        Seek(pawn.transform.position + fleeVector);

    }

    public void Patrol()
    {
        // IF we have enough waypoints in our list to move to a current waypoint
        if (waypoints.Count > currentWaypoint)
        {
            // Then seek that waypoint
            Seek(waypoints[currentWaypoint]);
            // If we are close enough, then increment to next waypoint
            if (Vector3.Distance(pawn.transform.position, waypoints[currentWaypoint].position) <= waypointStopDistance)
            {
                currentWaypoint++;
            }
        }
        else if (isLooping == true)
        {
            RestartPatrol();
        }
    }

    public void RestartPatrol()
    {
        // Set the index to 0
        currentWaypoint = 0;
    }

    #endregion
}
