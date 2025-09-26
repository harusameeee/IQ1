using UnityEngine;

public class AnimatorStateController : MonoBehaviour
{
    [SerializeField] Animator anim = null;
    [SerializeField] ConditionalZMovement stateController = null;
    [SerializeField] string stateParam = "state";
    ConditionalZMovement.State prevState;

    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
        if (stateController == null) stateController = GetComponent<ConditionalZMovement>();
        prevState = stateController.currentState;
        anim.SetInteger(stateParam, (int)prevState);
    }

    void Update()
    {
        var nowState = stateController.currentState;
        if (nowState != prevState)
        {
            anim.SetInteger(stateParam, (int)nowState);
            prevState = nowState;
        }

        // Startアニメが終わったらFlyへ
        //if (nowState == ConditionalZMovement.State.Start)
        //{
        //    AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        //    if (info.IsName("Start") && info.normalizedTime >= 1.0f)
        //    {
        //        stateController.currentState = ConditionalZMovement.State.Fly;
        //        anim.SetInteger(stateParam, (int)ConditionalZMovement.State.Fly);
        //        prevState = ConditionalZMovement.State.Fly;
        //    }
        //}

        // Stopアニメが終わったらStartへ
        //if (nowState == ConditionalZMovement.State.Stop)
        //{
        //    AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        //    if (info.IsName("Stop") && info.normalizedTime >= 1.0f)
        //    {
        //        stateController.currentState = ConditionalZMovement.State.Start;
        //        anim.SetInteger(stateParam, (int)ConditionalZMovement.State.Start);
        //        prevState = ConditionalZMovement.State.Start;
        //    }
        //}
    }
}