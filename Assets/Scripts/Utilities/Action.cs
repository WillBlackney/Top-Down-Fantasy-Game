using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{

    /* This class allows the sending of yield statements back
     * up the stack, and allows deeply nested coroutines to
     * track the state and progress of each other. The main purpose 
     * of this is to control the timing of visual events in the game.
     * Better practice dictates using a structured event queue system, but that
     * is beyond the scope of and time frame of this project.
     */

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
