using System;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSystem : MonoBehaviour
{
    static List<TrafficCamera> listOfTrafficCamera;
    static List<TrafficNode> listOfTrafficNodes;
    Capacity capacity;
    void Start()
    {
        listOfTrafficNodes = new List<TrafficNode>();
        GameObject trafficLightContainer = GameObject.Find("Traffic Light container");
        foreach(Transform trafficLightTransform in trafficLightContainer.GetComponentInChildren<Transform>())
        {
            listOfTrafficNodes.Add(trafficLightTransform.Find("Traffic Node").GetComponent<TrafficNode>());
        }
    }

    void Calculate()
    {
        //capacity.matrix.Add(new[] { new float {0,0 } };
    }
    public static TrafficNode GetTrafficNode(int id)
    {
        try { return listOfTrafficNodes[id]; }
        catch (ArgumentOutOfRangeException) { return null; }
    }
}
