using UnityEngine;
using System.Collections;
public class blood_bar : MonoBehaviour {
	public GameObject _bloodBar;//获取血条信息。
	public GameObject MainCamera;//获取主摄像机       
	public UILabel nameLab;

	void Start(){
		nameLab.text = "amazing boy";
	}
	void Update () {
		_bloodBar.transform.position=new Vector3(transform.position.x,transform.position.y+3.0f,transform.position.z);//让血条信息一直处于人物的头顶某处
		Vector3 v=transform.position-MainCamera.transform.position;
		Quaternion rotation = Quaternion.LookRotation(v);
		_bloodBar.transform.rotation = rotation;//让血条一直面向摄像机。由于摄像机是以人物为目标的，所以v应该为人物的位置到摄像机位置的向量，否则信息栏会出现偏差。
	}
}
