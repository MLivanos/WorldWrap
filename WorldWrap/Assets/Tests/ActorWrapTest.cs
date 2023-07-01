using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class ActorWrapTest
{
    [SerializeField] private GameObject player;
    [SerializeField] private UnitTestActor actor;
    [SerializeField] private GameObject[] balls;
    [SerializeField] private GameObject[] ducks;
    [SerializeField] private GameObject redBlock;
    [SerializeField] private GameObject purpleBlock;
    [SerializeField] private GameObject blueBlock;
    [SerializeField] private GameObject greenBlock;
    [SerializeField] private GameObject whiteBlock;
    [SerializeField] private GameObject grayBlock;
    [SerializeField] private GameObject orangeBlock;
    [SerializeField] private GameObject yellowBlock;
    [SerializeField] private GameObject cyanBlock;

    private void SetupVariables()
    {
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

    private GameObject FindGameObjectByName(string objectName)
    {
        GameObject[] gameObjectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject objectInScene in gameObjectsInScene)
        {
            if (objectInScene.name == objectName)
            {
                return objectInScene;
            }
        }
        return null;
    }

    private GameObject FindChildByName(GameObject parent, string childName)
    {
        foreach(Transform childTransform in parent.transform)
        {
            if (childTransform.gameObject.name == childName)
            {
                return childTransform.gameObject;
            }
        }
        return null;
    }

    private Vector2 GetXZPosition(GameObject objectInQuestion)
    {
        return new Vector2(objectInQuestion.transform.localPosition.x, objectInQuestion.transform.localPosition.z);
    }

    private bool PlayerInBounds()
    {
        Vector3 playerPosition = player.transform.position;
        return Math.Abs(playerPosition.x) < 24.0f && Math.Abs(playerPosition.y) < 24.0f;
    }

    private Vector3[] MoveUpIntoNextBlock()
    {
        float displacement = 13.0f;
        Vector3[] actions = new Vector3[4];
        actions[0] = new Vector3(displacement, 0, 0);
        actions[1] = new Vector3(0, 0, 2*displacement);
        actions[2] = new Vector3(-displacement, 0, 0);
        actions[3] = new Vector3(0, 0, 2*displacement);
        return actions;
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("UnitTestScene", LoadSceneMode.Single);
    }

    private WaitForSeconds MoveActor(Vector3 velocity, float timeToWalk=1.0f)
    {
        actor.MoveInDirection(velocity);
        return new WaitForSeconds(timeToWalk);
    }

    [UnityTest, Order(1)]
    public IEnumerator MovingIntoAWrapTriggerDoesNotWrapWorld()
    {
        LoadScene();
        yield return new WaitForSeconds(3.0f);
        SetupVariables();
        yield return MoveActor(new Vector3(0, 0, 10));
        Assert.AreEqual(GetXZPosition(redBlock), Vector2.zero);
        Assert.AreEqual(GetXZPosition(greenBlock).normalized, Vector2.left);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(2)]
    public IEnumerator MovingOutOfAWrapTriggerDoesNotWrapWorld()
    {
        yield return MoveActor(new Vector3(0, 0, 10));
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
        yield return MoveActor(new Vector3(-15, 0, 0));
        Assert.AreEqual(GetXZPosition(redBlock), Vector2.zero);
        Assert.AreEqual(GetXZPosition(greenBlock).normalized, Vector2.left);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(5)]
    public IEnumerator MovingOutOfTheSecondWrapTriggerWrapsWorldLeft()
    {
        yield return MoveActor(new Vector3(0, 0, -10));
        Assert.AreEqual(GetXZPosition(greenBlock), Vector2.zero);
        Assert.AreEqual(GetXZPosition(redBlock).normalized, Vector2.right);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(6)]
    public IEnumerator MovingBackToRedBlockFromGreenWrapsWorldRight()
    {
        yield return MoveActor(new Vector3(0, 0, 10));
        yield return MoveActor(new Vector3(15, 0, 0));
        yield return MoveActor(new Vector3(0, 0, 10));
        Assert.AreEqual(GetXZPosition(redBlock), Vector2.zero);
        Assert.AreEqual(GetXZPosition(greenBlock).normalized, Vector2.left);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(7)]
    public IEnumerator MovingToPurpleBlockFromRedWrapsWorldDown()
    {
        Vector3[] directions = MoveUpIntoNextBlock();
        foreach(Vector3 direction in directions)
        {
            yield return MoveActor(direction);
        }
        Assert.AreEqual(GetXZPosition(redBlock).normalized, Vector2.down);
        Assert.AreEqual(GetXZPosition(purpleBlock), Vector2.zero);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(8)]
    public IEnumerator MovingToBlueBlockFromPurpleWrapsWorldDown()
    {
        Vector3[] directions = MoveUpIntoNextBlock();
        foreach(Vector3 direction in directions)
        {
            yield return MoveActor(direction);
        }
        Assert.AreEqual(GetXZPosition(purpleBlock).normalized, Vector2.down);
        Assert.AreEqual(GetXZPosition(blueBlock), Vector2.zero);
        Assert.IsTrue(PlayerInBounds());
    }

    [UnityTest, Order(9)]
    public IEnumerator MovingToRedBlockFromBlueWrapsWorldDown()
    {
        Vector3[] directions = MoveUpIntoNextBlock();
        foreach(Vector3 direction in directions)
        {
            yield return MoveActor(direction);
        }
        Assert.AreEqual(GetXZPosition(blueBlock).normalized, Vector2.down);
        Assert.AreEqual(GetXZPosition(redBlock), Vector2.zero);
        Assert.IsTrue(PlayerInBounds());
    }
}
