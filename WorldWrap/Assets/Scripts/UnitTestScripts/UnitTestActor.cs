using UnityEngine;

public class UnitTestActor : MonoBehaviour
{
    public Vector3 heldObjectPosition;
    private Rigidbody actorRigidbody;
    private GameObject heldObject;

    private void Start()
    {
        actorRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    public void TeleportTo(Vector3 position)
    {
        transform.position = position;
    }

    public void MoveInDirection(Vector3 direction)
    {
        actorRigidbody.velocity = direction;
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
