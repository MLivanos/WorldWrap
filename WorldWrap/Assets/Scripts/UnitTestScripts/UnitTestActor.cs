using UnityEngine;

public class UnitTestActor : MonoBehaviour
{
    public Vector3 heldObjectPosition; 
    private GameObject heldObject;

    public void TeleportTo(Vector3 position)
    {
        transform.position = position;
    }

    public void MoveInDirection(Vector3 direction)
    {
        transform.Translate(direction);
    }

    public void PickUp(GameObject objectToPickup)
    {
        objectToPickup.transform.parent = transform;
        heldObject = objectToPickup;
        objectToPickup.transform.localPosition = heldObjectPosition;
    }

    public void PlaceDown()
    {
        heldObject.transform.parent = null;
        heldObject = null;
    }
}
