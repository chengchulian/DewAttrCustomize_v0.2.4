using System;
using UnityEngine;

namespace Dustyroom;

public class LinearMotion : MonoBehaviour
{
	public enum TranslationMode
	{
		Off,
		XAxis,
		YAxis,
		ZAxis,
		Vector
	}

	public enum RotationMode
	{
		Off,
		XAxis,
		YAxis,
		ZAxis,
		Vector
	}

	public TranslationMode translationMode;

	public Vector3 translationVector = Vector3.forward;

	public float translationSpeed = 1f;

	public RotationMode rotationMode;

	public Vector3 rotationAxis = Vector3.up;

	public float rotationSpeed = 50f;

	public bool useLocalCoordinate = true;

	public float translationAcceleration;

	public float rotationAcceleration;

	private Vector3 TranslationVector => translationMode switch
	{
		TranslationMode.XAxis => Vector3.right, 
		TranslationMode.YAxis => Vector3.up, 
		TranslationMode.ZAxis => Vector3.forward, 
		TranslationMode.Vector => translationVector, 
		TranslationMode.Off => Vector3.zero, 
		_ => throw new ArgumentOutOfRangeException(), 
	};

	private Vector3 RotationVector => rotationMode switch
	{
		RotationMode.XAxis => Vector3.right, 
		RotationMode.YAxis => Vector3.up, 
		RotationMode.ZAxis => Vector3.forward, 
		RotationMode.Vector => rotationAxis, 
		RotationMode.Off => Vector3.zero, 
		_ => throw new ArgumentOutOfRangeException(), 
	};

	private void Update()
	{
		if (translationMode != 0)
		{
			Vector3 positionDelta = TranslationVector * translationSpeed * Time.deltaTime;
			if (useLocalCoordinate)
			{
				base.transform.localPosition += positionDelta;
			}
			else
			{
				base.transform.position += positionDelta;
			}
		}
		if (rotationMode != 0)
		{
			Quaternion rotationDelta = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, RotationVector);
			if (useLocalCoordinate)
			{
				base.transform.localRotation = rotationDelta * base.transform.localRotation;
			}
			else
			{
				base.transform.rotation = rotationDelta * base.transform.rotation;
			}
		}
	}

	private void FixedUpdate()
	{
		translationSpeed += translationAcceleration;
		rotationSpeed += rotationAcceleration;
	}
}
