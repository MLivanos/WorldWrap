using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class WorldWrapTest
{

    protected GameObject player;
    protected UnitTestActor actor;

    protected virtual void SetupVariables()
    {
        player = FindGameObjectByName("Player");
        actor = player.GetComponent<UnitTestActor>();
    }

    protected GameObject FindGameObjectByName(string objectName)
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

    protected GameObject FindChildByName(GameObject parent, string childName)
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

    protected Vector2 GetXZPosition(GameObject objectInQuestion)
    {
        return new Vector2(objectInQuestion.transform.localPosition.x, objectInQuestion.transform.localPosition.z);
    }

    protected void LoadScene(string sceneToLoad = "UnitTestScene")
    {
        if (SceneManager.GetActiveScene().name != sceneToLoad)
        {
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        }
    }

    protected WaitForSeconds MoveActor(Vector3 velocity, float timeToWalk=1.0f, float speedScaleFactor=2.0f)
    {
        actor.MoveInDirection(velocity*speedScaleFactor);
        return new WaitForSeconds(timeToWalk/speedScaleFactor);
    }

    protected Vector3[] MoveUpIntoNextBlock()
    {
        float displacement = 8.5f;
        Vector3[] actions = new Vector3[4];
        actions[0] = new Vector3(displacement, 0, 0);
        actions[1] = new Vector3(0, 0, 2.0f * displacement);
        actions[2] = new Vector3(-displacement, 0, 0);
        actions[3] = new Vector3(0, 0, 2.0f * displacement);
        return actions;
    }

    protected Vector3[] MoveLeftIntoNextBlock()
    {
        float displacement = 8.5f;
        Vector3[] actions = new Vector3[4];
        actions[0] = new Vector3(-2.0f * displacement, 0, 0);
        actions[1] = new Vector3(0, 0, displacement);
        actions[2] = new Vector3(-2.0f * displacement, 0, 0);
        actions[3] = new Vector3(0, 0, -displacement);
        return actions;
    }

    protected Vector3[] MoveRightIntoNextBlock()
    {
        float displacement = 8.5f;
        Vector3[] actions = new Vector3[4];
        actions[0] = new Vector3(2.0f * displacement, 0, 0);
        actions[1] = new Vector3(0, 0, displacement);
        actions[2] = new Vector3(2.0f * displacement, 0, 0);
        actions[3] = new Vector3(0, 0, -displacement);
        return actions;
    }

    protected Vector3[] MoveDownIntoNextBlock()
    {
        float displacement = 8.5f;
        Vector3[] actions = new Vector3[4];
        actions[0] = new Vector3(0, 0, -2.0f * displacement);
        actions[1] = new Vector3(displacement, 0, 0);
        actions[2] = new Vector3(0, 0, -2.0f * displacement);
        actions[3] = new Vector3(-displacement, 0, 0);
        return actions;
    }

}
