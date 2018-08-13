using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateMachineBehaviour 
{
	PlayerController player;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		player = FindObjectOfType<PlayerController>();
		animator.SetBool("IsIdle", true);
		animator.ResetTrigger("Land");
		animator.ResetTrigger("Attack");
		animator.ResetTrigger("SpecialAttack");
		animator.SetBool("IsJumping", false);
		animator.SetBool("CanJump", true);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		float horizontal = Mathf.Abs(animator.GetFloat("Horizontal"));
		float vertical = Mathf.Abs(animator.GetFloat("Vertical"));

		if(horizontal > 0.1f || vertical > 0.1f)
		{
			animator.SetBool("IsIdle", false);
			animator.SetBool("IsRunning", true);
		}
	
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
