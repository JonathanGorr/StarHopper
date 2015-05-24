using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	[HideInInspector] public int coins, parts;

	[HideInInspector] public GameObject[] maxParts;

	private Slider oxygenBar;

	//variables
	public float 
		time = 60f,
		fadeSpeed = 1f;

	//bools
	private bool
		mapOn = true;

	[HideInInspector] public bool paused;

	//UI
	private GameObject
		pauseMenu,
		rocketMenu,
		miniMap,
		titleUI,
		gameUI,
		tutorialUI,
		rocketMenuChoices,
		controls;

	private Text
		timer,
		timeLeft,
		deathMessage,
		rocketText,
		levelText,
		scoreText,
		partsText;

	//components
	private PlayerInput input;
	private Fading fader;

	private void Awake()
	{
		//Find ui groups
		fader = GetComponent<Fading> ();
		input = GetComponent<PlayerInput> ();
		gameUI = GameObject.Find("GameUI");
		titleUI = GameObject.Find("TitleUI");
		tutorialUI = GameObject.Find("TutorialUI");
		pauseMenu = GameObject.Find("PauseMenu");
		rocketMenu = GameObject.Find("RocketMenu");
		miniMap = GameObject.Find("MiniMapPanel");
		oxygenBar = GameObject.Find("Oxygen").GetComponent<Slider>();
		timer = GameObject.Find("Timer").GetComponent<Text>();
		timeLeft = GameObject.Find("TimeLeft").GetComponent<Text>();
		deathMessage = GameObject.Find("DeathMessage").GetComponent<Text>();
		rocketText = rocketMenu.GetComponentInChildren<Text>();
		rocketMenuChoices = GameObject.Find("Choices");
		levelText = GameObject.Find("LevelText").GetComponent<Text>();
		controls = GameObject.Find("Controls");

		//if on the titleScreen, hide most
		if(Application.loadedLevelName == "Title")
		{
			levelText.text = "";

			gameUI.SetActive(false);
			tutorialUI.SetActive(false);
			titleUI.SetActive(true);
		}
		else if(Application.loadedLevelName == "Intro")
		{
			levelText.text = "";

			gameUI.SetActive(true);
			tutorialUI.SetActive(true);
			titleUI.SetActive(false);
		}
		//else reveal ui elements
		else
		{
			levelText.text = Application.loadedLevelName.ToString();

			gameUI.SetActive(true);
			tutorialUI.SetActive(false);
			titleUI.SetActive(false);
		}

		//set these off by default
		rocketMenu.SetActive(false);
		pauseMenu.SetActive(false);
		deathMessage.text = "";

		//framerate
		Application.targetFrameRate = 60;

		//ScoreText-----
		if(Application.loadedLevelName != "Title")
		{
			scoreText = transform.Find("UI/GameUI/Score").GetComponent<Text>();
			partsText = transform.Find("UI/GameUI/Parts").GetComponent<Text>();
		}
	}

	void Start()
	{
		maxParts = GameObject.FindGameObjectsWithTag ("Part");
	}

	void Update()
	{
		//if not on mobile, hide touch controls
		controls.SetActive (Application.platform != RuntimePlatform.WindowsPlayer ? false : true);

		//restart
		if(input.restart) StartRestart();

		//esc to pause
		if(input.pause)
		{
			//toggle
			paused = !paused;

			if(paused) Pause();
			else UnPause();
		}
	}

	void FixedUpdate()
	{
		//oxygen bar
		oxygenBar.value = time -= Time.deltaTime;

		//timer
		timer.text = Mathf.Round(oxygenBar.value).ToString();

		//display coins and parts
		if(Application.loadedLevelName != "Title")
		{
			scoreText.text = "$" + coins.ToString();
			
			//display current number of parts + / + parts in level
			//TODO: Count number of parts in scene by using gameobject.findobjectsbytag("parts").count or something;
			partsText.text = parts.ToString() + "/" + maxParts.Length.ToString();
		}

		//parts
		if(parts < maxParts.Length)
		{
			rocketMenuChoices.SetActive(false);
			rocketText.text = "You need " + PartsLeft() + " more parts.";
		}
		else
		{
			rocketMenuChoices.SetActive(true);
			rocketText.text = "Leave Solar System?";
		}

		//Timeleft------------------------------------------
		switch((int) time)
		{
			case 30:
				timeLeft.text = "30 Seconds Left!";
				break;
			case 15:
				timeLeft.text = "15 Seconds Left!";
				break;
			case 10:
				timeLeft.text = "10 Seconds Left!";
				break;
			case 5:
				timeLeft.text = "5 Seconds Left!";
				break;
			default:
				timeLeft.text = "";
				break;
		}

		//kill state
		if(time <= 0)
			Kill ();
	}

	public void Kill()
	{
		deathMessage.text = "You asphyxiated!";
		GetComponent<PlayerInput> ().enabled = false;
		Invoke("StartRestart", 2f);
	}

	public void Pause()
	{
		paused = true;
		pauseMenu.SetActive(true);
		Time.timeScale = 0;
	}

	public void UnPause()
	{
		paused = false;
		pauseMenu.SetActive(false);
		Time.timeScale = 1;
	}

	public void AddToScore(string name)
	{
		if(name == "Coins")
			coins ++;
		else if(name == "Parts")
			parts++;
	}

	//calculate how many more parts are needed
	int PartsLeft()
	{
		int partsLeft = maxParts.Length - parts;
		return partsLeft;
	}

	public void Depart()
	{
		rocketMenu.SetActive(true);
	}

	public void Suppress()
	{
		rocketMenu.SetActive(false);
	}

	public void ToggleMap()
	{
		mapOn = !mapOn;

		miniMap.SetActive(mapOn ? true : false);
	}

	public void StartGoToMenu()
	{
		StartCoroutine ("GoToMenu");
	}

	public void StartRestart()
	{
		StartCoroutine ("Restart");
	}

	public void StartNextLevel()
	{
		StartCoroutine ("NextLevel");
	}

	public void StartQuit()
	{
		StartCoroutine ("Quit");
	}

	IEnumerator GoToMenu()
	{
		UnPause();
		fader.BeginFade (1);
		yield return new WaitForSeconds (fader.fadeSpeed);
		Application.LoadLevel("Title");
	}

	IEnumerator Restart()
	{
		UnPause();
		fader.BeginFade (1);
		yield return new WaitForSeconds (fader.fadeSpeed);
		Application.LoadLevel(Application.loadedLevel);
	}

	IEnumerator NextLevel()
	{
		UnPause();
		fader.BeginFade (1);
		yield return new WaitForSeconds (fader.fadeSpeed);
		//if there is another level, load it, otherwise go to menu or, ideally, game over screen
		if(Application.CanStreamedLevelBeLoaded(Application.loadedLevel + 1))
			Application.LoadLevel(Application.loadedLevel + 1);
		else
			Application.LoadLevel("Title");
	}

	IEnumerator Quit()
	{
		UnPause();
		fader.BeginFade (1);
		yield return new WaitForSeconds (fader.fadeSpeed);
		Application.Quit();
	}
}
