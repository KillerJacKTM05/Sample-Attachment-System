using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piano : MonoBehaviour
{
    private Vector3 _offset;
    private bool ejected = false;
    private Rigidbody pianoRb;

    public bool done = false;
    public GameObject solidPiano;
    public GameObject fracturedPiano;

    private MeshRenderer solidRender;
    void Start()
    {
        _offset = solidPiano.transform.position - CameraManager.Instance.getTarget().position;
        pianoRb = solidPiano.GetComponent<Rigidbody>();
        solidRender = solidPiano.GetComponent<MeshRenderer>();
        solidRender.enabled = false;
    }

    void Update()
    {
        if (GameManager.CanPlay && !ejected && !done)
        {
            PianoFollowsThePlayer();
        }
        else if (ejected && !done)
        {
            Eject();
        }

        if (solidPiano.activeInHierarchy)
        {
            fracturedPiano.transform.position = pianoRb.transform.position;
        }
    }

    private void PianoFollowsThePlayer()
    {
        var fixedpos = new Vector3(0, CameraManager.Instance.getTarget().position.y, CameraManager.Instance.getTarget().position.z);
       pianoRb.transform.position = fixedpos + _offset;
      
    }

    private void Eject()
    {
        //solidPiano.transform.SetParent(LevelManager.Instance.GetLevel().transform);
        solidRender.enabled = true;
        pianoRb.useGravity = true;
        done = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ejected = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //ejected = false;
        }
    }
}
