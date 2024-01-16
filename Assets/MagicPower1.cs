using System.Collections;
using System.Collections.Generic;
using Skills;
using UnityEngine;

public class Corout : MonoBehaviour
{
    // public void DoCoroutine(Animator animator)
    // {
    //     StartCoroutine(DelayToSpawnProjectile(animator));
    // }
    //
    // private IEnumerator DelayToSpawnProjectile(Animator animator)
    // {
        // yield return new WaitForSeconds(1f);
        // foreach (GameObject child in animator.transform)
        // {
        //     if (child.CompareTag("ProjectileSkill"))
        //     {
        //         animator.gameObject.GetComponent<SpawnProjectiles>().SpawnSkill(child);
        //     }
        // }
    // }
}

public class MagicPower1 : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // animator.gameObject.GetComponent<SpawnProjectiles>().SpawnProjectile();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    //
    // }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
