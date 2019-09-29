using System.Collections;
using UnityEngine;
using static Stoplight;

public class SingleStopLight : MonoBehaviour
{
    // defines the state if it's go, stop, or on pause
    public State state;
    Coroutine coroutine;
    // true if the stoplight is suppose to changed
    bool isCurrentlyWaiting = false;
    // time before it changes to the other state
    float secondsToChange;
    // defines how long it should wait before the state switches
    [SerializeField]
    float stopTimerValue, readyTimerValue, goTimerValue;
    MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        secondsToChange = 3;
        stopTimerValue = 3;
        readyTimerValue = 3;
        goTimerValue = 5;

        state = State.Stop;

        // start the loop
        if (!isCurrentlyWaiting)
        {
            coroutine = StartCoroutine(WaitTilTimerToChangeState());
        }
    }

    public State GetState() { return state; }
    IEnumerator WaitTilTimerToChangeState()
    {
        isCurrentlyWaiting = true;
        yield return new WaitForSeconds(secondsToChange);
        isCurrentlyWaiting = false;
        if (state == State.Stop)
        {
            Debug.Log("Go");
            state = State.Go;
            secondsToChange = goTimerValue;
            meshRenderer.material.color = new Color(0, 255, 0);
        }
        else if (state == State.Go)
        {
            Debug.Log("Ready");
            state = State.Ready;
            secondsToChange = readyTimerValue;
            meshRenderer.material.color = new Color(255, 255 / 2, 0);
        }
        else
        {
            Debug.Log("Stop");
            state = State.Stop;
            secondsToChange = stopTimerValue;
            meshRenderer.material.color = new Color(255, 0, 0);
        }
        coroutine = StartCoroutine(WaitTilTimerToChangeState());
    }
    public void SetReadyWaitTime(float value) { readyTimerValue = value; }
    public void SetStopWaitTime(float value) { stopTimerValue = value; }
    public void SetGoWaitTime(float value) { goTimerValue = value; }
    public void ResetTimer() { StopCoroutine(coroutine); coroutine = StartCoroutine(WaitTilTimerToChangeState()); }
}
