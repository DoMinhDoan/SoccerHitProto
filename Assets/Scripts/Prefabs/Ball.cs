using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	public float m_Speed = 100f;

	private int m_number;
	private bool m_rotateAround;
	private bool m_reverse = false;
	private bool m_canMove = false;
	private float m_xPosition;

	public bool CanMove() {
		return m_canMove;
	}

	public void CanMove(bool value) {
		m_canMove = value;
	}

	public void SetNumber(int value) {
		m_number = value;
	}

	public void NextLevel() {
		Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
		m_xPosition = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (!Main.instance.IsGameOver ()) {
			if (m_canMove) {
				if (m_rotateAround) {
					int speed = Goal.instance.GetSpeed ();
					int direction = Goal.instance.GetDirection ();

					transform.RotateAround (Main.instance.goal.transform.position, new Vector3 (0, 0, 1), speed * Time.deltaTime * direction);
				} else {
					transform.Translate (Vector3.up * m_Speed * Time.deltaTime);
					transform.position = new Vector3 (m_xPosition, transform.position.y, 0);
					transform.rotation = new Quaternion ();
				}
			}
		} else {
			if (m_reverse) {
				transform.Translate (Vector3.down * m_Speed * Time.deltaTime);
				transform.position = new Vector3 (m_xPosition, transform.position.y, 0);
				transform.rotation = new Quaternion ();
			}
		}
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.collider.name.Contains ("goal")) {
			AudioManager.instance.PlaySound ("sfx_score_normal");
			m_rotateAround = true;
			if (!m_reverse) {
				Main.instance.NextLevel (m_number);
			}
		} else if (collision.collider.name.Contains ("Ball")) {
			Debug.Log ("Ball.cs Ball GameOver");

			AudioManager.instance.PlaySound ("sfx_crowd_miss_1");
			m_reverse = true;
			Main.instance.GameOver ();
		} else if (collision.collider.name.Contains ("Obstacle")) {
			Debug.Log ("Ball.cs Obstacle GameOver");

			List<string> sounds = new List<string> ();
			sounds.Add ("sfx_defender_hit");
			sounds.Add ("sfx_crowd_miss_1");
			AudioManager.instance.PlaySoundMulti (sounds);

			m_reverse = true;
			Main.instance.GameOver ();
		}
	}
}