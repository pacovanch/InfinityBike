﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BezierCurve : MonoBehaviour

{
	public GameObject curvePrefab; 

	void Start(){
		for (float i = 0;i < 1f; i=i+0.05f){
			Instantiate (curvePrefab,GetPoint(i),Quaternion.identity); 
		}
	}
    
    public Vector3[] points;

    public void Reset()
    {
        points = new Vector3[] {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
			new Vector3(4f, 0f, 0f)
        };
    }

    public Vector3 GetPoint(float t)
    {
		return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2],points[3], t));
    }
    public  Vector3 GetVelocity(float t)
    {
        return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2],points[3], t)) -
            transform.position;
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }
}

