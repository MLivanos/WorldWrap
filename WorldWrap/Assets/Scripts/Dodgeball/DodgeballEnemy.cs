using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballEnemy : DodgeballActor
{
    [SerializeField] private float seekRadius;
    [SerializeField] private float searchForBallTime;
    private int dodgeballLayer;
    private bool isSearchingForBall;
    private bool isGrabbingBall;
    private bool isMovingTowardsBall;
    private bool isNearEnemy;
    private bool enemyHasBall;

    private void Start()
    {
        health = 2;
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
    }

    private bool IsBallInRange()
    {
        LayerMask dodgeballMask = 1 << dodgeballLayer;
        Collider[] dodgeballsInRange = Physics.OverlapSphere(transform.position, seekRadius, dodgeballMask);
        foreach (Collider ball in dodgeballsInRange)
        {
            if (IsBallVisible(ball.gameObject.transform.position - transform.position))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsBallVisible(Vector3 angleToBall)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, angleToBall, out hit, seekRadius))
        {
            return hit.collider.gameObject.layer == dodgeballLayer;
        }
        Debug.DrawRay(transform.position, angleToBall * seekRadius, Color.white, 10.0f);
        return false;
    }
}
