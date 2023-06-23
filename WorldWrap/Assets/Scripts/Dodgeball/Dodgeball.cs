using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodgeball : MonoBehaviour
{
    private bool isActive;

    private void Start()
    {
        isActive = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Do not harm weilder
        if (collision.gameObject.transform == transform.parent)
        {
            return;
        }
        DodgeballActor actorScript = collision.gameObject.GetComponent<DodgeballActor>();
        if (isActive && actorScript != null)
        {
            actorScript.decrementHealth();
            Debug.Log(collision.gameObject.name + " is hit, current health: " + actorScript.GetHealth());
            if (actorScript.isDead())
            {
                Debug.Log(collision.gameObject.name + " Has died");
            }
        }
        SetActive(false);
    }

    public void SetActive(bool activity)
    {
        isActive = activity;
    }

}
