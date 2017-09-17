using UnityEngine;
using System.Collections;

public class CollisionDetect : MonoBehaviour {

    float[] LastPos = new float[6] { 0, 0, 0, 0, 0, 0 };
    RobotMotion robotmotion;
    BasicOperate basicTips;//使用新手上路的提示
    ButtonRespond button;
    void Start()
    {
        robotmotion = GameObject.Find("MyMotion").GetComponent<RobotMotion>();
        basicTips = GameObject.Find("MainScript").GetComponent<BasicOperate>();
        button = GameObject.Find("MyButton").GetComponent<ButtonRespond>();
    }

    void Update()
    {
        if (GSKDATA.SoftCurrentMode != "Teach")
        {
            if (GSKDATA.IsCollide && !GSKDATA.Scram) 
            {
                button.OnScramClick();
                robotmotion.AxisPositionSet(LastPos);
                //提示发生碰撞
                StartCoroutine(CollisionTips());
                GSKDATA.IsCollide = false;
            }
        }
    }

    IEnumerator CollisionTips()
    {
        basicTips.TipWindow("发生碰撞",true);
        yield return new WaitForSeconds(2.0f);
        basicTips.TipWindow("", false);

    }
    void FixedUpdate()
    {
        if (GSKDATA.SoftCurrentMode != "Teach")
        {
            if (GSKDATA.OpenCollideFunc)
            {
                if (!GSKDATA.IsCollide && !GSKDATA.Scram)
                {
                    LastPos = robotmotion.CurrentAngle_All();
                }
            }
        }
        
    }
}
