using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapTrigger : MonoBehaviour
{
    [SerializeField] private string triggerName;
    WarpManager warpManager;

    private void Start()
    {
        try
        {
            warpManager = GameObject.Find("WarpManager").GetComponent<WarpManager>();
        }
        catch
        {
            Exception missingManagerException = new Exception("To use WarpTriggers, WarpManager object must exist and be called WarpManager");
            Debug.LogException(missingManagerException);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       warpManager.LogEntry(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        warpManager.LogExit(gameObject);
    }
}
