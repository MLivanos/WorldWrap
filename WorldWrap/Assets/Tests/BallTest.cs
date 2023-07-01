using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class BallTest : WorldWrapTest
{
    private GameObject[] balls;
    private Vector3[] originalPositions;

    protected override void SetupVariables()
    {
        base.SetupVariables();
        player = FindGameObjectByName("Player");
        actor = player.GetComponent<UnitTestActor>();
        balls = new GameObject[4];
        originalPositions = new Vector3[4];
        for(int ballNumber = 0; ballNumber < 4; ballNumber++)
        {
            balls[ballNumber] = FindGameObjectByName("Ball" + ballNumber);
            originalPositions[ballNumber] = balls[ballNumber].transform.position;
        }
    }

    private bool Vector3sAreEqual(Vector3 a, Vector3 b, float threshold = 0.01f)
    {
        return Math.Abs(a.x - b.x) < threshold && Math.Abs(a.y - b.y) < threshold && Math.Abs(a.z - b.z) < threshold;
    }

    [UnityTest, Order(1)]
    public IEnumerator BallRemainsInSameRelativePositionWhileHolding()
    {
        SetupVariables();
        actor.TeleportTo(new Vector3(-10.0f, 0.0f, 0.0f));
        actor.PickUp(balls[0]);
        Vector3[] directions = MoveLeftIntoNextBlock();
        foreach(Vector3 direction in directions)
        {
            yield return MoveActor(direction);
        }
        Assert.IsTrue(Vector3sAreEqual(balls[0].transform.localPosition, actor.heldObjectPosition));
    }

    [Test, Order(2)]
    public void OtherBallsRemainInRelativePositionsAfterWrap()
    {
        bool allBallsAreOnShelf = true;
        for(int ballNumber = 0; ballNumber < 3; ballNumber++)
        {
            if (balls[ballNumber + 1].transform.position.y <= 1.0f)
            {
                allBallsAreOnShelf = false;
            }
        }
        Assert.IsTrue(allBallsAreOnShelf);
    }

    [UnityTest, Order(3)]
    public IEnumerator BallRemainsInSameRelativePositionAfterDropped()
    {   
        actor.PlaceDown();
        balls[0].transform.position = balls[0].transform.position + Vector3.forward * 10.0f;
        originalPositions[0] = balls[0].transform.position;
        Vector3[] directions = MoveLeftIntoNextBlock();
        foreach(Vector3 direction in directions)
        {
            yield return MoveActor(direction);
        }
        actor.MoveInDirection(Vector3.zero);
        Assert.AreEqual(GetXZPosition(balls[0]), new Vector2(originalPositions[0].x, originalPositions[0].z));
    }
}
