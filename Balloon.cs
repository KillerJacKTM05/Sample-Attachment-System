using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Demos;
using RootMotion.Dynamics;

public class Balloon : MonoBehaviour
{
    private SkinnedMeshRenderer balloonMesh;
    private Rigidbody rb;
    private ConfigurableJoint joint;
    private float shape1 = 0;
    public float maxY = 5f;

    [Range(0,100f)]public float blendspeed = 3f;
    [Range(0, 100f)] public float balloonFlySpeed = 3f;

    public Rigidbody otherRb;
    public ParticleSystem BallonExplosion;


    private void Start()
    {
        BallonExplosion = GetComponentInChildren<ParticleSystem>();
        BallonExplosion.Stop();
    }

    public void Inflate()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        joint = this.gameObject.GetComponent<ConfigurableJoint>();
        balloonMesh = this.gameObject.GetComponent<SkinnedMeshRenderer>();
        joint.connectedBody = otherRb;
        AttachmentManager.I.puppet.state = PuppetMaster.State.Dead;
        StartCoroutine(InflateBalloon());
    }

    public void DetachBalloon()
    {
        StopCoroutine(InflateBalloon());
        joint.connectedBody = null;
        AttachmentManager.I.puppet.state = PuppetMaster.State.Alive;
        //gameObject.transform.SetParent(LevelManager.Instance.GetLevel().transform);
        rb.isKinematic = false;
        rb.velocity = new Vector3(0, balloonFlySpeed, 0);
        StartCoroutine(confetti());
    }

    private IEnumerator InflateBalloon()
    {
        while (AttachmentManager.I.isBalloonActive)
        {
            balloonMesh.SetBlendShapeWeight(0, shape1);
            if(shape1 < 100)
            {
                shape1 += Time.deltaTime * blendspeed;
                this.gameObject.transform.position += (Vector3.up + 3 * Vector3.forward).normalized * balloonFlySpeed * (shape1 / 1000) * Time.deltaTime;
                transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -Mathf.Infinity, maxY), transform.position.z);
            }
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator confetti()
    {
        BallonExplosion.Play();
        balloonMesh.enabled = false;
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
        yield return new WaitForSeconds(0.1f);
    }
}
