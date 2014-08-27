using UnityEngine;
using System.Collections;

public class HorizontalController : MonoBehaviour {

    //角色状态
    public const int WAITING = 1;
    public const int WALKING = 2;
    public const int FALL = 3;
    public const int JUMPING = 4;
    public const int DOUBLE = 5;
    public const int CLIMB = 6; 
    public const int DIE = 7;

    //速度
    public float _walkingSpeed = 4;
    public float _jumpingHeight = 3;
    public float _gravitySpeed = 20;

    //当前状态
    private int _roleState;
    private int _roleDirection;
    private float _curAccelerated;

    //计时状态状态
    private float _lastJumpTimes;

    //组件
    private Animator _animator;
    private JoyStickListener _jsl;
    private SkillController _sc;
    private CollisionFlags _collisionFlags;
    private CharacterController _controller;

    void Start()
    {
        _roleState = WAITING;
        _roleDirection = JoyStickListener.RIGHT;

        _jsl = GetComponent<JoyStickListener>();
        _sc = GetComponent<SkillController>();
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
	}

	void ShowAttack(){
		//print("guolaile");
	}

    void Update()
    {
        Vector3 movement = new Vector3();

        //死亡则不再执行
        if (_roleState == DIE)
        {
            _animator.SetBool("isDie", true);
            movement = applyGravity(movement);
            _collisionFlags = _controller.Move(movement);
            return;
        }

        //应用位移
        if(_jsl.getHDirction() == JoyStickListener.CENTER)
        {
            if (_roleState == WALKING) _roleState = WAITING;
        }
        else
        {
            if (_roleState == WAITING) _roleState = WALKING;
            movement = applyWalkAround(movement);
        }

        //应用跳跃
        if (_jsl.getJump())
        {
            if (_roleState == JUMPING)
            {
                float t = Time.time - _lastJumpTimes;
                if (t > 0.4 && t < 0.8)
                {
                    _roleState = DOUBLE;
                    _curAccelerated = Mathf.Sqrt(2 * _jumpingHeight * _gravitySpeed);
                }
            }
            else if (_roleState != DOUBLE)
            {
                _roleState = JUMPING;
                _lastJumpTimes = Time.time;
                _curAccelerated = Mathf.Sqrt(2 * _jumpingHeight * _gravitySpeed);
            }
        }

        //应用攻击
        if (_sc.getState() != SkillController.NO_COMBAT)
        {
           
        }

        //应用爬楼
        if (_jsl.getStairs() && _roleState != JUMPING &&
            _sc.getState() == SkillController.NO_COMBAT)
        {
            movement = applyClimb(movement);
        }

        //应用重力
        movement = applyGravity(movement);

        if (_roleState == CLIMB)
        {
            transform.Translate(movement, Space.World);
        }
        else
        {
            _collisionFlags = _controller.Move(movement);
        }
        changeAnimation();
    }


    //切换动画选项
    private void changeAnimation(){
        _animator.SetBool("isJump", _roleState == JUMPING);
        _animator.SetBool("iswalk", _roleState == WALKING);
        _animator.SetBool("isClimb", _roleState == CLIMB);
		if(_sc.getAttackDoubleHit() > 0)
			print("_sc.getAttackDoubleHit()"+_sc.getAttackDoubleHit());
        _animator.SetInteger("attTimes", _sc.getAttackDoubleHit());
    }



    //应用移动
    private Vector3 applyWalkAround(Vector3 movement)
    {
        switch (_jsl.getHDirction())
        {
            case JoyStickListener.RIGHT:
                if (_roleDirection != JoyStickListener.RIGHT)
                {
                    setDirction(JoyStickListener.RIGHT);
                }
                movement.x = _walkingSpeed * Time.deltaTime;
                break;
            case JoyStickListener.LEFT:
                if (_roleDirection != JoyStickListener.LEFT)
                {
                    setDirction(JoyStickListener.LEFT);
                }
                movement.x = -_walkingSpeed * Time.deltaTime;
                break;
        }
        return movement;
    }

    //设置当前方向
    private void setDirction(int dir)
    {
        _roleDirection = dir;
        Vector3 rota = new Vector3();
        rota.y = 180;
        transform.Rotate(rota);
    }

    //应用重力
    private Vector3 applyGravity(Vector3 movement)
    {
        //在楼梯上的时候不受重力影响
        if (_roleState == CLIMB)return movement;
        if(IsGrounded())
        {
            if (_roleState == JUMPING || _roleState == DOUBLE)
            {
                _roleState = WAITING;
            }
            _curAccelerated = 0;
        }else
        {
            _curAccelerated -= _gravitySpeed * Time.deltaTime;
        }
        movement.y += _curAccelerated;
        return movement;
    }

    //应用爬楼
    private Vector3 applyClimb(Vector3 movement)
    {
        Stairs s = _jsl.getStairs();
        Vector3 p = transform.position;
        //正在爬楼  则检查是否超出楼梯两点
        if (_roleState == CLIMB)
        {
            if (_jsl.getVDirction() == JoyStickListener.TOP)
            {
                _animator.speed = 1f;
                movement.y += _walkingSpeed * Time.deltaTime;
            }
            else if (_jsl.getVDirction() == JoyStickListener.BOTTOM)
            {
                _animator.speed = 1f;
                movement.y += -_walkingSpeed * Time.deltaTime;
            }
            else
            {
                _animator.speed = 0;
            }

            //检查是否出楼梯
            if (movement.y + p.y > s.getGlobalTop())
            {
                _animator.speed = 1f;
                _roleState = WAITING;
                movement.y = s.getStairsTop() - p.y;
            }
            else if (movement.y + p.y < s.getGlobalBottom())
            {
                _animator.speed = 1f;
                _roleState = WAITING;
                movement.y = s.getStairsBottom() - p.y;
            }

            //限定不能出楼梯
            if (s.getIsTraverse())
            {
                if (movement.x + p.x < s.getGlobalLeft())
                {
                    movement.x = s.getGlobalLeft() - p.x;
                }
                else if (movement.x + p.x > s.getGlobalRight())
                {
                    movement.x = s.getGlobalRight() - p.x;
                }
            }
            else
            {
                movement.x = 0;
            }
        }
        else
        {
            //开始爬楼
            if (p.y > s.getGlobalTop() && _jsl.getVDirction() == JoyStickListener.BOTTOM)
            {
                _roleState = CLIMB;
                movement.y += s.getGlobalTop() - p.y;
            }
            else if (p.y < s.getGlobalBottom() && _jsl.getVDirction() == JoyStickListener.TOP)
            {
                _roleState = CLIMB;
                movement.y += s.getGlobalBottom() - p.y;
            }
        }
        return movement;
    }

    //是否在地上
    private bool IsGrounded()
    {
        return (_collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

}
