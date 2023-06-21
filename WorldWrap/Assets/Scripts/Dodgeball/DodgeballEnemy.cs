using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DodgeballEnemy : DodgeballActor
{
    [SerializeField] private float seekRadius;
    [SerializeField] private float searchForBallTime;
    [SerializeField] private Transform lureObject;
    private NavMeshAgent navMeshAgent;
    private int dodgeballLayer;
    private bool isSearchingForBall;
    private bool isGrabbingBall;
    private bool isMovingTowardsBall;
    private bool isNearEnemy;
    private bool enemyHasBall;

    private void Start()
    {
        health = 2;
        navMeshAgent = GetComponent<NavMeshAgent>();
        dodgeballLayer = LayerMask.NameToLayer("Dodgeballs");;
        isSearchingForBall = true;
        isNearEnemy = false;
        enemyHasBall = false;
        isGrabbingBall = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown("q"))
        {
            Debug.Log(IsBallInRange());
        }
        if(Input.GetKeyDown("s"))
        {
            SearchForBall();
        }
        if(Input.GetKeyDown("p"))
        {
            Debug.Log(IsPlayerInRange());
        }
        navMeshAgent.destination = lureObject.position;
    }

    private void SearchForBall()
    {
        
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
