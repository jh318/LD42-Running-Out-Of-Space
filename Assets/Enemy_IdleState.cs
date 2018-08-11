using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_IdleState : StateMachineBehaviour 
{
	[SerializeField] float transitionSensitivity = 0.1f;
	NavMeshAgent agent;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		agent = animator.gameObject.GetComponent<NavMeshAgent>();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		float horizontal = Mathf.Abs(animator.GetFloat("HorizontalVelocity"));
		float forwardVelocity = Mathf.Abs(animator.GetFloat("ForwardVelocity"));

		if(horizontal > transitionSensitivity || forwardVelocity > transitionSensitivity)
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
