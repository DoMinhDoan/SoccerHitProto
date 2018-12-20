using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	public static Goal instance;

	private int m_speed;
    private int m_direction;
    private float m_angleRotated;
    private List<RotateRule> m_rotateRules = new List<RotateRule>();
    private int m_currentRotateRule = 0;

	public int GetSpeed() {
		return m_speed;
	}

    public int GetDirection()
    {
        return m_direction;
    }

    public void SetRotateRule(Level level)
    {
        Debug.Log("SetRotateRule");
        m_rotateRules.Clear();
        foreach (RotateRule rorateRule in level.rotateRules)
        {
            m_rotateRules.Add(rorateRule);
            Debug.Log("SetRotateRule AddRule");
        }

        
        if (m_rotateRules.Count != 0)
        {
            m_currentRotateRule = 0;
            if (m_rotateRules[m_currentRotateRule].direction == Direction.Left)
            {
                m_direction = 1;
            }
            else
            {
                m_direction = -1;
            }
        }
    }

	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		if (!Main.instance.IsGameOver ())
        {
            UpdateRotateRule();
            transform.Rotate(new Vector3(0,0,Time.deltaTime * m_speed * m_direction));
            m_angleRotated += m_speed * Time.deltaTime;
            //Debug.Log("m_angleRotated " + m_angleRotated);
        }

	}

    void UpdateRotateRule()
    {
        if (m_rotateRules == null || m_rotateRules.Count == 0)
        {
            return;
        }

        //Debug.Log("lenght " + m_rotateRules[m_currentRotateRule].lenght);

        if (m_angleRotated > m_rotateRules[m_currentRotateRule].lenght)
        {
            m_currentRotateRule++;
            m_angleRotated = 0;

            if (m_currentRotateRule >= m_rotateRules.Count)
            {
                m_currentRotateRule = 0;
            }

            if (m_rotateRules[m_currentRotateRule].direction == Direction.Left)
            {
                m_direction = 1;
            }
            else
            {
                m_direction = -1;
            }

        }

        m_speed = m_rotateRules[m_currentRotateRule].speed1;
    }
}