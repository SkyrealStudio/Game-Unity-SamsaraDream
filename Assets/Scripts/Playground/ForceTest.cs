using System;
using UnityEditor;
using Unity;
using UnityEngine;

public class ForceTest : MonoBehaviour
{
    public const float FixedUpdateTimeStep = 0.02f;

    private Vector3 previousVelocity, currentVelocity;
    new Rigidbody2D rigidbody;
    public Vector3 acceleration;
    public Vector3 force;
    public Vector3 addiingForce = new Vector3(0, 1, 0);

    public ForceMode addingForceMode = ForceMode.Impulse;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        previousVelocity = currentVelocity;
        currentVelocity = rigidbody.velocity;
        acceleration = new Vector3((currentVelocity - previousVelocity).x / FixedUpdateTimeStep,
                                   (currentVelocity - previousVelocity).y / FixedUpdateTimeStep,
                                   (currentVelocity - previousVelocity).z / FixedUpdateTimeStep);
        force = acceleration * rigidbody.mass;
    }
}