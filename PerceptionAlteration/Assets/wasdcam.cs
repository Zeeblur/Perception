//WASD to orbit, left Ctrl/Alt to zoom
using UnityEngine;

[AddComponentMenu("Camera-Control/wasdcam")]

public class wasdcam : MonoBehaviour {
	public Transform target;
	public float distance = 20.0f;
	public float zoomSpd = 2.0f;
	
	public float thetaSpeed = 5f;
	public float phiSpeed = 5f;
	public float moveSpeed = .25f;

	//rotation
	private float theta = 0.0f;
	private float phi = 0.0f;


	public void Start () {
		
		theta = 0f;
		phi = 0f;
		
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
	
	public void LateUpdate () {
		if (target) {
			float dtheta = 0, dphi = 0;
			if(Input.GetKey("left"))
				dphi = -1;
			else if (Input.GetKey("right"))
				dphi = 1;
			if(Input.GetKey("up"))
				dtheta  = -1;
			else if (Input.GetKey("down"))
				dtheta  = 1;

			phi +=  dphi * phiSpeed;
			theta += dtheta * thetaSpeed;
            if (theta > 90)
                theta = 90;
            else if (theta < -90)
                theta = -90;


            float cosYaw = (float)(Mathf.Cos((phi / 180 * Mathf.PI)));
			float sinYaw = (float)(Mathf.Sin((phi / 180 * Mathf.PI)));
			float cosPitch = (float)(Mathf.Cos((theta / 180 * Mathf.PI)));
			float sinPitch = (float)(Mathf.Sin((theta / 180 * Mathf.PI)));


			Vector3 frontVector = new Vector3(sinYaw * cosPitch, -sinPitch, cosYaw * cosPitch);
			Vector3 topVector = new Vector3(sinYaw * sinPitch, cosPitch, cosYaw * sinPitch);
			Vector3 rightVector = Vector3.Cross (topVector, frontVector);



			//distance -= Input.GetAxis("Fire1") *zoomSpd* 0.02f;
			//distance += Input.GetAxis("Fire2") *zoomSpd* 0.02f;
			
			Quaternion rotation = Quaternion.Euler(theta, phi, 0.0f);


			//Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
			Vector3 position = transform.position;
			float dx=0, dz=0;
			if(Input.GetKey("a"))
				dx = -1;
			else if(Input.GetKey ("d"))
				dx = 1;
			if(Input.GetKey("w"))
				dz = 1;
			else if(Input.GetKey ("s"))
				dz = -1;

			position += (dx * rightVector + dz * frontVector)  * moveSpeed;


			transform.rotation = rotation;
			transform.position = position;
		}
	}
}