using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class ChangeText : MonoBehaviour
{
    private DodgeballPlayer player;
    private TMP_Text text;
    private bool isDisplayingPickupMessage;

    private void Start()
    {
        isDisplayingPickupMessage = true;
        SetupObjects();
    }

    private void Update()
    {
        if (PlayerJustPickedUpBall())
        {
            isDisplayingPickupMessage = false;
            ChangeToThrowMessage();
        }
        if (PlayerJustThrewBall())
        {
            isDisplayingPickupMessage = true;
            ChangeToPickupMessage();
        }
    }

    private void SetupObjects()
    {
        GameObject[] gameObjectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject objectInScene in gameObjectsInScene)
        {
            if (objectInScene.name == "Player")
            {
                player = objectInScene.GetComponent<DodgeballPlayer>();
            }
        }
        text = gameObject.GetComponent<TMP_Text>();
    }

    private void ChangeToPickupMessage()
    {
        text.text = "Click To Pickup A Ball";
    }

    private void ChangeToThrowMessage()
    {
        text.text = "Click Again To Throw!";
    }

    private bool PlayerJustPickedUpBall()
    {
        return isDisplayingPickupMessage && player.gameObject.activeSelf && player.IsHoldingObject();
    }

    private bool PlayerJustThrewBall()
    {
        return !isDisplayingPickupMessage && player.gameObject.activeSelf && !player.IsHoldingObject();
    }
}
