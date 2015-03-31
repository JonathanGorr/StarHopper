using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	[HideInInspector]
	public int coins, parts;

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

	private void Awake()
	{
		//Find ui groups
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
		oxygenBar.maxValue = time;

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

			if(gameUI)
				gameUI.SetActive(false);

			if(tutorialUI)
				tutorialUI.SetActive(false);

			if(titleUI)
				titleUI.SetActive(true);
		}
		else if(Application.loadedLevelName == "Intro")
		{
			levelText.text = "";

			if(gameUI)
				gameUI.SetActive(true);
			
			if(tutorialUI)
				tutorialUI.SetActive(true);

			if(titleUI)
				titleUI.SetActive(false);
		}

		//else reveal ui elements
		else
		{
			levelText.text = Application.loadedLevelName.ToString();

			if(gameUI)
				gameUI.SetActive(true);
			
			if(tutorialUI)
				tutorialUI.SetActive(false);
			
			if(titleUI)
				titleUI.SetActive(false);
		}

		//set these off by default
		if(rocketMenu)
			rocketMenu.SetActive(false);

		if(pauseMenu)
			pauseMenu.SetActive(false);

		if(deathMessage)
			deathMessage.text = "";

		//framerate
		Application.targetFrameRate = 60;
	}

	void Update()
	{
		//restart
		if(Input.GetKeyDown(KeyCode.R))
			Restart();

		//esc to pause
		if(Input.GetKeyDown(KeyCode.Escape))
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

		switch((int) oxygenBar.value)
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

		if(mapOn)
			miniMap.SetActive(true);
		else
			miniMap.SetActive(false);
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
