using UnityEngine;
using System.Collections;

public class Stairs : MonoBehaviour {

    //楼梯有效区域距离顶部偏移
    public float marginTop = 1f;
    //楼梯有效区域距离底部偏移
    public float marginBottom = 0.1f;
    //楼梯有效区域距离左边偏移
    public float marginLeft = 0.0f;
    //楼梯有效区域距离右边偏移
    public float marginRight = 0.0f;

    //允许横向移动
    public bool traverse = true;
    //使用自定义爬楼速度
    public bool customSpeed = false;

    private float _stairsHeight;
    private float _stairsWidth;
    private float _globalTop;
    private float _globalBottom;
    private float _globalLeft;
    private float _globalRight;
    private float _stairsTop;
    private float _stairsBottom;

    void Start()
    {
        _stairsHeight = transform.localScale.y;
        _stairsWidth = transform.localScale.x;
        _stairsTop = transform.position.y + (_stairsHeight / 2);
        _stairsBottom = transform.position.y - (_stairsHeight / 2);
        _globalTop = _stairsTop - marginTop;
        _globalBottom = _stairsBottom + marginBottom;
        _globalLeft = transform.position.x - (_stairsWidth / 2) + marginLeft;
        _globalRight = transform.position.x + (_stairsWidth / 2) + marginRight;
    }

    public float getStairsTop()
    {
        return _stairsTop;
    }

    public float getStairsBottom()
    {
        return _stairsBottom;
    }

    public float getStairsHeight()
    {
        return _stairsHeight;
    }

    public float getStairsWidth()
    {
        return _stairsWidth;
    }

    public float getGlobalTop()
    {
        return _globalTop;
    }

    public float getGlobalBottom()
    {
        return _globalBottom;
    }

    public float getGlobalLeft()
    {
        return _globalLeft;
    }

    public float getGlobalRight()
    {
        return _globalRight;
    }

    public bool getIsTraverse()
    {
        return traverse;
    }

}
