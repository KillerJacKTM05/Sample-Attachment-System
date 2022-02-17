using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour
{
    public GameObject SolidObject;
    public GameObject FracturedObject;

    private Rigidbody[] rbs;
    private bool characterEntered = false;
    private bool broken = false;
    private void Start()
    {
        FracturedObject.SetActive(false);
    }
    private void Update()
    {
        if(characterEntered == true && AttachmentManager.I.isKicked && broken == false)
        {
            Break();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            characterEntered = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            characterEntered = false;
        }
    }
    public void Break()
    {
        //Debug.Log("entered here");
        SolidObject.SetActive(false);
        SolidObject = null;
        FracturedObject.SetActive(true);
        rbs = gameObject.GetComponentsInChildren<Rigidbody>();
        StartCoroutine(routin1());
        broken = true;
    }
    private IEnumerator routin1()
    {
        yield return new WaitForSeconds(0.4f);
        foreach( var rb in rbs)
        {
            rb.isKinematic = false;
        }
    }
}
