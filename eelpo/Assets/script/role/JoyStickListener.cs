using UnityEngine;
using System.Collections;

public class JoyStickListener : MonoBehaviour {

    //方向
    public const int CENTER = 0;
    public const int LEFT = 1;
    public const int RIGHT = 2;
    public const int TOP = 3;
    public const int BOTTOM = 4;

    //指向摇杆
    public EasyJoystick dirctionJoystick;
    public EasyButton jumpButton;
    public EasyButton attButton;

    //摇杆状态
    private int joyStickVDirction;
    private int joyStickHDirction;

    private bool joyStickJump;
    private bool joyStickAtt;

    //虚拟键状态
    private int virtualVDirction;
    private int virtualHDirction;

    private bool virtualJump;
    private bool virtualAtt;

    private Stairs stairs;

	
	void Start () {
        virtualVDirction = joyStickVDirction = CENTER;
        virtualHDirction = joyStickHDirction = CENTER;
        virtualJump = joyStickJump = false;
        virtualAtt = joyStickAtt = false;

        EasyJoystick.On_JoystickMove += OnJoystickMove;
        EasyJoystick.On_JoystickMoveEnd += OnJoystickMoveEnd;
        EasyButton.On_ButtonDown += OnJumpDown;
        EasyButton.On_ButtonUp += jumpUp;
	}

    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        virtualJump = Input.GetButton("Jump") || Input.GetKeyDown("k");
		if(virtualJump)
			print("virtualJump+"+virtualJump);
        virtualAtt = Input.GetButton("Fire1");

        if (hor > 0.1)
        {
            virtualHDirction = RIGHT;
        }
        else if (hor < -0.1)
        {
            virtualHDirction = LEFT;
        }
        else
        {
            virtualHDirction = CENTER;
        }

        if (ver > 0.1)
        {
            virtualVDirction = TOP;
        }
        else if (ver < -0.1)
        {
            virtualVDirction = BOTTOM;
        }
        else
        {
            virtualVDirction = CENTER;
        }
    }

    //摇杆事件
    void OnJoystickMove(MovingJoystick move)
    {
        if (move.joystickName != dirctionJoystick.name)
			return;

        float joyPositionX = move.joystickAxis.x;
        float joyPositionY = move.joystickAxis.y;
        if (joyPositionX > 0.1)
        {
            joyStickHDirction = RIGHT;
        }
        else if (joyPositionX < -0.1)
        {
            joyStickHDirction = LEFT;
        }
        else
        {
            joyStickHDirction = CENTER;
        }
        if (joyPositionY > 0.1)
        {
            joyStickVDirction = TOP;
        }
        else if (joyPositionY < -0.1)
        {
            joyStickVDirction = BOTTOM;
        }
        else
        {
            joyStickVDirction = CENTER;
        }
    }

    void OnTriggerEnter(Collider c)
    {
        Stairs s = c.gameObject.GetComponent<Stairs>();
        if (s)
        {
            stairs = s;
            //Debug.Log("stairs in");
        }
        else
        {
            stairs = null;
        }
    }

    void OnTriggerExit(Collider c)
    {
        Stairs s = c.gameObject.GetComponent<Stairs>();
        if (s)
        {
            stairs = null;
            //Debug.Log("stairs out");
        }
    }

    void OnJoystickMoveEnd(MovingJoystick move)
    {
        if (move.joystickName == dirctionJoystick.name)
        {
            joyStickHDirction = CENTER;
            joyStickVDirction = CENTER;
        }
    }

    void jumpUp(string buttonName)
    {
        if (buttonName == jumpButton.name)
        {
            joyStickJump = false;
        }
        else if (buttonName == attButton.name)
        {
            joyStickAtt = false;
        }
    }

    void OnJumpDown(string buttonName)
    {
        if (buttonName == jumpButton.name)
        {
            joyStickJump = true;
        }
        else if (buttonName == attButton.name)
        {
            joyStickAtt = true;
        }
    }

    public int getHDirction()
    {
        return Mathf.Max(joyStickHDirction,virtualHDirction);
    }

    public int getVDirction()
    {
        return Mathf.Max(joyStickVDirction,virtualVDirction);
    }

    public bool getJump()
    {
        return joyStickJump || virtualJump;
    }

    public bool getAtt()
    {
        return joyStickAtt || virtualAtt;
    }

    public Stairs getStairs()
    {
        return stairs;
    }
}
