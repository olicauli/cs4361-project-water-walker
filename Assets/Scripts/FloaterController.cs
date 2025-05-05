using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.HighDefinition;
using Unity.VisualScripting;


public class Floater : MonoBehaviour
{
    // from following this tutorial:
    // https://www.youtube.com/watch?v=vzqoLJmpUqU
    public Rigidbody rb;
    // how deep it goes down before it starts trying to buoy itself back up--
    // since our water is very shallow, 0.15 is a good default value
    public float depthBefSub;
    public float displacementAmount;
    // number of points applying buoyant force
    public int floaters;
    // how slowly it comes back up after being submerged
    public float waterDrag;
    // how slowly it rotates back after being submerged
    public float waterAngularDrag;
    public WaterSurface water;
    WaterSearchParameters Search;
    WaterSearchResult Result;

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForceAtPosition(Physics.gravity / floaters, transform.position, ForceMode.Acceleration);
        Search.startPositionWS = transform.position;

        water.ProjectPointOnWaterSurface(Search, out Result);

        // if the object is submerged under the water, apply buoyant forces
        if (transform.position.y < Result.projectedPositionWS.y)
        {
            float displacementMulti = Mathf.Clamp01(((Result.projectedPositionWS.y - transform.position.y) / depthBefSub) * displacementAmount);
            // apply buoyant force upwards
            rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMulti, 0f), transform.position, ForceMode.Acceleration);
            // apply water drag against velocity
            rb.AddForce(displacementMulti * - rb.linearVelocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            // apply angular water drag against velocity
            rb.AddTorque(displacementMulti * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
