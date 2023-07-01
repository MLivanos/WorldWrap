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

    private void SetupVariables()
    {
        player = FindGameObjectByName("Player");
        actor = player.GetComponent<UnitTestActor>();
        balls = new GameObject[4];
        balls[0] = FindGameObjectByName("Ball0");
        balls[1] = FindGameObjectByName("Ball1");
        balls[2] = FindGameObjectByName("Ball2");
        balls[3] = FindGameObjectByName("Ball3");
    }

    [UnityTest, Order(1)]
    public IEnumerator BallRemainsInSameRelativePositionWhileHolding()
    {
        SetupVariables();
        actor.PickUp(balls[0]);
        yield return MoveActor(new Vector3(0, 0, 8));
        Assert.AreEqual(balls[0].transform.localPosition, actor.heldObjectPosition);
    }
}
