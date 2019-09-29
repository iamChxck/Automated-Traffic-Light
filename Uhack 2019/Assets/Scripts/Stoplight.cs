using UnityEngine;

public class Stoplight : MonoBehaviour
{
    // north east west south
    // this controls the light
    SingleStopLight[] singleStopLights;
    // this detects the cars and trigger 
    TrafficCamera[] trafficCameras;

    void Start()
    {
        const int directions = 4;
        singleStopLights = new SingleStopLight[directions];
        trafficCameras = new TrafficCamera[directions];
        for(int i = 0; i < directions; i++)
        {
            singleStopLights[i] = transform.Find("Single Light container").GetChild(i).GetComponent<SingleStopLight>();
            trafficCameras[i] = transform.Find("Traffic Camera container").GetChild(i).GetComponent<TrafficCamera>();
        }
    }
    public enum State
    {
        Stop,
        Ready,
        Go
    }
}
