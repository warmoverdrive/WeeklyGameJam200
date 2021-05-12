using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	Camera target;

	private void Start()
	{
		target = Camera.main;
	}

	// Update is called once per frame
	void Update()
	{
		transform.LookAt(target.transform, Vector3.up);
	}
}
