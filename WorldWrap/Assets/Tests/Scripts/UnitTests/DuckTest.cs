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
    private GameObject redBlock;
    private GameObject whiteBlock;
    private GameObject yellowBlock;

    protected override void SetupVariables()
    {
        base.SetupVariables();
        verticalDuck = FindGameObjectByName("Duck");
        diagonalDuck = FindGameObjectByName("Duck1");
        horizontalDuck = FindGameObjectByName("Duck2");
        GameObject bounds = FindGameObjectByName("GlobalBounds");
        redBlock = FindChildByName(bounds, "RedBlock");
        whiteBlock = FindChildByName(bounds, "WhiteBlock");
        yellowBlock = FindChildByName(bounds, "YellowBlock");
    }

    private bool DuckIsOnDiagonal(GameObject duck)
    {
        return IsResident(duck, redBlock) || IsResident(duck, whiteBlock) || IsResident(duck, yellowBlock);
    }

    private bool IsResident(GameObject duck, GameObject block)
    {
        BlockTrigger blockTrigger = block.GetComponentInChildren<BlockTrigger>();
        foreach(GameObject resident in blockTrigger.getResidents())
        {
            if(resident == duck)
            {
                return true;
            }
        }
        return false;
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
        Assert.IsTrue(DuckIsOnDiagonal(diagonalDuck));
    }
}
