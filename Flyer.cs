using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyer : MonoBehaviour
{
    public LayerMask charLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (charLayer == (charLayer | (1 << other.gameObject.layer)))
        {
            var tempChar = other.GetComponent<BasicCharScript>();
            if(tempChar != null)
            {
                tempChar.Fly();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var temp = other.GetComponent<BasicCharScript>();
        if(temp != null)
        {
            temp.Walk();
        }
    }
}
