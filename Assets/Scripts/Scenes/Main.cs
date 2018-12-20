using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor.SceneManagement;

public class Main : MonoBehaviour {
	public Image goal;
	public Level[] levels;
	public Rigidbody ballPrefab;
	public Rigidbody obstaclePrefab;
	public GameObject replayPrefab;
    public GameObject homePrefab;
    public Text txtLevel;
	public Text txtBallNumber;
	public float ballDelay = 0.3f;
	public float goalX = 270f;
	public float goalY = 480f;

	public static Main instance;

	private int m_currentBall = -1;
	private int m_currentLevel = 0;
	private bool m_isGameOver = false;
	private bool m_isCanShoot = false;

	private Rigidbody[] m_ballClone;
	private Rigidbody[] m_obstacleClone;
	private GameObject m_replayClone;
    private GameObject m_homeClone;

#if UNITY_STANDALONE
    IEnumerator DelayBeforeStart() {
		yield return new WaitForSeconds(1.0f);
	}
#endif
	IEnumerator DelayAndNextBall() {
		m_isCanShoot = false;
		yield return new WaitForSeconds(ballDelay);
		NextBall ();
		m_isCanShoot = true;
	}

	public bool IsGameOver() {
		return m_isGameOver;
	}

	public int GetCurrentLevel() {
		return m_currentLevel;
	}

	public void GameOver() {
		Debug.Log ("Main.cs GameOver");

        m_isGameOver = true;

        float xPosition = Screen.width / 2;
		float yPosition = Screen.height / 2;
        if(m_replayClone == null)
        {
            m_replayClone = (GameObject)Instantiate(replayPrefab, new Vector3(goalX, goalY, 0), transform.rotation, transform);
            Button btnReplay = m_replayClone.GetComponent<Button>();
            btnReplay.onClick.AddListener(BtnReplayClicked);
        }

        if (m_homeClone == null)
        {
            m_homeClone = (GameObject)Instantiate(homePrefab, new Vector3(goalX, goalY - 200, 0), transform.rotation, transform);
            Button btnHome = m_homeClone.GetComponent<Button>();
            btnHome.onClick.AddListener(BtnHomeClicked);
        }

        m_replayClone.SetActive(true);
        m_homeClone.SetActive(true);
    }

	public void NextBall() {
		Debug.Log ("Main.cs NextBall");
		m_currentBall = m_currentBall + 1;
		txtBallNumber.text = (levels [m_currentLevel].ballNumber - m_currentBall).ToString ();
		if (m_currentBall < levels [m_currentLevel].ballNumber) {
			m_ballClone[m_currentBall] = (Rigidbody)Instantiate (ballPrefab, new Vector3 (Screen.width / 2, 100, 0), transform.rotation, transform);
			m_ballClone [m_currentBall].gameObject.GetComponent<Ball> ().SetNumber (m_currentBall);
		}
	}

	public void NextLevel(int ballNumber) {
		Debug.Log ("Main.cs NextLevel");
		if (ballNumber == levels [m_currentLevel].ballNumber - 1)
        {
            m_currentLevel = m_currentLevel + 1;
            goal.GetComponent<Goal>().SetRotateRule(levels[m_currentLevel]);
			if (m_currentLevel < levels.Length) {
				AudioManager.instance.PlaySound ("sfx_crowd_cheer_1");
				m_currentBall = -1;

				GenerateBall ();
				GenerateObstacle ();
				//NextBall ();
				txtLevel.text = (m_currentLevel + 1).ToString ();
				txtBallNumber.text = (levels [m_currentLevel].ballNumber - m_currentBall).ToString ();
			} else {
				Debug.Log ("Main.cs GameOver");
				GameOver ();
			}
		}
	}

	Vector3 FindPoint(Vector3 c, float r, int i) {
		return c + Quaternion.AngleAxis(i, Vector3.forward) * (Vector3.right * r);
	}

	void Shoot(Rigidbody ball) {
		AudioManager.instance.PlaySound ("sfx_ball_hit_1");
		m_ballClone[m_currentBall].GetComponent<Ball> ().CanMove (true);
		StartCoroutine(DelayAndNextBall());
	}

	void RotateGoal(RotateRule rotateRule) {
		goal.transform.Rotate(new Vector3(0, 0, rotateRule.lenght) * Time.deltaTime * rotateRule.speed1);
	}

	void GenerateBall() {
		Debug.Log ("Main.cs GenerateBall");
		if (m_ballClone != null) {
			for (int i = 0; i < m_ballClone.Length; i++) {
                if(m_ballClone[i])
				    m_ballClone [i].GetComponent<Ball>().NextLevel();
			}
		}
		Rigidbody[] temp = new Rigidbody[levels[m_currentLevel].ballNumber];
		m_ballClone = temp;
	}

	void GenerateObstacle() {
		Debug.Log ("Main.cs GenerateObstacle");
		if (m_obstacleClone != null) {
			for (int i = 0; i < m_obstacleClone.Length; i++) {
				m_obstacleClone [i].GetComponent<Obstacle>().NextLevel();
			}
		}
		Rigidbody[] temp = new Rigidbody[levels[m_currentLevel].obstacles.Length];
		m_obstacleClone = temp;
		for (int index = 0; index < levels [m_currentLevel].obstacles.Length; index++) {
			Vector3 position = FindPoint(goal.transform.position, 150.0f, levels [m_currentLevel].obstacles [index]);
			m_obstacleClone[index] = (Rigidbody)Instantiate (obstaclePrefab, position, transform.rotation, transform);
		}
	}

	void Awake() {
		instance = this;
	#if UNITY_STANDALONE
		Screen.SetResolution(540, 960, false);
		Screen.fullScreen = false;
	#endif
	}

	// Use this for initialization
	void Start () {
	#if UNITY_STANDALONE
		StartCoroutine(DelayBeforeStart());
	#endif
        //NextLevel();

        PopulateList();
    }

    List<string> names = new List<string>() { "Nothing", "Barney", "Wilma", "Betty" };

    public Dropdown m_Dropdown;
    public int m_levelIndex = 0;

    public int m_currentState = 0;

    public GameObject m_ingame;
    public GameObject m_menu;
    public GameObject m_btnStart;

    void DropdownValueChanged(Dropdown change)
    {
        Debug.Log("New Value : " + change.value);
        //m_levelIndex = change.value;
        m_currentLevel = change.value;
    }

    void PopulateList()
    {

        m_Dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(m_Dropdown);
        });

        Button btn = m_btnStart.GetComponent<Button>();
        btn.onClick.AddListener(BtnStartClicked);

        //Fetch the Dropdown GameObject the script is attached to
        //m_Dropdown = GetComponent<Dropdown>();
        //Clear the old options of the Dropdown menu
        m_Dropdown.options.Clear();
        
        foreach (var level in levels)
        {
            m_Dropdown.options.Add(new Dropdown.OptionData(level.name));
        }

        // https://answers.unity.com/questions/1091581/dropdown-will-not-change-selected-item-text-after.html
        int TempInt = m_Dropdown.value;
        m_Dropdown.value = m_Dropdown.value + 1;
        m_Dropdown.value = TempInt;
    }

    public void BtnStartClicked()
    {
        m_currentState = 1; //hard code ingame

        m_menu.SetActive(false);
        m_ingame.SetActive(true);

        m_isCanShoot = true;
        goal.GetComponent<Goal>().SetRotateRule(levels[m_currentLevel]);
        GenerateBall();
        GenerateObstacle();
        NextBall();
        txtLevel.text = (m_currentLevel + 1).ToString();
        txtBallNumber.text = (levels[m_currentLevel].ballNumber - m_currentBall).ToString();
    }

    public void BtnHomeClicked()
    {
        Application.LoadLevel("main");
    }

    public void BtnReplayClicked()
    {
        goal.GetComponent<Goal>().SetRotateRule(levels[m_currentLevel]);
        if (m_currentLevel < levels.Length)
        {
            m_replayClone.SetActive(false);
            m_homeClone.SetActive(false);

            m_currentBall = -1;

            m_isGameOver = false;
            m_isCanShoot = true;
            goal.GetComponent<Goal>().SetRotateRule(levels[m_currentLevel]);
            GenerateBall();
            GenerateObstacle();
            NextBall();
            txtLevel.text = (m_currentLevel + 1).ToString();
            txtBallNumber.text = (levels[m_currentLevel].ballNumber - m_currentBall).ToString();
        }
        else
        {
            Debug.Log("Main.cs GameOver");
            GameOver();
        }
    }


    // Update is called once per frame
    void Update () {
		if (!m_isGameOver) {
			if (Input.GetButtonDown ("Fire1")) {
				if (m_isCanShoot) {
					if (m_currentBall < levels [m_currentLevel].ballNumber) {
						Shoot (m_ballClone [m_currentBall]);
					}
				}
			}
		}
	}
}