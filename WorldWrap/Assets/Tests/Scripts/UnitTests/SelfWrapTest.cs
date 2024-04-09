using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class SelfWrapTest : WorldWrapTest
{
    private GameObject verticalDuck;
    private GameObject diagonalDuck;
    private GameObject horizontalDuck;
    private BoundsTrigger bounds;
    private float newHeight;
    private float[] oldHeights;

    protected override void SetupVariables()
    {
        base.SetupVariables();
        newHeight = 120.0f;
        verticalDuck = FindGameObjectByName("Duck");
        diagonalDuck = FindGameObjectByName("Duck1");
        horizontalDuck = FindGameObjectByName("Duck2");
        player = FindGameObjectByName("Player");
        oldHeights = new float[3];
        oldHeights[0] = verticalDuck.transform.position.y;
        oldHeights[1] = horizontalDuck.transform.position.y;
        oldHeights[2] = diagonalDuck.transform.position.y;
        GameObject boundsObject = FindGameObjectByName("GlobalBounds");
        bounds = boundsObject.GetComponent<BoundsTrigger>();
    }

    [Test, Order(1)]
    public void DucksDoNotHaveSelfWrapToStart()
    {
        SetupVariables();
        Assert.IsTrue(verticalDuck.GetComponent<SelfWrap>() == null);
        Assert.IsTrue(horizontalDuck.GetComponent<SelfWrap>() == null);
        Assert.IsTrue(diagonalDuck.GetComponent<SelfWrap>() == null);
    }

    [UnityTest, Order(2)]
    public IEnumerator DucksGetSelfWrapAfterLeavingBoundsTrigger()
    {
        verticalDuck.transform.Translate(new Vector3(0, newHeight, 0));
        horizontalDuck.transform.Translate(new Vector3(0, newHeight, 0));
        diagonalDuck.transform.Translate(new Vector3(0, newHeight, 0));
        yield return new WaitForSeconds(0.1f);
        Assert.IsTrue(verticalDuck.GetComponent<SelfWrap>() != null);
        Assert.IsTrue(horizontalDuck.GetComponent<SelfWrap>() != null);
        Assert.IsTrue(diagonalDuck.GetComponent<SelfWrap>() != null);
    }

    [UnityTest, Order(3)]
    public IEnumerator DucksStayInBoundsAfterTime()
    {
        yield return new WaitForSeconds(2.0f);
        Assert.IsTrue(bounds.InsideBounds(verticalDuck.transform.position.x, verticalDuck.transform.position.z));
        Assert.IsTrue(bounds.InsideBounds(horizontalDuck.transform.position.x, horizontalDuck.transform.position.z));
        Assert.IsTrue(bounds.InsideBounds(diagonalDuck.transform.position.x, diagonalDuck.transform.position.z));
    }

    [UnityTest, Order(4)]
    public IEnumerator DucksMaintainRelativePositionAfterWrapWest()
    {
        horizontalDuck.GetComponent<Rigidbody>().velocity = Vector3.zero;
        horizontalDuck.transform.position = new Vector3(0, horizontalDuck.transform.position.y, 0);
        Vector3 oldPosition = horizontalDuck.transform.position;
        player.transform.Translate(new Vector3(-30,0,0));
        yield return new WaitForSeconds(0.05f);
        Assert.IsTrue(Vector3sAreEqual(horizontalDuck.transform.position, oldPosition + new Vector3(30,0,0)));
    }

    [UnityTest, Order(5)]
    public IEnumerator DucksMaintainRelativePositionAfterWrapWestAgain()
    {
        Vector3 oldPosition = horizontalDuck.transform.position;
        player.transform.Translate(new Vector3(-30,0,0));
        yield return new WaitForSeconds(0.05f);
        Assert.IsTrue(Vector3sAreEqual(horizontalDuck.transform.position, oldPosition + new Vector3(-60,0,0)));
    }

    [UnityTest, Order(6)]
    public IEnumerator DucksMaintainRelativePositionAfterWrapNorth()
    {
        Vector3 oldPosition = horizontalDuck.transform.position;
        player.transform.Translate(new Vector3(-30,0,30));
        yield return new WaitForSeconds(0.05f);
        Assert.IsTrue(Vector3sAreEqual(horizontalDuck.transform.position, oldPosition + new Vector3(30,0,-30)));
    }

    [UnityTest, Order(7)]
    public IEnumerator DucksMaintainRelativePositionAfterWrapNorthAgain()
    {
        Vector3 oldPosition = horizontalDuck.transform.position;
        player.transform.Translate(new Vector3(0,0,30));
        yield return new WaitForSeconds(0.05f);
        Assert.IsTrue(Vector3sAreEqual(horizontalDuck.transform.position, oldPosition + new Vector3(0,0,60)));
    }

    [UnityTest, Order(8)]
    public IEnumerator DucksLosesSelfWrapAfterReenteringBoundsTrigger()
    {
        verticalDuck.transform.Translate(new Vector3(0, -newHeight, 0));
        horizontalDuck.transform.Translate(new Vector3(0, -newHeight, 0));
        diagonalDuck.transform.Translate(new Vector3(0, -newHeight, 0));
        yield return new WaitForSeconds(0.1f);
        Assert.IsTrue(verticalDuck.GetComponent<SelfWrap>() == null);
        Assert.IsTrue(horizontalDuck.GetComponent<SelfWrap>() == null);
        Assert.IsTrue(diagonalDuck.GetComponent<SelfWrap>() == null);
    }

    [UnityTest, Order(9)]
    public IEnumerator DucksStayInBoundsAfterReturn()
    {
        yield return new WaitForSeconds(2.0f);
        Assert.IsTrue(bounds.InsideBounds(verticalDuck.transform.position.x, verticalDuck.transform.position.z));
        Assert.IsTrue(bounds.InsideBounds(horizontalDuck.transform.position.x, horizontalDuck.transform.position.z));
        Assert.IsTrue(bounds.InsideBounds(diagonalDuck.transform.position.x, diagonalDuck.transform.position.z));
    }
}
