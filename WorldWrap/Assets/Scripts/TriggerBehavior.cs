using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehavior : MonoBehaviour
{
    protected WrapManager wrapManager;

    protected void Start()
    {
        try
        {
            wrapManager = GameObject.Find("WarpManager").GetComponent<WrapManager>();
        }
        catch
        {
            Exception missingManagerException = new Exception("To use WarpTriggers, WarpManager object must exist and be called WarpManager");
            Debug.LogException(missingManagerException);
        }
    }

}
