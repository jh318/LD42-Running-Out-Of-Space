using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : StateMachineBehaviour 
{
	Rigidbody body;
	PlayerController player;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		body = animator.gameObject.GetComponent<Rigidbody>();
		player = animator.gameObject.GetComponent<PlayerController>();
		animator.SetBool("CanJump", false);
		animator.SetBool("IsJumping", true);
		animator.ResetTrigger("Landing"); // Reseolve bug with trigger getting set at start (due to player positioning probably)
		animator.SetBool("IsIdle", false);
		animator.SetBool("IsRunning", false);

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

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
