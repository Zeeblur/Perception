//WASD to orbit, left Ctrl/Alt to zoom
using UnityEngine;


public class hjkuyiMove : MonoBehaviour {
	public Transform target;

	public float moveSpeed = .25f;


	public void Start () {

		
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
	
	public void LateUpdate () {
		if (target) {

			Vector3 position = transform.position;
			float dx=0, dy = 0, dz=0;
			if(Input.GetKey("h"))
				dx = -1;
			else if(Input.GetKey ("k"))
				dx = 1;
			if(Input.GetKey("u"))
				dz = 1;
			else if(Input.GetKey ("j"))
				dz = -1;
			if(Input.GetKey("y"))
				dy = 1;
			else if(Input.GetKey ("i"))
				dy = -1;

			position.x += dx * moveSpeed;
			position.y += dy * moveSpeed;
			position.z += dz * moveSpeed;


			transform.position = position;
		}
	}
}