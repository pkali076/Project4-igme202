using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public abstract class Vehicle : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 vehiclePosition;
    private Vector3 direction;
    private Vector3 velocity;
    private Vector3 acceleration;
    private Vector3 futureLocation;



    public Vector3 Position => vehiclePosition;

    [SerializeField]
    [Min(0.001f)]
    private float mass = 1;

   // Bounds worldBounds;
    public float Mass => mass;

    public float maxSpeed = 3f;
    public float maxForce = 2f;

    public float radius = 6f;
    public float forwardDistance = 100f;
    public float forwardRadius = 50f;
    public float forwardAngle = 0f;

    Vector3 cameraPosition;
    Vector3 halfCameraSize = Vector3.zero;

    //make reference to scene manager for each vehicle
    public SceneManager sceneManager;


    protected void Start()
    {
        //vehiclePosition = transform.position;
        vehiclePosition = transform.position;
        cameraPosition = Camera.main.transform.position;
        halfCameraSize.z = Camera.main.orthographicSize;
        halfCameraSize.x = halfCameraSize.z * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        CalcSteeringForces();
        UpdatePosition();
       // BounceVehicleAroundCamera();
        SetTransform();


    }
    private void UpdatePosition()
    {

        velocity += acceleration * Time.deltaTime;

        //dont worry about the y axis, just focused on xz plane
        velocity.y = 0;

        vehiclePosition += velocity * Time.deltaTime;
        vehiclePosition.y = 0;


        direction = velocity.normalized;
        acceleration = Vector3.zero;
       // transform.position = vehiclePosition;
       // transform.rotation = Quaternion.LookRotation(Vector3.forward, acceleration);

      //  acceleration = Vector3.zero;
    }
    public void ApplyFriction(float coeff)
    {
        Vector3 friction = velocity.normalized * -1;
        friction *= coeff;
        acceleration += friction;
    }

    protected abstract void CalcSteeringForces();


    public void ApplyForce(Vector3 force)
    {

        acceleration += force / mass;
    }

    private void SetTransform()
    {
        transform.position = vehiclePosition;
    }



    public Vector3 Wander(Vector3 targetPosition)
    {
        float change = .3f;

        forwardAngle += Random.Range(-change, change);

        futureLocation = velocity;
        futureLocation = futureLocation.normalized;
        futureLocation *= forwardDistance;
        futureLocation += vehiclePosition;

        float x = forwardRadius * Mathf.Cos(forwardAngle);
        float z = forwardRadius * Mathf.Sin(forwardAngle);
        Vector3 desiredLocation = new Vector3(x, 0, z);
        targetPosition = futureLocation + desiredLocation;

        return Seek(targetPosition);
 
    }

    /* 
     * 
     * ********************new version of Arrive() function.. however this is still named "Seek"********************************
     * 
     * */
    public Vector3 Seek(Vector3 targetPosition)
    {

        Vector3 desiredVelocity = targetPosition - vehiclePosition;
        float d = desiredVelocity.magnitude; //create a float based on magnitude of desired trajectory
        desiredVelocity = desiredVelocity.normalized; //normalize vector

        if (d < 100) //if distance is within 100 pixels
        {
            // float m = map(d, 0, 100, 0, maxSpeed); //from natureofcode example, ch. 6
            desiredVelocity = desiredVelocity * .5f; //lower the speed
        }
        else
        {
            desiredVelocity = desiredVelocity * maxSpeed; //else, maximize speed
        }

        Vector3 seekingForce = desiredVelocity - velocity; //returning the force forward to locate desired target

        seekingForce = Vector3.ClampMagnitude(seekingForce, maxSpeed); //using type name Vector3 for maxSpeed.
        return seekingForce; //return Vector3 value
    }

    public Vector3 Seek(GameObject targetObject)
    {
        return Seek(targetObject.transform.position);
    }
    public Vector3 Seek(Vehicle targetVehicle)
    {
        return Seek(targetVehicle.Position);
    }

    public Vector3 GetFuturePosition(float seconds)
    {
        return vehiclePosition + velocity * seconds;
    }



    public Vector3 Flee()
    {
        return Vector3.zero;
    }

    public Vector3 Flee(Vector3 targetPosition)
    {
        Vector3 desiredVelocity = vehiclePosition - targetPosition;

        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        Vector3 seekingForce = desiredVelocity - velocity;

        return seekingForce;
    }

    public Vector3 Flee(GameObject targetObject)
    {
        return Flee(targetObject.transform.position);
    }

    public Vector3 Flee(Vehicle targetVehicle)
    {
        return Flee(targetVehicle.Position);
    }

    public Vector3 Pursue(Vehicle targetVehicle, float seconds = 1f)
    {
        return Seek(targetVehicle.GetFuturePosition(seconds));
    }

    public Vector3 Evade(Vehicle targetVehicle, float seconds = 1f)
    {
        return Flee(targetVehicle.GetFuturePosition(seconds));
    }

    void BounceVehicleAroundCamera()
    {
        if (vehiclePosition.x > cameraPosition.x + halfCameraSize.x)
        {
            vehiclePosition.x = cameraPosition.x + halfCameraSize.x;
            velocity.x *= -1f;
        }
        else if (vehiclePosition.x < cameraPosition.x - halfCameraSize.x)
        {
            vehiclePosition.x = cameraPosition.x - halfCameraSize.x;
            velocity.x *= -1f;
        }
        if (vehiclePosition.z > cameraPosition.z + halfCameraSize.z)
        {
            vehiclePosition.z = cameraPosition.z + halfCameraSize.z;
            velocity.z *= -1f;
        }
        else if (vehiclePosition.z < cameraPosition.z - halfCameraSize.z)
        {
            vehiclePosition.z = cameraPosition.z - halfCameraSize.z;
            velocity.z *= -1f;
        }
    }
    //public void FindNewPoisition()
    //{
    //    vehiclePosition.x = Random.Range(worldBounds.min.x, worldBounds.max.x);
    //    vehiclePosition.z = Random.Range(worldBounds.min.z, worldBounds.max.z);
    //    vehiclePosition.y = 0;

    //    transform.position = vehiclePosition;
    //}

    private void ObstacleAvoidance()
    {

    }
}

