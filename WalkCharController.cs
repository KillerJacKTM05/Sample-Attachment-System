using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Demos;
using RootMotion.Dynamics;
using DG.Tweening;
using Sirenix.OdinInspector;

public class WalkCharController : UserControlThirdPerson
{
	// Start is called before the first frame update
	public ParticleSystem kickParticle;
	public GameObject forcePoint;

	private Vector3 particleOffset;
	private Kick kickReference;

	private bool happiness = false;
	protected override void Start()
    {
		happiness = false;
		kickParticle.Stop();
		particleOffset = kickParticle.gameObject.transform.position - this.transform.position;
		kickReference = gameObject.GetComponent<Kick>();
	}

    // Update is called once per frame
    protected override void Update()
    {

		if(AttachmentManager.I.isKicked)
        {
			thisBehaviorPuppet.puppetMaster.targetAnimator.CrossFadeInFixedTime("Kick", 0.08f);
            if (thisBehaviorPuppet.state == BehaviourPuppet.State.Puppet)
            {
				kickReference.KickFunc(forcePoint.transform, kickParticle);
			}
        }

        state.crouch = false;

		if (AttachmentManager.I.isGameWon && !happiness)
		{
			thisBehaviorPuppet.puppetMaster.targetAnimator.CrossFadeInFixedTime("Victory", 0.08f);
			happiness = true;
		}

		if (!GameManager.CanPlay)
        {
			state.move = Vector3.zero;
			state.lookPos = transform.position + transform.forward;
			return;
        }


		float h = Input.GetAxisRaw("Horizontal");
		float v = Vector3.forward.magnitude;

		// calculate move direction
		Vector3 move = new Vector3(h, 0f, v).normalized;

		// Flatten move vector to the character.up plane
		if (move != Vector3.zero)
		{
			Vector3 normal = transform.up;
			Vector3.OrthoNormalize(ref normal, ref move);
			state.move = move;
		}
		else state.move = Vector3.zero;

		bool walkToggle = Input.GetKey(KeyCode.LeftShift);

		// We select appropriate speed based on whether we're walking by default, and whether the walk/run toggle button is pressed:
		//float walkMultiplier = (walkByDefault ? walkToggle ? 1 : 0.5f : walkToggle ? 0.5f : 1);
		state.move *= speedMultiplier;
		kickParticle.gameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z) + particleOffset;

		// calculate the head look target position
		state.lookPos = transform.position + transform.forward * 100f;
	}
}
