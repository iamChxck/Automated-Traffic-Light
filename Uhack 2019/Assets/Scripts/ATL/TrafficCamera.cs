using System.Collections.Generic;
using UnityEngine;

public class TrafficCamera : MonoBehaviour
{
    int countOfCarsInLane;
    BoxCollider boxCollider;
    List<CarController> listOfCars;
    // the TrafficNode contains data pertaining to the intersection
    int trafficNodeId;

    void Start()
    {
        countOfCarsInLane = 0;
        boxCollider = GetComponent<BoxCollider>();
        listOfCars = new List<CarController>();
        if (boxCollider == null) { Debug.LogError("Box collider of camera is null"); }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            TrafficNode trafficNode = TrafficSystem.GetTrafficNode(trafficNodeId);
            CarController car = collision.gameObject.GetComponent<CarController>();
            if (!trafficNode.HasThreeNodesConnected())
            {
                car.DecideWith3StreetsConnected();
            }
            else if (TrafficSystem.GetTrafficNode(trafficNodeId).HasTwoNodesConnected())
            {
                car.DecideWith2StreetsConnected( trafficNode.left != TrafficNode.NO_ROAD_CONNECTED ||
                                                trafficNode.front != TrafficNode.NO_ROAD_CONNECTED);
            }
            countOfCarsInLane++;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car")) { countOfCarsInLane--; }
    }
    public void SetTrafficNodeId(int id) { trafficNodeId = id;  }
    public string GetCarTrafficInCamera() { return "Traffic Camera: " + countOfCarsInLane; }
}
