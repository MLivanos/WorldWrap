using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class SafetyTriggerTest : WorldWrapTest
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
    private float translateDistance;
    private float waitTime;

    protected override void SetupVariables()
    {
        base.SetupVariables();
        translateDistance = 36.0f;
        waitTime = 0.05f;
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
    public IEnumerator TeleportingWestInitiatesWrap()
    {
        LoadScene();
        yield return new WaitForSeconds(3.0f);
        SetupVariables();
        player.transform.position = new Vector3(0,player.transform.position.y,0);
        yield return new WaitForSeconds(waitTime);
        player.transform.Translate(-translateDistance, 0, 0);
        yield return new WaitForSeconds(waitTime);
        Assert.AreEqual(GetXZPosition(greenBlock), Vector2.zero);
        Assert.AreEqual(GetXZPosition(redBlock).normalized, Vector2.right);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(2)]
    public IEnumerator TeleportingEastInitiatesWrap()
    {
        player.transform.Translate(translateDistance, 0, 0);
        yield return new WaitForSeconds(waitTime);
        Assert.AreEqual(GetXZPosition(redBlock), Vector2.zero);
        Assert.AreEqual(GetXZPosition(greenBlock).normalized, Vector2.left);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(3)]
    public IEnumerator TeleportingNorthInitiatesWrap()
    {
        player.transform.Translate(0, 0, translateDistance);
        yield return new WaitForSeconds(waitTime);
        Assert.AreEqual(GetXZPosition(purpleBlock), Vector2.zero);
        Assert.AreEqual(GetXZPosition(redBlock).normalized, Vector2.down);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(4)]
    public IEnumerator TeleportingSouthInitiatesWrap()
    {
        player.transform.Translate(0, 0, -translateDistance);
        yield return new WaitForSeconds(waitTime);
        Assert.AreEqual(GetXZPosition(redBlock), Vector2.zero);
        Assert.AreEqual(GetXZPosition(cyanBlock).normalized, Vector2.right);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(5)]
    public IEnumerator TeleportingNorthWestInitiatesWrap()
    {
        player.transform.Translate(-translateDistance, 0, translateDistance);
        yield return new WaitForSeconds(waitTime);
        Assert.AreEqual(GetXZPosition(orangeBlock), Vector2.zero);
        Assert.AreEqual(GetXZPosition(purpleBlock).normalized, Vector2.right);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(6)]
    public IEnumerator TeleportingSouthEastInitiatesWrap()
    {
        player.transform.Translate(translateDistance, 0, -translateDistance);
        yield return new WaitForSeconds(waitTime);
        Assert.AreEqual(GetXZPosition(redBlock), Vector2.zero);
        Assert.AreEqual(GetXZPosition(cyanBlock).normalized, Vector2.right);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(7)]
    public IEnumerator TeleportingSouthWestInitiatesWrap()
    {
        player.transform.Translate(-translateDistance, 0, -translateDistance);
        yield return new WaitForSeconds(waitTime);
        Assert.AreEqual(GetXZPosition(yellowBlock), Vector2.zero);
        Assert.AreEqual(GetXZPosition(orangeBlock).normalized, Vector2.down);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(8)]
    public IEnumerator TeleportingNorthEastInitiatesWrap()
    {
        player.transform.Translate(translateDistance, 0, translateDistance);
        yield return new WaitForSeconds(waitTime);
        Assert.AreEqual(GetXZPosition(redBlock), Vector2.zero);
        Assert.AreEqual(GetXZPosition(blueBlock).normalized, Vector2.down);
        Assert.IsTrue(PlayerInBounds());
    }

}