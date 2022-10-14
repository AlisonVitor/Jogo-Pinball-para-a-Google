// LeaderboardSystem : Description : Manage a UI interface to save and load score and player name on PlayerPrefs
// This script are used on gameObjects Canvas_Game -> Panel_Leaderboard,leaderboard and Grp_CanvasLeaderboard
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LeaderboardSystem : MonoBehaviour {

	public EventSystem					eventSystem;	
	public StandaloneInputModule		standaloneInputModule;
	//Game_Manager 		obj_Game_Manager;									// Access the Game_Manager component

	public int 			MaxLetter		= 5;								// Max length name
	public string[] 	arrChara; 											// Characters that could be used for a name
	public Text[] 		txt_Center;											// Display character on these ui.text

	public Text 		txt_PlayerName;										// Display the name on this UI.text
	public int 			OffsetLetter 	= 4;								// Use to display A ltter on UI.text name text_Center

	private int 		cmpt 			= 0;								// use calculate the good letter to display
	public int  		StartChara 		= 0;								// Use to display A ltter on UI.text name text_Center

	public Text 		txt_Leaderboard_Score;								// Display Leaderboard on screen

	public GameObject 	obj_PauseButton;									// In Game Pause Button
	public GameObject 	obj_PanelLeaderboard;								// UI Leaderboard panel (Save score in this panel)

	// --> Next variables are used to display score on the scroll menu tha could find on Main menu button Leaderboard
	public Text 		txt_OnLeaderboard_LCD_Name;							// name of the table					
	public Text 		txt_OnLeaderboard_Name;								// Player name	
	public Text 		txt_OnLeaderboard_Position;							// Position on leaderboard
	public Text 		txt_OnLeaderboard_Score;							// Player Score
	public Text 		txt_OnLeaderboard_Loop;								// Not use
	public int 			MaxScoreDisplay = 50;								// Max score display on leaderboard

	// Use this for initialization
	void Start () {																	// --> Initialization

		GameObject TmpEvent = GameObject.Find("EventSystem");
		if(TmpEvent){
			eventSystem = TmpEvent.GetComponent<EventSystem>();
			standaloneInputModule = TmpEvent.GetComponent<StandaloneInputModule>();
		}
		else{
			Debug.Log("Pinball Creator : Info : An EventSystem is needed");}


		for(var i =0;i<txt_Center.Length;i++){
			cmpt = StartChara;
			if(cmpt<0)cmpt=arrChara.Length-1;
			if(txt_Center[i])txt_Center[i] = txt_Center[i].gameObject.GetComponent<Text>();	// Access component
			if(txt_Center[i])txt_Center[i].text = arrChara[(cmpt+i)%arrChara.Length];		// init letter to display
		}

		if(txt_PlayerName)txt_PlayerName = txt_PlayerName.gameObject.GetComponent<Text>();	// init Name
		if(txt_PlayerName)txt_PlayerName.text = "-";										// 

	}

	public void Display_Next_Letter () {											// --> UI Button is pressed.Display Next letter on screen.
		cmpt++;
		cmpt = cmpt%arrChara.Length;
		Display_Letter ();
	}

	public void Display_Last_Letter () {											// --> UI Button is pressed.Display Last letter on screen.
		cmpt--;
		if(cmpt<0)cmpt=arrChara.Length-1;
		Display_Letter ();
	}

	public void Display_Letter () {													// --> Display Letters on screen
		for(var i =0;i<txt_Center.Length;i++){
			txt_Center[i].text = arrChara[(cmpt+i)%arrChara.Length];
		}
	}

	public void AddLetter () {														// --> Add letter to the name
		if(txt_PlayerName.text.Length < MaxLetter){
			if(txt_PlayerName.text == "-")txt_PlayerName.text ="";

			if(arrChara[(cmpt+OffsetLetter)%arrChara.Length] == "")
				txt_PlayerName.text +=" ";
			else
				txt_PlayerName.text += arrChara[(cmpt+OffsetLetter)%arrChara.Length];
		}
	}

	public void SupprLetter () {													// --> UI Button is pressed. Delete Last Letter
		string tmpString = txt_PlayerName.text;
		if(tmpString.Length>1)															// Check if name islonger than one character
			tmpString = tmpString.Substring(0,tmpString.Length -1);	
		else
			tmpString = "-";

		txt_PlayerName.text = tmpString;												// Display text
	}

	public void Validate () {														// --> UI Button is pressed. Validate name and score. Save them on a PlayerPrefs

		Scene scene = SceneManager.GetActiveScene();

		PlayerPrefs.SetString(scene.name+"_Lead",
			PlayerPrefs.GetString(scene.name+"_Lead") + txt_PlayerName.text + "," + 	// Name
			PlayerPrefs.GetInt("CurrentScore") + "," +									// Score (string)
			"" + "," +
			PlayerPrefs.GetInt("CurrentScore") + "," );									// Score (int)

	}
		

	public void UpdateLeaderboard(){												// --> Update the leaderboard 
		List<PlayerScoreCompare> playersScores = new List<PlayerScoreCompare>();		// Create a list 

		GameObject objTmp = GameObject.Find("ScrollMenu_Manager");

		MainMenu MainMenuTmp = objTmp.GetComponent<MainMenu>();

		string text = PlayerPrefs.GetString(MainMenuTmp.SceneName[MainMenuTmp.CurrentScene] +"_Lead");	// Scores and player name record on a string
		string[] textSplit;																// Create an array to split the string

		textSplit = text.Split(","[0]);													// string is split when there is "," character


		for(int i = 0; i < textSplit.Length-1; i++){									// Create the list with name and scores
			//Debug.Log(textSplit[i]);
			if(i%4 == 0)
				playersScores.Add (new PlayerScoreCompare(textSplit[i], textSplit[i+1],textSplit[i+2], int.Parse(textSplit[i+3])));
		}

		playersScores.Sort();															// sort the list
		playersScores.Reverse();														// reverse list	

		if(txt_OnLeaderboard_LCD_Name)txt_OnLeaderboard_LCD_Name.text = MainMenuTmp.TableName[MainMenuTmp.CurrentScene];	
		if(txt_OnLeaderboard_Name)txt_OnLeaderboard_Name.text = "";						// Init text
		if(txt_OnLeaderboard_Score)txt_OnLeaderboard_Score.text = "";					// Init text
		if(txt_OnLeaderboard_Position)txt_OnLeaderboard_Position.text = "";
		if(txt_OnLeaderboard_Loop)txt_OnLeaderboard_Loop.text = "";

		for(int i  = 0; i < playersScores.Count; i++){									// Display all the score on the leaderboard panel
			if(i < MaxScoreDisplay){
				string s_color;
				if(i%2 == 0)s_color = "<color=#FFFFFFFF>";
				else s_color = "<color=#FFFFFFFF>";

				if(txt_OnLeaderboard_Position)txt_OnLeaderboard_Position.text += s_color + (i+1).ToString() + "\n" + "</color>";
				if(txt_OnLeaderboard_Name)txt_OnLeaderboard_Name.text += s_color + playersScores[i].name + "\n" + "</color>";									// Display Nameon on screen.
				if(txt_OnLeaderboard_Score)txt_OnLeaderboard_Score.text += s_color + playersScores[i].score + "\n" + "</color>";								// Display score on screen
				if(txt_OnLeaderboard_Loop)txt_OnLeaderboard_Loop.text += s_color + playersScores[i]._loop + "\n" + "</color>";		
			}
		}	
	}




	string PlayerName(){						// return the player name
		return txt_PlayerName.text;
	}

	int PlayerScore(){							// return the player score
		return PlayerPrefs.GetInt ("CurrentScore");
	}
}
