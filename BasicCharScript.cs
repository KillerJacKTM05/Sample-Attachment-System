using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCharScript : MonoBehaviour
{
    public bool isFlying = false;
    public WalkCharController walkChar;

    public void Fly()
    {
        walkChar.state.jump = true;
    }

    public void Walk()
    {
        walkChar.state.jump = false;
    }
}
