using UnityEngine;

public class NPCWalk : StateMachineBehaviour
{
    [SerializeField] float speed;
    private NPC_New npcState;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        npcState = animator.GetComponent<NPC_New>();
        npcState.ChangePos();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        npcState.myTransform.position += npcState.myTransform.forward * speed * Time.deltaTime;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
