using UnityEngine;

public class TrafficNode : MonoBehaviour
{
    public static int NO_ROAD_CONNECTED = 0;
    static int TrafficNodeCount = 0;
    private int id;
    // index of the traffic node in the system
    public int front, left, right, back;

    public void Start()
    {
        TrafficNodeCount++;
        // get id based from the child parent relationship
        // Traffic Light container > Traffic Light > TrafficNode (Script)
        for(int i=0;i< transform.parent.parent.childCount; i++)
        {
            if(transform.parent.gameObject == transform.parent.parent.GetChild(i).gameObject)
            {
                id = i;
            }
        }
    }
    public int GetId() { return id; }
    // checks if the current node has two or more nodes connected
    public bool HasTwoNodesConnected()
    {
        // number of nodes connected to the current node
        int nodesConnected = 0;
        if (front != 0) { nodesConnected++; }
        if (left != 0) { nodesConnected++; }
        if (right != 0) { nodesConnected++; }
        if (back != 0) { nodesConnected++; }
        return nodesConnected != 2;
    }
    // checks if the current node has two or more nodes connected
    public bool HasThreeNodesConnected()
    {
        // number of nodes connected to the current node
        int nodesConnected = 0;
        if (front != 0) { nodesConnected++; }
        if (left != 0) { nodesConnected++; }
        if (right != 0) { nodesConnected++; }
        if (back != 0) { nodesConnected++; }
        return nodesConnected != 3;
    }

}
