using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrabScript : MonoBehaviour
{
    public GameObject Path;
    private GameObject crab;
    private Rigidbody rb;
    // for creating a path
    // private GameObject pathParent; // the game object that contains each point we want to go to
    private List<Vector3> pointsInPath = new();
    private int pointIndex;
    // NOTE: rotationspeed isnt really working, i think the angular speed is capped in navmesh agent. change that instead.
    public float walkSpeed;
    public float rotationSpeed;
    // if you want the navmesh agent to walk along its path forever
    public bool loopPath = false;
    private bool isDone = false;
    void Start()
    {
        // for moving the crab
        rb = GetComponent<Rigidbody>();

        // if a path is specified, set the path;
        if (Path != null) PopulatePointsInPath();
    }

    // Update is called once per frame
    void Update()
    {
        // move and update if we have at least 1 point
        if (hasPath() && !isDone)
        {
            // Debug.Log("Update: Has path");
            // if we have arrived at the point, set a new destination
            if (ArrivedAtDestination())
            {
                // Debug.Log("Arrived at destination");
                SetNextDestination();
            }

            // Debug.Log("Moving");
            // move towards current destination
            Move();
        }
    }

    private void Move()
    {
        // get direction
        Vector3 currPos = rb.position;
        Vector3 targetPos = GetNextDestination();
        // Debug.Log("target position: " + targetPos);
        currPos.y = 0; targetPos.y = 0;
        Vector3 moveDir = (targetPos - currPos).normalized; 
        // do not care about y value

        // Debug.Log("currPos: " + currPos);
        // Debug.Log("moveDir: " + moveDir);
        // get the next position
        Vector3 newPos = rb.position + moveDir * walkSpeed * Time.fixedDeltaTime;
        // Debug.Log("newPos: " + newPos);
        // move to the position
        rb.MovePosition(newPos);

        // face towards our move direction, if we are moving
        if (moveDir.sqrMagnitude > 0.01f)
            FaceDirection(moveDir);
    }

    private void FaceDirection(Vector3 moveDir)
    {
        // get where the crab is looking towards
        Quaternion rotation = Quaternion.LookRotation(moveDir);
        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, rotation, rotationSpeed * Time.fixedDeltaTime));
    }

    private bool ArrivedAtDestination()
    {
        Vector3 currLocation = rb.position;
        Vector3 targetLocation = GetNextDestination();
        currLocation.y = 0; targetLocation.y = 0;

        // if we are within 0.01f of our distance, we have arrived
        Vector3 distance = targetLocation - currLocation;
        return distance.sqrMagnitude > 0.01f? false: true;
    }

    private void PopulatePointsInPath()
    {
        // make sure list is empty before populating
        pointsInPath.Clear();

        // for every gameobject in the point parent object
        foreach (Transform pt in Path.transform)
        {
            // Debug.Log(pt);
            // add it to the list
            pointsInPath.Add(pt.position);
        }
    }

    private Vector3 GetNextDestination()
    {
        return pointsInPath[pointIndex];
    }
    private void SetNextDestination()
    {
        // if we do not have a path specified, just return
        if (!hasPath()) return;

        // determine the next index to use,
        if (pointIndex < pointsInPath.Count - 1)
        {
            pointIndex++;
        }
        else 
        {
            // if path is not looping, then we're done.
            if (!loopPath) isDone = true;
            // otherwise, start back from the first point
            else pointIndex = 0;
        }
    }
    // checks if a path is specified
    private bool hasPath()
    {
        return Path != null && pointsInPath.Count > 0;
    }
}
