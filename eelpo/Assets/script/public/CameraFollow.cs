using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;

    public Transform sceneBg;

    public Transform skyBg;
	
	public float xDirectionOffset = 0;
	
	public float yDirectionOffset = 0;
	
	public float zDirectionOffset = 0;

    
    private bool isReady;
    private float marginLeft;
    private float marginRight;
    private float marginTop;
    private float marginBottom;
    private float senceWidth;
    private float senceHeight;

    private float parallaxX;
    private float parallaxY;
    private float parallaxW;
    private float parallaxH;

    private Camera camera;
	
	void Start () {
        isReady = true;
        if (!target) isReady = false;
        if (!sceneBg) isReady = false;
        if (!skyBg) isReady = false;
        if (!isReady) return;

        //场景尺寸
        MeshCollider senceBgSize = sceneBg.gameObject.GetComponent<MeshCollider>();
        float senceWidth = Mathf.Abs(senceBgSize.bounds.size.x);
        float senceHeight = Mathf.Abs(senceBgSize.bounds.size.y);

        camera = GetComponent<Camera>();

        //透视相机(平面有1米的高度)
        //float distance = Mathf.Abs(transform.position.z - sceneBg.position.z) + 0.5f;
        //float angle = camera.fieldOfView;
        //float s = camera.aspect;
        //float w = Mathf.Tan(angle * (Mathf.PI / 180)) * distance;
        //float h = w / s;

        //正交相机
        float s = camera.aspect;
        float h = camera.orthographicSize;
        float w = h * s;

        marginLeft = sceneBg.position.x - (senceWidth / 2) + w;
        marginRight = sceneBg.position.x + (senceWidth / 2) - w;
        marginTop = sceneBg.position.y + (senceHeight / 2) - h;
        marginBottom = sceneBg.position.y - (senceHeight / 2) + h;

        MeshCollider skyBgSize = skyBg.gameObject.GetComponent<MeshCollider>();
        parallaxX = sceneBg.position.x - (senceWidth / 2) + (skyBgSize.bounds.size.x / 2);
        parallaxY = sceneBg.position.y - (senceHeight / 2) + (skyBgSize.bounds.size.y / 2);
        parallaxW = senceWidth - skyBgSize.bounds.size.x;
        parallaxH = senceHeight - skyBgSize.bounds.size.y;
	}
	
	void LateUpdate () {
        if (!isReady) return;
		
		Vector3 newPoint = new Vector3();

		newPoint.x = target.position.x + xDirectionOffset;
		newPoint.y = target.position.y + yDirectionOffset;
		newPoint.z = target.position.z + zDirectionOffset;

        if (newPoint.x < marginLeft)
        {
            newPoint.x = marginLeft;
        }
        else if (newPoint.x > marginRight)
        {
            newPoint.x = marginRight;
        }
        if (newPoint.y < marginBottom)
        {
            newPoint.y = marginBottom;
        }
        else if (newPoint.y > marginTop)
        {
            newPoint.y = marginTop;
        }
		
		transform.position = newPoint;
		//transform.LookAt(target);

        Vector3 pPoint = skyBg.position;
        pPoint.x = parallaxX + parallaxW * (transform.position.x - marginLeft) / (marginRight - marginLeft);
        pPoint.y = parallaxY + parallaxH * (transform.position.y - marginBottom) / (marginTop - marginBottom);
        skyBg.position = pPoint;
	}
}
