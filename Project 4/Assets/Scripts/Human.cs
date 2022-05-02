using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Vehicle
{
    //public Vector3 vehiclePosition;
    public Zombie enemyZombie;

    //[SerializeField]
    //public Transform objectHTransform;
    //public Bounds objectHBounds;

    protected override void CalcSteeringForces()
    {
        Vector3 ultimateForce = Vector3.zero;

        ultimateForce += Wander(transform.position);
        ApplyForce(ultimateForce);

        ultimateForce += Evade(enemyZombie);

        ultimateForce = Vector3.ClampMagnitude(ultimateForce, maxForce);

        ApplyForce(ultimateForce);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }
    //public void Separate(List<Human> human)
    //{
    //    float desiredSeparation = radius * 1.5f;
    //    int count = 0;

    //    var s = new Human();
    //    foreach(Human other in human)
    //    {
    //        float d =  - other.vehiclePosition;
    //        float d = new Vector3
    //    }
    //}


}
