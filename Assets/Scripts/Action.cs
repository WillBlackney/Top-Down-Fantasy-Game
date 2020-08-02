using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    public bool actionResolved;

    public Action()
    {
        actionResolved = false;
    }
    public void MarkAsComplete()
    {
        actionResolved = true;
    }
}
