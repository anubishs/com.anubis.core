using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvents : MonoBehaviour
{
    [Header("Events")]
    [Tooltip("Set the array number to a higher one to have multiple event and call then on animation events with the CallAnimationEvent(int number of array)")]
    public UnityEvent[] animationEvents;
    public float delay;
    public void CallAnimationEvent(int whichEvent)
    {
        animationEvents[whichEvent].Invoke();
    }
    public void DelayCallAnimationEvent(int whichEvent)
    {
        StartCoroutine(DelayedCallAnimationCoroutine(whichEvent));
    }
    private IEnumerator DelayedCallAnimationCoroutine(int whichEvent)
    {
        yield return new WaitForSeconds(delay);
        animationEvents[whichEvent].Invoke();
    }
}
