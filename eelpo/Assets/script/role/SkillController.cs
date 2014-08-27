using UnityEngine;
using System.Collections;

public class SkillController : MonoBehaviour {

    public const int NO_COMBAT = 0;//无攻击状态
    public const int ATTACK = 1;//普通攻击
    public const int SKILL_ATTACK = 2;//技能攻击
    public const int BE_ATTACK = 3;//被攻状态

    //普通工具冷却时间
    public float attackCooling = 0.8f;
    //普通攻击连击限定时间
    public float attackLimit = 1.0f;
    //普通攻击连击次数
    public int attackDoubleHit = 0;
    //击中连接
    public int doubleHit = 0;


    private JoyStickListener _jsl;

    private int _roleState;
    private int _doubleHit;

    private float _lastAttTimes;

	private Animator animator;

	private int curComboCount;

	void Start () {
        _doubleHit = 0;
        _roleState = NO_COMBAT;
        _jsl = GetComponent<JoyStickListener>();
		animator = GetComponent<Animator>();
	}
	
	void Update () {
		AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
		if(stateinfo.IsName("Base Layer.waiting"))
			print("aaaaaaaa");
		if(_jsl.getAtt() ){
			if(stateinfo.nameHash == Animator.StringToHash("Base Layer.waiting") && stateinfo.normalizedTime > 0.1f )
				_doubleHit = 1;
			if( stateinfo.nameHash == Animator.StringToHash("Base Layer.attack1") && stateinfo.normalizedTime > 0.4f )
				_doubleHit = 2;
			if( stateinfo.nameHash == Animator.StringToHash("Base Layer.attack2") && stateinfo.normalizedTime > 0.4f )
				_doubleHit = 3;
			if(stateinfo.nameHash == Animator.StringToHash("Base Layer.attack3") && stateinfo.normalizedTime > 0.4f )
				_doubleHit = 1;
		}else{
			_doubleHit = 0;
		}
	}

    //当前状态
    public int getState()
    {
        return _roleState;
    }

    //普通攻击连击数
    public int getAttackDoubleHit()
    {
        return _doubleHit;
    }
}
