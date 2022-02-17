using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Kick : MonoBehaviour
{
    [Range(0, 10f)] [SerializeField] public float hitRadius = 3f;
    [Range(0, 2000f)] [SerializeField] public float hitPower = 100f;
    [Range(0, 100f)] [SerializeField] public float shakeMagnitude = 10f;
    [Range(0, 100f)] [SerializeField] public float roughness = 5f;
    [Range(0, 2f)] [SerializeField] public float fadeIn = 0.4f;
    [Range(0, 2f)] [SerializeField] public float fadeOut = 0.4f;
    public LayerMask kickHitLayer;
    public void KickFunc(Transform trans, ParticleSystem par)
    {
        Collider[] hitCols = Physics.OverlapSphere(trans.position, hitRadius, kickHitLayer);
        foreach (var hitted in hitCols)
        {
            //Debug.Log("hit");
            Rigidbody rigid = hitted.attachedRigidbody;
            if(rigid != null)
            {
                //Debug.Log("object");
                StartCoroutine(KickRountine(rigid, par));
            }
        }
    }
    private IEnumerator KickRountine(Rigidbody rigid, ParticleSystem particle)
    {
        yield return new WaitForSeconds(0.5f);
        CameraShaker.Instance.ShakeOnce(shakeMagnitude, roughness, fadeIn, fadeOut);
        particle.Play();
        rigid.AddForce(hitPower * Vector3.forward, ForceMode.Impulse);
    }
}
