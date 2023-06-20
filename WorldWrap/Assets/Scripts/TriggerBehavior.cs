using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehavior : MonoBehaviour
{
    protected WrapManager wrapManager;
    protected int wrapLayer;

    protected virtual void Start()
    {
        try
        {
            wrapManager = GameObject.Find("WrapManager").GetComponent<WrapManager>();
        }
        catch
        {
            Exception missingManagerException = new Exception("To use TriggerBehavior, WrapManager object must exist and be called WrapManager");
            Debug.LogException(missingManagerException);
        }
        wrapLayer = wrapManager.GetWrapLayer();
    }

}
