using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballActor : MonoBehaviour
{
    protected int health;
    
    public void decrementHealth()
    {
        health --;
    }

    public bool isDead()
    {
        return health <= 0;
    }
}
