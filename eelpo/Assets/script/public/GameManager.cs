using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	//public  Controller2D controller2D;
	
	public Texture playerHealthTexture;
	
	public float screenPositionX;
	
	public float screenPositionY;
	
	public int iconSizeX = 25;
	
	public int iconSizeY = 25;
	
	public int playerHealth = 3;
	
	public int curExp = 0;
	
	int maxExp = 50;
	
	int level = 1;
	
	bool playerStatus;

	GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");  
	}
	
	// Update is called once per frame
	void Update (){
		if(curExp > maxExp){
			levelUP();
		}
		
		if(Input.GetKeyDown(KeyCode.C)){
			playerStatus = !playerStatus;
		}
		//if(playerStatus){
			//statsDisplay.text = "Level:"+level+"\nExp"+curExp+"/"+maxExp;
		//}else{
			//statsDisplay.text = "";
		//}
		
	}
	
	void levelUP(){
		curExp = 0;
		maxExp = maxExp +50;
		level ++;
		
		playerHealth ++;
	}
	
	void OnGUI(){
		return;
		for(int h = 0; h<playerHealth;h++){
			GUI.DrawTexture(new Rect(screenPositionX + (h * iconSizeX),screenPositionY,iconSizeX,
				iconSizeY),playerHealthTexture,ScaleMode.ScaleToFit,true,0);
		}
	}
	
	void PlayerDamaged(int damage){
		if(player.renderer.enabled){
			if(playerHealth > 0){
				playerHealth -= damage;
			}
			if(playerHealth <= 0){
				RestartScene();
			}
			
		}
	}
	void RestartScene(){
		Application.LoadLevel(Application.loadedLevel);
	}
}
