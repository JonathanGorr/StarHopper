using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour {

	private Text scoreText;
	private Text partsText;
	private LevelManager manager;

	void Awake()
	{
		manager = GetComponent<LevelManager>();

		if(Application.loadedLevelName != "Title")
		{
			scoreText = transform.Find("UI/GameUI/Score").GetComponent<Text>();
			partsText = transform.Find("UI/GameUI/Parts").GetComponent<Text>();
		}
	}

	void FixedUpdate()
	{
		//display coins
		if(Application.loadedLevelName != "Title")
		{
			scoreText.text = "$" + manager.coins.ToString();

			//display current number of parts + / + parts in level
			//TODO: Count number of parts in scene by using gameobject.findobjectsbytag("parts").count or something;
			partsText.text = manager.parts.ToString() + "/" + manager.maxParts.ToString();
		}
	}
}
