using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	[HideInInspector] public int coins, parts;

	public int maxParts;

	private Slider oxygenBar;
	public float time = 60f;

	public bool mobile;

	private bool
		paused,
		mapOn = true;

	//UI
	private GameObject
		pauseMenu,
		rocketMenu,
		miniMap,
		titleUI,
		gameUI,
		tutorialUI,
		player,
		rocketMenuChoices,
		controls;

	private Text
		timer,
		timeLeft,
		deathMessage,
		rocketText,
		levelText;

	private PlayerInput input;

	private void Awake()
	{
		//Find ui groups
		input = GetComponent<PlayerInput> ();
		player = GameObject.Find("Player");
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

		//set oxygen max to current start supply
		//oxygenBar.maxValue = time;

		//mobile controls toggle
		/*
		if(mobile)
		{
			controls.SetActive(true);
		}
		else
			controls.SetActive(false);
		*/

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
	}

	void Update()
	{
		//if not on mobile, hide touch controls
		if (Application.platform != RuntimePlatform.WindowsPlayer)
			controls.SetActive (false);
		else
			controls.SetActive (true);

		//restart
		if(input.restart)
			Restart();

		//esc to pause
		if(input.pause)
		{
			//toggle
			paused = !paused;

			if(paused)
				Pause();
			else
				UnPause();
		}
	}

	//calculate how many more parts are needed
	int PartsLeft()
	{
		int partsLeft = maxParts - parts;
		return partsLeft;
	}

	void FixedUpdate()
	{
		//oxygen bar
		oxygenBar.value = time -= Time.deltaTime;

		//timer
		timer.text = Mathf.Round(oxygenBar.value).ToString();

		//parts
		if(parts < maxParts)
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
		{
			deathMessage.text = "You asphyxiated!";
			Destroy(player);
			Invoke("Restart", 2f);
		}
	}

	public void Pause()
	{
		pauseMenu.SetActive(true);
		Time.timeScale = 0;
	}

	public void UnPause()
	{
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

		if(mapOn) miniMap.SetActive(true);

		else miniMap.SetActive(false);
	}

	public void Resume()
	{
		UnPause();
	}

	public void GoToMenu()
	{
		UnPause();
		Application.LoadLevel("Title");
	}

	public void Restart()
	{
		UnPause();
		Application.LoadLevel(Application.loadedLevel);
	}

	public void NextLevel()
	{
		UnPause();

		//if there is another level, load it, otherwise go to menu or, ideally, game over screen
		if(Application.CanStreamedLevelBeLoaded(Application.loadedLevel + 1))
			Application.LoadLevel(Application.loadedLevel + 1);
		else
			Application.LoadLevel("Title");
	}

	public void Quit()
	{
		UnPause();
		Application.Quit();
	}
}
