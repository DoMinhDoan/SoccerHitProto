using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
	private Vector3 m_currentPosition;

	public void NextLevel() {
		Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (!Main.instance.IsGameOver ()) {
			int speed = Goal.instance.GetSpeed ();
			int direction = Goal.instance.GetDirection ();

			// Debug.Log("obstacle speed " + speed);
			transform.RotateAround (Main.instance.goal.transform.position, new Vector3 (0, 0, 1), speed * Time.deltaTime * direction);
			transform.LookAt (Main.instance.goal.transform, new Vector3 (0, 0, -90));
			transform.rotation = new Quaternion (0, 0, transform.rotation.z, transform.rotation.w);

			m_currentPosition = transform.position;
		} else {
			transform.position = m_currentPosition;
		}
	}
}