﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{

    protected Rigidbody rb;
    public float breakForce = 10000;
    public WheelCollider backWheel;
    public WheelCollider frontWheel;
    public float velocityDrag = 1f;
    public bool isGrounded = true;
    
    protected float targetAngle = 0;
    [SerializeField]
    protected bool idleMode = true;
    public bool IdleMode
    {
        get
        {   
            return idleMode;
        }   
        set
        {
            idleMode = value;
            if (idleMode)
            {
                EnterIdleMode();
            }   
            else
            {
                ExitIdleMode();
            }   
        }   

    }

    
    protected abstract void EnterIdleMode();
    protected abstract void ExitIdleMode();

    protected abstract void SetSteeringAngle();

    protected void ApplyVelocityDrag(float drag)
    {
        rb.AddForce(-drag * rb.velocity.normalized * Mathf.Abs(Vector3.SqrMagnitude(rb.velocity)));
    }

    protected void SetRotationUp()
    {
        Vector3 normal = Vector3.zero;
        GetNormal(out normal);

        if (isGrounded)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(transform.forward, normal), 50f * Time.deltaTime);
        else
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(transform.forward, Vector3.up), 50f * Time.deltaTime);

    }

    protected void GetNormal(out Vector3 normal)
    {
        WheelHit hit;
        Vector3 vect = Vector3.zero;
        bool isGrounded = false;
        if (backWheel.GetGroundHit(out hit))
        {
            vect += hit.normal;
            isGrounded = true;
        }

        if (frontWheel.GetGroundHit(out hit))
        {
            vect += hit.normal;
            isGrounded = true;
        }

        normal = vect;
        this.isGrounded = isGrounded;
    }
    protected bool lockDraftingCheck = false;



    protected IEnumerator CheckIfFollowingDriver()
    {
        bool hitValid = false;
        Vector3 pos = transform.position;
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.SphereCast(ray, 0.4f, out hit,2.5f))
        {
            if (hit.transform.gameObject.GetComponent<Movement>() != null)
            {
                hitValid = true;
            }   
        }


        if (hitValid)
        {
            float distance = Vector3.Distance(transform.position, hit.transform.position);

            if (distance > 0.1f)
            { ApplyVelocityDrag(-velocityDrag / (1 + distance)); }
            else
            { ApplyVelocityDrag(-velocityDrag); }

        }
        yield return new WaitForSeconds(0.5f);
        lockDraftingCheck = false;
    }
    




    public void Go(float motorTorque)
    {
        backWheel.brakeTorque = 0;
        frontWheel.brakeTorque = 0;
        backWheel.motorTorque = motorTorque;
    }

    public void Stop()
    {
        backWheel.brakeTorque = breakForce;
        frontWheel.brakeTorque = breakForce;
        backWheel.motorTorque = 0;
    }




}