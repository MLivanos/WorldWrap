using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodgeball : MonoBehaviour
{
    private GameObject weilder;
    private bool isActive;

    private void Start()
    {
        isActive = false;
    }

    private void Update()
    {
        if (transform.parent != null)
        {
            weilder = transform.parent.gameObject;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Do not harm weilder
        if (collision.gameObject.transform == transform.parent || weilder == collision.gameObject)
        {
            return;
        }
        DodgeballActor actorScript = collision.gameObject.GetComponent<DodgeballActor>();
        if (isActive && actorScript != null)
        {
            actorScript.decrementHealth();
            Destroy(gameObject);
        }
        weilder = null;
        SetActive(false);
    }

    public void SetActive(bool activity)
    {
        isActive = activity;
    }

    public bool IsActive()
    {
        return isActive;
    }

}
