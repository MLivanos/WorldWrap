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
        DodgeballActor actorScript = collision.gameObject.GetComponent<DodgeballActor>();
        if (isActive && actorScript)
        {
            actorScript.decrementHealth();
            if (actorScript.isDead())
            {
                Debug.Log(" Has died");
            }
        }
        SetActive(false);
    }

    public void SetActive(bool activity)
    {
        isActive = activity;
    }

}
