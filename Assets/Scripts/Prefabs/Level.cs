using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
	Left,
	Right
}

[System.Serializable]
public class RotateRule {
	public Direction direction;
	public int lenght;
	public int speed1;
}

public class Level : MonoBehaviour {
	public int ballNumber;
	public int[] obstacles;
	public RotateRule[] rotateRules;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}
}