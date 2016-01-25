using UnityEngine;
using System.Collections;

public class Fish : BaseAI
{
    Quaternion newRot;
    Vector3 myRot;

    float rotationSpeed = 90;
    float wiggleAngle = 67.5f;
    float wiggleTime = 0.25f;


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        isUnderWater = IsUnderWater();

        AvoidObstacles();
        Wiggle();
    }

    bool IsUnderWater()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.up, out hit))
            if (hit.collider.gameObject.layer == (int)Layers.Water)
                return true;
        return false;
    }

    /// <summary>
    /// Ervoor zorgen dat hij niet tegen muren op botst
    /// </summary>
    void AvoidObstacles()
    {
        RaycastHit hit;
        float rayDistance = 2;

        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance))
            if (hit.collider)
                transform.Rotate(Vector3.up * Time.smoothDeltaTime * rotationSpeed);
        if (Physics.Raycast(transform.position, Vector3.up, out hit, rayDistance))
            if (hit.collider)
                transform.Rotate(Vector3.right * Time.smoothDeltaTime * rotationSpeed);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
            if (hit.collider)
                transform.Rotate(Vector3.left * Time.smoothDeltaTime * rotationSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, transform.rotation.y, 0, transform.rotation.w), Time.smoothDeltaTime);
    }

    /// <summary>
    /// Wiggles the fish :D
    /// </summary>
    void Wiggle()
    {
        if (stateManager.currentState.ToString() == "Idle")
            return;

        if (wiggleTime < 0)
        {
            myRot = transform.localEulerAngles;
            myRot.y += wiggleAngle;

            newRot = Quaternion.Euler(myRot);

            wiggleAngle *= -1;

            wiggleTime = (stateManager.currentState.ToString() == "Flee") ? 0.1f : 0.25f;
        }
        else
            wiggleTime -= Time.smoothDeltaTime;

        if (transform.rotation != newRot)
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, Time.smoothDeltaTime);
    }
}