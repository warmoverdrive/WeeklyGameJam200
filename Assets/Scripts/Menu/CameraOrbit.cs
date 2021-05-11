using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
	[SerializeField]
	float orbitSpeed = 10f;
	Vector3 target;

	private void Start()
	{
		target = new Vector3(2, 0, 2);
	}

	private void Update()
	{
		transform.RotateAround(target, Vector3.up, 20 * Time.deltaTime * orbitSpeed);
	}
}