using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class ActorWrapTest : WorldWrapTest
{
    private GameObject redBlock;
    private GameObject purpleBlock;
    private GameObject blueBlock;
    private GameObject greenBlock;
    private GameObject whiteBlock;
    private GameObject grayBlock;
    private GameObject orangeBlock;
    private GameObject yellowBlock;
    private GameObject cyanBlock;

    protected override void SetupVariables()
    {
        base.SetupVariables();
        player = FindGameObjectByName("Player");
        actor = player.GetComponent<UnitTestActor>();
        GameObject bounds = FindGameObjectByName("GlobalBounds");
        redBlock = FindChildByName(bounds, "RedBlock");
        purpleBlock = FindChildByName(bounds, "PurpleBlock");
        blueBlock = FindChildByName(bounds, "BlueBlock");
        greenBlock = FindChildByName(bounds, "GreenBlock");
        whiteBlock = FindChildByName(bounds, "WhiteBlock");
        grayBlock = FindChildByName(bounds, "GrayBlock");
        orangeBlock = FindChildByName(bounds, "OrangeBlock");
        yellowBlock = FindChildByName(bounds, "YellowBlock");
        cyanBlock = FindChildByName(bounds, "CyanBlock");
    }

    private bool PlayerInBounds()
    {
        Vector3 playerPosition = player.transform.position;
        return Math.Abs(playerPosition.x) < 24.0f && Math.Abs(playerPosition.y) < 24.0f;
    }

    [UnityTest, Order(1)]
    public IEnumerator MovingIntoAWrapTriggerDoesNotWrapWorld()
    {
        LoadScene();
        yield return new WaitForSeconds(3.0f);
        SetupVariables();
        yield return MoveActor(new Vector3(0, 0, 8));
        Assert.AreEqual(GetXZPosition(redBlock), Vector2.zero);
        Assert.AreEqual(GetXZPosition(greenBlock).normalized, Vector2.left);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(2)]
    public IEnumerator MovingOutOfAWrapTriggerDoesNotWrapWorld()
    {
        yield return MoveActor(new Vector3(0, 0, 8));
        Assert.AreEqual(GetXZPosition(redBlock), Vector2.zero);
        Assert.AreEqual(GetXZPosition(greenBlock).normalized, Vector2.left);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(3)]
    public IEnumerator MovingBackIntoAWrapTriggerDoesNotWrapWorld()
    {
        yield return MoveActor(new Vector3(0, 0, -7));
        Assert.AreEqual(GetXZPosition(redBlock), Vector2.zero);
        Assert.AreEqual(GetXZPosition(greenBlock).normalized, Vector2.left);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(4)]
    public IEnumerator MovingToAWrapTriggerFromAnotherDoesNotWrapWorld()
    {
        yield return MoveActor(new Vector3(-9, 0, 0));
        Assert.AreEqual(GetXZPosition(redBlock), Vector2.zero);
        Assert.AreEqual(GetXZPosition(greenBlock).normalized, Vector2.left);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(5)]
    public IEnumerator MovingOutOfTheSecondWrapTriggerWrapsWorldLeft()
    {
        yield return MoveActor(new Vector3(0, 0, -8));
        Assert.AreEqual(GetXZPosition(greenBlock), Vector2.zero);
        Assert.AreEqual(GetXZPosition(redBlock).normalized, Vector2.right);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(6)]
    public IEnumerator MovingBackToRedBlockFromGreenWrapsWorldRight()
    {
        yield return MoveActor(new Vector3(0, 0, 8));
        yield return MoveActor(new Vector3(20, 0, 0));
        Assert.AreEqual(GetXZPosition(redBlock), Vector2.zero);
        Assert.AreEqual(GetXZPosition(greenBlock).normalized, Vector2.left);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(7)]
    public IEnumerator MovingToPurpleBlockFromRedWrapsWorldDown()
    {
        yield return MoveActor(new Vector3(0, 0, 35));
        Assert.AreEqual(GetXZPosition(redBlock).normalized, Vector2.down);
        Assert.AreEqual(GetXZPosition(purpleBlock), Vector2.zero);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(8)]
    public IEnumerator MovingToBlueBlockFromPurpleWrapsWorldDown()
    {
        yield return MoveActor(new Vector3(0, 0, 35));
        Assert.AreEqual(GetXZPosition(purpleBlock).normalized, Vector2.down);
        Assert.AreEqual(GetXZPosition(blueBlock), Vector2.zero);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(9)]
    public IEnumerator MovingToRedBlockFromBlueWrapsWorldDown()
    {
        yield return MoveActor(new Vector3(0, 0, 35));
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Assert.AreEqual(GetXZPosition(blueBlock).normalized, Vector2.down);
        Assert.AreEqual(GetXZPosition(redBlock), Vector2.zero);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(10)]
    public IEnumerator MovingToGreenBlockFromRedWrapsWorldRight()
    {
        actor.TeleportTo(Vector3.zero);
        yield return MoveActor(new Vector3(-35, 0, 0));
        Assert.AreEqual(GetXZPosition(redBlock).normalized, Vector2.right);
        Assert.AreEqual(GetXZPosition(greenBlock), Vector2.zero);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(11)]
    public IEnumerator MovingToCyanBlockFromGreenWrapsWorldRight()
    {
        actor.TeleportTo(Vector3.zero);
        yield return MoveActor(new Vector3(-35, 0, 0));
        Assert.AreEqual(GetXZPosition(greenBlock).normalized, Vector2.right);
        Assert.AreEqual(GetXZPosition(cyanBlock), Vector2.zero);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(12)]
    public IEnumerator MovingToRedBlockFromCyanWrapsWorldRight()
    {
        actor.TeleportTo(Vector3.zero);
        yield return MoveActor(new Vector3(-35, 0, 0));
        Assert.AreEqual(GetXZPosition(cyanBlock).normalized, Vector2.right);
        Assert.AreEqual(GetXZPosition(redBlock), Vector2.zero);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(13)]
    public IEnumerator MovingToBlueBlockFromRedWrapsWorldUp()
    {
        actor.TeleportTo(Vector3.zero);
        yield return MoveActor(new Vector3(0, 0, -35));
        Assert.AreEqual(GetXZPosition(redBlock).normalized, Vector2.up);
        Assert.AreEqual(GetXZPosition(blueBlock), Vector2.zero);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(14)]
    public IEnumerator MovingToPurpleBlockFromBlueWrapsWorldUp()
    {
        actor.TeleportTo(Vector3.zero);
        yield return MoveActor(new Vector3(0, 0, -35));
        Assert.AreEqual(GetXZPosition(blueBlock).normalized, Vector2.up);
        Assert.AreEqual(GetXZPosition(purpleBlock), Vector2.zero);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(15)]
    public IEnumerator MovingToRedBlockFromPurpleWrapsWorldUp()
    {
        actor.TeleportTo(Vector3.zero);
        yield return MoveActor(new Vector3(0, 0, -35));
        Assert.AreEqual(GetXZPosition(purpleBlock).normalized, Vector2.up);
        Assert.AreEqual(GetXZPosition(redBlock), Vector2.zero);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(16)]
    public IEnumerator MovingToCyanBlockFromRedWrapsWorldLeft()
    {
        actor.TeleportTo(Vector3.zero);
        yield return MoveActor(new Vector3(35, 0, 0));
        Assert.AreEqual(GetXZPosition(redBlock).normalized, Vector2.left);
        Assert.AreEqual(GetXZPosition(cyanBlock), Vector2.zero);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(17)]
    public IEnumerator MovingToGreenBlockFromCyanWrapsWorldLeft()
    {
        actor.TeleportTo(Vector3.zero);
        yield return MoveActor(new Vector3(35, 0, 0));
        Assert.AreEqual(GetXZPosition(cyanBlock).normalized, Vector2.left);
        Assert.AreEqual(GetXZPosition(greenBlock), Vector2.zero);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(18)]
    public IEnumerator MovingToRedBlockFromGreenWrapsWorldLeft()
    {
        actor.TeleportTo(Vector3.zero);
        yield return MoveActor(new Vector3(35, 0, 0));
        Assert.AreEqual(GetXZPosition(greenBlock).normalized, Vector2.left);
        Assert.AreEqual(GetXZPosition(redBlock), Vector2.zero);
        Assert.IsTrue(PlayerInBounds());
    }
}
