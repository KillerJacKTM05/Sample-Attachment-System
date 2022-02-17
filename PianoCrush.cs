using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PianoCrush : MonoBehaviour
{
    [Range(0, 10f)] public float fallStrength = 4f;
    public Piano pianoReference;

    private void OnCollisionEnter(Collision collision)
    {
        if (pianoReference.done)
        {
            if (collision.collider.CompareTag("Player"))
            {
                Debug.Log("uf");
                pianoReference.solidPiano.SetActive(false);
                pianoReference.fracturedPiano.SetActive(true);
            }
            else
            {
                pianoReference.solidPiano.SetActive(false);
                pianoReference.fracturedPiano.SetActive(true);
                GiveForce();
            }
        }        
    }
    private void GiveForce()
    {
        Rigidbody[] rigids = pianoReference.fracturedPiano.GetComponents<Rigidbody>();
        foreach (var rbs in rigids)
        {
            rbs.AddExplosionForce(fallStrength, rbs.transform.position, 1);
        }
    }
}
