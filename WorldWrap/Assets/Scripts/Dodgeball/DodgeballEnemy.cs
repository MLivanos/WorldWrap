using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DodgeballEnemy : DodgeballActor
{
    [SerializeField] private float seekRadius;
    [SerializeField] private float searchForBallTime;
    private GameObject lureObject;
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
        health = 2;
        searchTimer = 0.0f;
        currentState = EnemyBehaviorState.SearchingForBall;
        isActivelySearching = false;
        lureObject = new GameObject("LureObject");
        Collider lureCollider = lureObject.AddComponent<BoxCollider>();
        lureCollider.isTrigger = true;
        bounds = GameObject.Find("GlobalBounds").GetComponent<BoundsTrigger>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        dodgeballLayer = LayerMask.NameToLayer("Dodgeballs");;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyBehaviorState.SearchingForBall:
                if (IsBallInRange())
                {
                    currentState = EnemyBehaviorState.MovingTowardsBall;
                    Debug.Log("Ball Found");
                }
                else
                {
                    SearchForBall();
                }
                break;
            default:
                break;
        }
    }

    private void SearchForBall()
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
            lureObject.transform.position = getRandomPointOnNavMesh();
            navMeshAgent.destination = lureObject.transform.position;
        }
    }

    private Vector3 getRandomPointOnNavMesh()
    {
        Vector2 xBounds = bounds.getXBounds();
        Vector2 zBounds = bounds.getZBounds();
        Vector3 randomPoint = new Vector3(UnityEngine.Random.Range(xBounds.x, xBounds.y), 0.0f, UnityEngine.Random.Range(zBounds.x, zBounds.y));
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, 10.5f, 1);
        return hit.position;
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

}
