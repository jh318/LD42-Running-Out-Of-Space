using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : StateMachineBehaviour 
{	
	[SerializeField] float tractionSensitivity = 7.0f;
	[SerializeField] float slideControl = 10f;

	PlayerController player;
	Rigidbody body;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		player = FindObjectOfType<PlayerController>();
		body = animator.gameObject.GetComponent<Rigidbody>();

		animator.SetBool("CanJump", true);

	}
	
	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		float horizontal = Mathf.Abs(animator.GetFloat("Horizontal"));
		float vertical = Mathf.Abs(animator.GetFloat("Vertical"));
		if (!animator.applyRootMotion)
		{
			body.velocity += player.TargetDirection * slideControl * Time.deltaTime;
		}
		
		if(!animator.GetBool("IsJumping")) // Leave disabled if transitioning to Jump
		{
			if (Mathf.Abs(body.velocity.z) < tractionSensitivity 
				&& Mathf.Abs(body.velocity.x) < tractionSensitivity)
			{
				animator.applyRootMotion = true;
			}
		}
		
		if(horizontal < 0.1f && vertical < 0.1f)
		{
			animator.SetBool("IsIdle", true);
			animator.SetBool("IsRunning", false);
		}

		Debug.Log(body.velocity.z);
	}

	// // // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	// override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	// }

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	// override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	// }

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
