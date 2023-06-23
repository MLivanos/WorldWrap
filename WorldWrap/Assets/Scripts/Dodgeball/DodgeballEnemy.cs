using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DodgeballEnemy : DodgeballActor
{
    [SerializeField] private float seekRadius;
    [SerializeField] private float pickupRadius;
    [SerializeField] private float searchForBallTime;
    [SerializeField] private float minThrowRadius;
    [SerializeField] private float maxThrowRadius;
    private float distanceToThrow;
    private GameObject lureObject;
    private GameObject ballOfInterest;
    private Transform playerTransform;
    private BoundsTrigger bounds;
    private NavMeshAgent navMeshAgent;
    private enum EnemyBehaviorState{
        Fleeing,
        SearchingForBall,
        MovingTowardsBall,
        HuntingForPlayer,
    };
    private EnemyBehaviorState currentState;
    private int dodgeballLayer;
    private bool isActivelySearching;
    private float searchTimer;

    private void Start()
    {
        SetupNavMesh();
        SetupStateMachine();
    }

    private void Update()
    {
        CheckIfHunted();
        switch (currentState)
        {
            case EnemyBehaviorState.Fleeing:
                Flee();
                break;
            case EnemyBehaviorState.SearchingForBall:
                SearchForBall();
                break;
            case EnemyBehaviorState.MovingTowardsBall:
                MoveTowardsBall();
                break;
            case EnemyBehaviorState.HuntingForPlayer:
                HuntForPlayer();
                break;
            default:
                break;
        }
    }

    private void CheckIfHunted()
    {
        if (isHoldingObject)
        {
            currentState = EnemyBehaviorState.HuntingForPlayer;
        }
        if (playerTransform.childCount > 1 && Vector3.Distance(playerTransform.position, transform.position) < seekRadius)
        {
            currentState = EnemyBehaviorState.Fleeing;
        }
    }

    private void Flee()
    {
        CheckForBall();
        MoveToRandomPoint(playerTransform.position.x, playerTransform.position.z);
    }

    private void SearchForBall()
    {
        CheckForBall();
        MoveToRandomPoint();
    }

    private void MoveToRandomPoint(float? limitX = null, float? limitZ = null)
    {   
        if (isActivelySearching)
        {
            float distanceToPOI = Vector3.Distance(transform.position, lureObject.transform.position);
            searchTimer += Time.deltaTime;
            if (distanceToPOI < 0.1f || searchTimer >= searchForBallTime)
            {
                isActivelySearching = false;
            }
        }
        else
        {
            searchTimer = 0.0f;
            isActivelySearching = true;
            lureObject.transform.position = getRandomPointOnNavMesh(limitX, limitZ);
        }
        navMeshAgent.destination = lureObject.transform.position;
    }

    private void CheckForBall()
    {
        if (IsBallInRange())
        {
            if (!isHoldingObject)
            {
                currentState = EnemyBehaviorState.MovingTowardsBall;
            }
            return;
        }
    }

    private Vector3 getRandomPointOnNavMesh(float? threatX = null, float? threatZ = null)
    {
        Vector2 xBounds = bounds.getXBounds();
        Vector2 zBounds = bounds.getZBounds();
        if (threatX != null && threatZ != null)
        {
            limitBoundsToThreat(xBounds, threatX ?? 0, 0);
            limitBoundsToThreat(zBounds, threatZ ?? 0, 2);
        }
        Vector3 randomPoint = new Vector3(UnityEngine.Random.Range(xBounds.x, xBounds.y), 0.0f, UnityEngine.Random.Range(zBounds.x, zBounds.y));
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, 10.5f, 1);
        return hit.position;
    }

    private void limitBoundsToThreat(Vector2 bounds, float threat, int axis)
    {
        if (transform.position[axis] > threat)
        {
            bounds.x = transform.position.x;
        }
        else
        {
            bounds.y = transform.position.x;
        }
    }

    private bool IsPlayerInRange()
    {
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, seekRadius);
        foreach (Collider collider in objectsInRange)
        {
            if (collider.gameObject.tag == "Player" && IsObjectVisible(collider.gameObject))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsBallInRange()
    {
        LayerMask dodgeballMask = 1 << dodgeballLayer;
        Collider[] dodgeballsInRange = Physics.OverlapSphere(transform.position, seekRadius, dodgeballMask);
        foreach (Collider ball in dodgeballsInRange)
        {
            if (IsObjectVisible(ball.gameObject))
            {
                ballOfInterest = ball.gameObject;
                lureObject.transform.position = ball.gameObject.transform.position;
                return true;
            }
        }
        return false;
    }

    private bool IsObjectVisible(GameObject objectInQuestion)
    {
        RaycastHit hit;
        Vector3 angleToObject = objectInQuestion.transform.position - transform.position;
        if (Physics.Raycast(transform.position, angleToObject, out hit, seekRadius))
        {
            return hit.collider.gameObject == objectInQuestion;
        }
        return false;
    }

    private void MoveTowardsBall()
    {
        Vector2 ballXZPosition = new Vector2(ballOfInterest.transform.position.x, ballOfInterest.transform.position.z);
        Vector2 myXZPosition = new Vector2(transform.position.x, transform.position.z);
        if (ballOfInterest.transform.parent != null)
        {
            currentState = EnemyBehaviorState.SearchingForBall;
        }
        if (Vector2.Distance(ballXZPosition, myXZPosition) <= pickupRadius)
        {
            PickupObject(ballOfInterest);
            currentState = EnemyBehaviorState.HuntingForPlayer;
            distanceToThrow = Random.Range(minThrowRadius, maxThrowRadius);
        }
        navMeshAgent.destination = ballOfInterest.transform.position;
    }

    private void HuntForPlayer()
    {
        navMeshAgent.destination = playerTransform.position;
        if (Vector3.Distance(playerTransform.position, transform.position) < distanceToThrow)
        {
            ThrowObject();
            currentState = EnemyBehaviorState.SearchingForBall;
        }
    }

    private void SetupNavMesh()
    {
        searchTimer = 0.0f;
        currentState = EnemyBehaviorState.SearchingForBall;
        isActivelySearching = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        dodgeballLayer = LayerMask.NameToLayer("Dodgeballs");
    }

    private void SetupStateMachine()
    {
        lureObject = new GameObject("LureObject");
        Collider lureCollider = lureObject.AddComponent<BoxCollider>();
        lureCollider.isTrigger = true;
        playerTransform = GameObject.Find("Player").transform;
        bounds = GameObject.Find("GlobalBounds").GetComponent<BoundsTrigger>();
    }
}
