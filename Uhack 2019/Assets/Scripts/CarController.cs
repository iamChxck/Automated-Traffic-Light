using UnityEngine;
using static Stoplight;

public class CarController : MonoBehaviour
{
    public enum CarState
    {
        FromLeft,
        FromRight,
        FromTop,
        FromBottom
    }

    CarState carState;
    public float moveSpeed = 10f;
    // stop,ready, go
    public State state;

    private Animator anim;

    // path to take
    int rngOfPathToTake;
    // decision range
    int minRng, maxRng;
    // decision when three roads exist
    int oneThird, twoThird, threeThird;
    // decision when two roads exist
    int half;
    // camera collider layer mask
    public LayerMask cameraLayerMask;

    const float CHECK_DISTANCE = 50;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        state = State.Go;
        
        //CheckDefaultDirection();
        // percentage chance to turn left, go straight, or right
        oneThird = 40;
        twoThird = 80;
        threeThird = 120;
        // decision rate of the car
        minRng = 1;
        maxRng = 120;
        rngOfPathToTake = Random.Range(minRng, maxRng);
        half = 60;
        StopMoving();
        // cameraLayerMask = LayerMask.GetMask("Traffic Camera");
    }

    // Update is called once per frame
    void Update()
    {
        //DrawLineFromPreviousPosition();
        Movement();
    }
    void DrawLineFromPreviousPosition()
    {
        Vector3 direction;
        if (carState == CarState.FromBottom)
        {
            direction = transform.up + transform.position;
        }
        else if (carState == CarState.FromTop)
        {
            direction = -transform.up + transform.position;
        }
        else if (carState == CarState.FromLeft)
        {
            direction = transform.right + transform.position;
        }
        else
        {
            direction = -transform.right + transform.position;
        }
        Debug.DrawLine(transform.position, direction, Color.red, 5);
    }
    public void SetCarState(CarState carState)
    {
        this.carState = carState;
    }
    void CheckDefaultDirection()
    {
        Debug.DrawLine(transform.position, transform.up + transform.position, Color.blue, 10);
        if (Physics.Raycast(transform.position, transform.right + transform.position, CHECK_DISTANCE))
        {
            Debug.Log("1");
            carState = CarState.FromBottom;
        }
        else if(Physics.Raycast(transform.position, -transform.right + transform.position, CHECK_DISTANCE))
        {
            Debug.Log("2");
            carState = CarState.FromTop;
        }
        else if (Physics.Raycast(transform.position, transform.up + transform.position, CHECK_DISTANCE))
        {
            Debug.Log("3");
            carState = CarState.FromLeft;
        }
        else if (Physics.Raycast(transform.position, -transform.up + transform.position, CHECK_DISTANCE))
        {
            Debug.Log("4");
            carState = CarState.FromRight;
        }
        else
        {
            //Vector3 computedPadding = new Vector3(CHEC);
            // we're in the corner and raycast hits nothing
            if(Physics.Raycast(transform.position, -transform.right, CHECK_DISTANCE, cameraLayerMask))
            {
                //carState = CarState.FromRight;
            }
            else
            {
                //carState = CarState.FromLeft;
            }
            Debug.LogError("Not yet working");
        }
    }
    void CheckIfHasSpace()
    {
        if (carState == CarState.FromTop && Physics.Raycast(transform.position, transform.forward, CHECK_DISTANCE, cameraLayerMask))
        { }
        Debug.LogError("Not yet implemented");
    }
    void Movement()
    {
        if(state == State.Stop) { Debug.Log("Car is waiting"); return; }
        if(carState == CarState.FromLeft) { MoveToSide(false); }
        else if(carState == CarState.FromRight) { MoveToSide(true); }
        else if(carState == CarState.FromTop) { MoveUpwards(false); }
        else if(carState == CarState.FromBottom) { MoveUpwards(true); }
    }
    /// <summary>
    /// Takes the direction from which the car came from and continue moving towards that direction
    /// </summary>
    public void HeadTowardsTheCurrentDirection()
    {
        if (carState == CarState.FromLeft) { MoveToSide(false); }
        else if (carState == CarState.FromRight) { MoveToSide(true); }
        else if (carState == CarState.FromTop) { MoveUpwards(false); }
        else if (carState == CarState.FromBottom) { MoveUpwards(true); }
    }
    /// <summary>
    /// decides whether the car goes left/right/top/bottom according to previous direction
    /// </summary>
    /// <param name="leftOrUp"></param>
    void TurnIfAnyIsPossible(bool leftOrUp)
    {
        if (carState == CarState.FromLeft) { MoveUpwards(leftOrUp?true:false);}
        else if (carState == CarState.FromRight) { MoveUpwards(leftOrUp ? false:true); }
        else if (carState == CarState.FromTop) { MoveToSide(leftOrUp ? false : true); }
        else if (carState == CarState.FromBottom) { MoveToSide(leftOrUp ? true : false); }
    }
    void MoveToSide(bool goingLeft)
    {
        transform.position += (goingLeft ? -transform.right : transform.right) * -1.0f * moveSpeed * Time.deltaTime;
        carState = (goingLeft ? CarState.FromRight:CarState.FromLeft);
    }
    void MoveUpwards(bool goingUp)
    {
        if (state == State.Go)
        {
            transform.position += (goingUp?transform.up :-transform.up)* -1.0f * moveSpeed * Time.deltaTime;
        }
        else if (state == State.Ready)
        {
            transform.position += (goingUp ? transform.up : -transform.up) * -1.0f * Time.deltaTime;
        }
        carState = (goingUp ? CarState.FromBottom : CarState.FromTop);
    }
    public void DecideWith3StreetsConnected()
    {
        // Make one for all of the stoplights with 3 nodes connected
        if (rngOfPathToTake <= oneThird)
        {
            // take the one to the left
            TurnIfAnyIsPossible(true);
        }
        else if (rngOfPathToTake <= twoThird && rngOfPathToTake > oneThird)
        {
            // take the one in front path
            MoveUpwards(true);
        }
        else if (rngOfPathToTake <= threeThird)
        {
            // take the one to the right
            TurnIfAnyIsPossible(false);
        }
    }
    public void DecideWith2StreetsConnected(bool leftOrUp)
    {
        // Make one for all of the stoplights with 2 nodes connected
        if (rngOfPathToTake <= half)
        {
            // take the one in front path
            HeadTowardsTheCurrentDirection();
        }
        else
        {
            // take the other one
            TurnIfAnyIsPossible(leftOrUp);
        }
    }
    public void StopMoving()
    {
        state = State.Stop;
        this.enabled = true;
    }
    // called only by the traffic camera
    public void ChangeState(State state)
    {
        this.state = state;
    }
}