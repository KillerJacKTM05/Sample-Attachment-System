using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bomb : MonoBehaviour
{
    [Range(0,10f)][SerializeField] public float hitRadius = 2f;
    [Range(0, 5000f)] [SerializeField] public float bombPower = 10f;

    public ParticleSystem bombExplosion;
    private AudioSource bombSound;
    private void Start()
    {
        bombExplosion = GetComponentInChildren<ParticleSystem>();
        bombSound = GetComponentInChildren<AudioSource>();
        bombSound.Pause();
        bombExplosion.Stop();
    }

    public void Explosion()
    {
        Collider[] detectFractures = Physics.OverlapSphere(transform.position, hitRadius);
        foreach (var detect in detectFractures)
        {
            Fracture fract = detect.GetComponent<Fracture>();
            if (fract != null)
            {
                fract.Break();
            }
        }

        StartCoroutine(delayOnBomb());
        bombExplosion.Play();
        bombSound.Play();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, hitRadius);
        foreach (var hitted in hitColliders)
        {
            
            Rigidbody rb = hitted.GetComponent<Rigidbody>();
            if(rb!= null && !hitted.gameObject.CompareTag("Ground"))
            {
                //Debug.Log("oh noo I'm hit");
                rb.isKinematic = false;
                rb.AddExplosionForce(bombPower * rb.mass, transform.position+(Vector3.back*.4f)+(Vector3.down*.3f), hitRadius);
                //rb.velocity = (bombPower * rb.mass) * (transform.position - rb.position).normalized;
            }
        }
        StartCoroutine(delayedCall());
    }
    private IEnumerator delayedCall()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    private IEnumerator delayOnBomb()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
