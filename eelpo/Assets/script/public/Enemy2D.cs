using UnityEngine;
using System.Collections;

public class Enemy2D : MonoBehaviour {
	public  GameManager gameManager;

	public int unitsToMove = 5;
	
	public int moveSpeed = 2;
	
	public bool basicEnemy;
	
	public bool advancedEnemy;
	
	bool moveRight = true;
	
	int damageValue = 1;
	
	int enemyHeath = 1;
	
	float  startPos;
	float endPos;

	GameObject player;

	JoyStickListener jsl;

	int beAttackTimes ;

	Animator animator;
	/// <summary>
	/// 英雄的状态
	/// </summary>


	void Awake(){
		startPos = transform.position.x;
		endPos = startPos + unitsToMove;
		
		if(basicEnemy){
			enemyHeath = 3;
		}
		if(advancedEnemy){
			enemyHeath = 6;
		}
	}
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		jsl = player.GetComponent<JoyStickListener>();
		animator = player.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(moveRight){
			rigidbody.position += Vector3.right * moveSpeed * Time.deltaTime;
		}
		if(rigidbody.position.x >= endPos){
			moveRight = false;
		}
		if(moveRight == false){
			rigidbody.position -= Vector3.right * moveSpeed * Time.deltaTime;
		}
		if(rigidbody.position.x <= startPos){
			moveRight =   true;
		}
	}
	
	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Player"){
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			if (stateInfo.nameHash == Animator.StringToHash("Base Layer.attack1")
			    || stateInfo.nameHash == Animator.StringToHash("Base Layer.attack2")
			    || stateInfo.nameHash == Animator.StringToHash("Base Layer.attack3")){
				beAttackTimes++;
				print("guolaile");
			}

			if(beAttackTimes > 2)
				Destroy(gameObject);

			//gameManager.SendMessage("PlayerDamaged",damageValue,SendMessageOptions.DontRequireReceiver);
			//gameManager.controller2D.SendMessage("TakenDamage",SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void EnemyDamaged(int damage){
		if(enemyHeath >0){
			enemyHeath -= damage;
		}
		if(enemyHeath <= 0){
			enemyHeath = 0;
			Destroy(gameObject);
			
			gameManager.curExp += 10;
		}
	}
}
