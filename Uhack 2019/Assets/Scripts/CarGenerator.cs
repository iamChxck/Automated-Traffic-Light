using UnityEngine;

public class CarGenerator : MonoBehaviour
{
    // number of exits/places where the cars can spawn
    public int NUMBER_OF_EXITS = 8;
    // position where the cars will generate
    Transform[] listOfSpawnPositions;
    // number of cars to spawn in the spawn position listed above
    public int[] listOfCarSpawnNumber;
    // game object that will contain all the cars
    Transform carContainer;
    // original car to be copied - object pooling
    GameObject car;
    // Start is called before the first frame update
    void Awake()
    {
        listOfSpawnPositions = new Transform[NUMBER_OF_EXITS];
        car = FindObjectOfType<CarController>().gameObject;
        carContainer = GameObject.Find("Car container").transform;

        Transform carSpawnContainer = GameObject.Find("Car Spawns container").transform;
        for (int i = 0; i < carSpawnContainer.childCount; i++)
        {
            listOfSpawnPositions[i]= carSpawnContainer.GetChild(i);
            GenerateCars(i, listOfCarSpawnNumber[i]);
        }
        car.SetActive(false);
    }

    void GenerateCars(int index, int numberOfCars)
    {
        for(int i= 0; i < numberOfCars; i++)
        {
            // destination where the car will go to upon instantiation
            Vector3 teleportDestination = listOfSpawnPositions[index].position;
            GameObject go = Instantiate(car,teleportDestination, Quaternion.identity) as GameObject;
            go.transform.parent = carContainer;
            go.transform.position = teleportDestination;
            go.transform.rotation = car.transform.rotation;

            if (index == 0) { go.GetComponent<CarController>().SetCarState(CarController.CarState.FromBottom); }
            else if (index == 1) { go.GetComponent<CarController>().SetCarState(CarController.CarState.FromRight); }
            else if (index == 2) { go.GetComponent<CarController>().SetCarState(CarController.CarState.FromRight); }
            else if (index == 3) { go.GetComponent<CarController>().SetCarState(CarController.CarState.FromTop); }
            else if (index == 4) { go.GetComponent<CarController>().SetCarState(CarController.CarState.FromTop); }
            else if (index == 5) { go.GetComponent<CarController>().SetCarState(CarController.CarState.FromLeft); }
            else if (index == 6) { go.GetComponent<CarController>().SetCarState(CarController.CarState.FromLeft); }
            else if (index == 7) { go.GetComponent<CarController>().SetCarState(CarController.CarState.FromBottom); }
        }
    }
}
