using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class NavMeshAgentTest : WorldWrapTest
{
    private GameObject redBlock;
    private GameObject bounds;
    private GameObject navMeshAgentObject;
    private NavMeshAgent navMeshAgent;
    private GameObject navMeshLure;
    private Vector3 westPosition;
    private Vector3 northPosition;
    private Vector3 wrapWestDestination;
    private Vector3 wrapNorthDestination;
    private Vector3 goEastPosition;
    private Vector3 goSouthPosition;

    protected override void SetupVariables()
    {
        base.SetupVariables();
        navMeshAgentObject = FindGameObjectByName("NavMeshAgent");
        navMeshAgent = navMeshAgentObject.GetComponent<NavMeshAgent>();
        navMeshLure = FindGameObjectByName("NavMeshLure");
        bounds = FindGameObjectByName("GlobalBounds");
        redBlock = FindChildByName(bounds, "RedBlock");
        westPosition = new Vector3(-40, 1, -30);
        northPosition = new Vector3(-30, 1, -40);
        wrapWestDestination = new Vector3(30, 1, -30);
        wrapNorthDestination = new Vector3(-30, 1, 30);
        goEastPosition = new Vector3(0, 1, -30);
        goSouthPosition = new Vector3(-30, 1, 0);
    }

    private bool AgentInBounds()
    {
        return bounds.GetComponent<BoundsTrigger>().InsideBounds(navMeshAgentObject.transform.position.x, navMeshAgentObject.transform.position.z);
    }

    [UnityTest, Order(1)]
    public IEnumerator NavMeshAgentChosesShortestPathNorth()
    {
        SetupVariables();
        player.transform.position = redBlock.transform.position;
        yield return new WaitForSeconds(0.05f);
        navMeshAgent.Warp(westPosition);
        yield return new WaitForSeconds(0.05f);
        navMeshAgent.SetDestination(wrapWestDestination);
        yield return new WaitForSeconds(2.0f);
        Assert.IsTrue(Vector3sAreEqual(navMeshAgentObject.transform.position, wrapWestDestination, 10.0f));
    }

    [Test, Order(2)]
    public void NavMeshStaysInBounds()
    {
        Assert.IsTrue(AgentInBounds());
    }

    [UnityTest, Order(3)]
    public IEnumerator NavMeshAgentChosesShortestPathWest()
    {
        navMeshAgent.Warp(northPosition);
        yield return new WaitForSeconds(0.05f);
        navMeshAgent.SetDestination(wrapNorthDestination);
        yield return new WaitForSeconds(2.0f);
        Assert.IsTrue(Vector3sAreEqual(navMeshAgentObject.transform.position, wrapNorthDestination, 10.0f));
    }

    [UnityTest, Order(4)]
    public IEnumerator NavMeshAgentChosesShortestPathEast()
    {
        navMeshAgent.Warp(westPosition);
        yield return new WaitForSeconds(0.05f);
        navMeshAgent.SetDestination(goEastPosition);
        yield return new WaitForSeconds(2.0f);
        Assert.IsTrue(Vector3sAreEqual(navMeshAgentObject.transform.position, goEastPosition, 10.0f));
    }

    [UnityTest, Order(5)]
    public IEnumerator NavMeshAgentChosesShortestPathSouth()
    {
        navMeshAgent.Warp(northPosition);
        yield return new WaitForSeconds(0.05f);
        navMeshAgent.SetDestination(goSouthPosition);
        yield return new WaitForSeconds(2.0f);
        Assert.IsTrue(Vector3sAreEqual(navMeshAgentObject.transform.position, goSouthPosition, 10.0f));
    }
}
