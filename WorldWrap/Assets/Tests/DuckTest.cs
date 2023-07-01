using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class DuckTest : WorldWrapTest
{
    private GameObject verticalDuck;
    private GameObject diagonalDuck;
    private GameObject horizontalDuck;

    private void SetupVariables()
    {
        verticalDuck = FindGameObjectByName("Duck");
        diagonalDuck = FindGameObjectByName("Duck1");
        horizontalDuck = FindGameObjectByName("Duck2");
    }

    [UnityTest, Order(1)]
    public IEnumerator VerticalDuckStaysOnVerticalAxisOverTime()
    {
        SetupVariables();
        float initialX = verticalDuck.transform.position.x;
        yield return new WaitForSeconds(0.4f);
        bool isEqual = Math.Abs(verticalDuck.transform.position.x - initialX) < 0.01f;
        Assert.IsTrue(isEqual);
    }

    [UnityTest, Order(2)]
    public IEnumerator HoriztonalDuckStaysOnHorizontalAxisOverTime()
    {
        float initialZ = horizontalDuck.transform.position.z;
        yield return new WaitForSeconds(0.4f);
        bool isEqual = Math.Abs(horizontalDuck.transform.position.z - initialZ) < 0.01f;
        Assert.IsTrue(isEqual);
    }

    [Test, Order(3)]
    public void DiagonalDuckStaysOnDiagonalAxisOverTime()
    {
        bool isEqual = Math.Abs(diagonalDuck.transform.position.x - diagonalDuck.transform.position.z) < 0.01f;
        if (!isEqual)
        {
            Debug.Log("x: " + diagonalDuck.transform.position.x + ", z: " + diagonalDuck.transform.position.z);
        }
        Assert.IsTrue(isEqual);
    }
}
