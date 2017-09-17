//<summary>
//MoveObject#FILEEXTENSION#
//#PROJECTNAME#
//Created by #SMARTDEVELOPERS# on #CREATIONDATE#.
//Company: Sunnytech
//Function:
//运动物体类
//
//<summary>
using UnityEngine;
using System.Collections;

public abstract class DeviceM
{
    public GameObject moveObject;
    public int ControlSignal;//控制信号
    public int SensorSignal = 0;//传感器信号
    public int MoveState = 0;//0代表暂停；1代表正向；-1代表反向；
    public DeviceM()
    {

    }
    public abstract void DMove();
    public abstract void CheckIn();//检查触发信号
}

//设备直线运动类
public class DeviceLine : DeviceM
{
    public float speed;
    bool worldspace = false;//是否以世界坐标为参考
    Vector3 direct;//运动方向
    float posMax;
    float posMin;
    bool signalfalse; //signal 为false 时启动
    public DeviceLine(string name,int signal, Vector3 dir, float s, float max,float min,bool signal_false = false)
    {
        moveObject = GameObject.Find(name);
        //if (moveObject == null)
        //{
        //    Debug.Log(name);
        //}
        ControlSignal = signal;
        direct = dir;
        speed = s;
        posMax = max;
        posMin = min;
        MoveState = 0;
        SensorSignal = 0;
        signalfalse = signal_false;
    }
    //设备运动
    public override void DMove()
    {
        if (MoveState != 0)
        {
            if (positionLimit())
            {
                MoveState = 0;
                return;
            }
            Vector3 move_distance = MoveState * speed * direct;
            if (worldspace)
            {
                moveObject.transform.Translate(move_distance, Space.World);
            }
            else
            {
                moveObject.transform.Translate(move_distance, Space.Self);
            }
        }
    }
    //触发信号检查
    public override void CheckIn()
    {
        if (signalfalse)
        {
            if (GSKDATA.OutInfo[ControlSignal])
            {
                GSKDATA.DeviceSignal[ControlSignal] = 0;
            }
            else
            {
                if (!GSKDATA.SensorSignal[SensorSignal])
                {
                    if (!positionLimit())
                    {
                        GSKDATA.DeviceSignal[ControlSignal] = 1;
                    }
                }
            }
        }
        else
        {
            if (GSKDATA.OutInfo[ControlSignal] && !GSKDATA.SensorSignal[SensorSignal])
            {
                if (!positionLimit())
                {
                    GSKDATA.DeviceSignal[ControlSignal] = 1;
                }
            }
            else
            {
                GSKDATA.DeviceSignal[ControlSignal] = 0;
            }
        }

        switch (GSKDATA.DeviceSignal[ControlSignal])
        {
            case 0:
                MoveState = 0;
                break;
            case 1:
                MoveState = 1;
                break;
        }
    }

    //检测是否超过行程
    private bool positionLimit()
    {
        if (direct == new Vector3(1, 0, 0))
        {
            if (speed > 0)
            {
                if (moveObject.transform.localPosition.x < posMin)
                {
                    moveObject.transform.localPosition = new Vector3(posMin, moveObject.transform.localPosition.y, moveObject.transform.localPosition.z);
                    return true;
                }
                if (moveObject.transform.localPosition.x >= posMax)
                {
                    moveObject.transform.localPosition = new Vector3(posMax, moveObject.transform.localPosition.y, moveObject.transform.localPosition.z);
                    return true;
                }
            }
            else
            {
                if (moveObject.transform.localPosition.x <= posMin)
                {
                    moveObject.transform.localPosition = new Vector3(posMin, moveObject.transform.localPosition.y, moveObject.transform.localPosition.z);
                    return true;
                }
                if (moveObject.transform.localPosition.x > posMax)
                {
                    moveObject.transform.localPosition = new Vector3(posMax, moveObject.transform.localPosition.y, moveObject.transform.localPosition.z);
                    return true;
                }
            }
            
        }
        else if(direct == new Vector3(0, 0, 1))
        {
            if (speed > 0)
            {
                if (moveObject.transform.localPosition.z < posMin)
                {
                    moveObject.transform.localPosition = new Vector3(moveObject.transform.localPosition.x, moveObject.transform.localPosition.y, posMin);
                    return true;
                }
                if (moveObject.transform.localPosition.z >= posMax)
                {
                    moveObject.transform.localPosition = new Vector3(moveObject.transform.localPosition.x, moveObject.transform.localPosition.y, posMax);
                    return true;
                }
            }
            else
            {
                if (moveObject.transform.localPosition.z <= posMin)
                {
                    moveObject.transform.localPosition = new Vector3(moveObject.transform.localPosition.x, moveObject.transform.localPosition.y, posMin);
                    return true;
                }
                if (moveObject.transform.localPosition.z > posMax)
                {
                    moveObject.transform.localPosition = new Vector3(moveObject.transform.localPosition.x, moveObject.transform.localPosition.y, posMax);
                    return true;
                }
            }
           
        }
        return false;
    }
}

//设备旋转运动类
public class DeviceRotate : DeviceM
{
    public float speed;
    Vector3 direct;//运动方向
    float angMax;
    float angMin;
    bool signalfalse; //signal 为false 时启动
    public float x, y, z;
    public DeviceRotate(string name, int signal, Vector3 dir, float s, float max, float min ,bool signalf = false)
    {
        moveObject = GameObject.Find(name);
        ControlSignal = signal;
        direct = dir;
        speed = s;
        angMax = max;
        angMin = min;
        MoveState = 0;
        SensorSignal = 0;
        signalfalse = signalf;
    }
    //设备运动
    public override void DMove()
    {
        if (MoveState != 0)
        {
            if (positionLimit())
            {
                MoveState = 0;
                return;
            }
            Vector3 move_angle = MoveState * speed * direct;
            moveObject.transform.Rotate(move_angle);
            //Debug.Log(1223);
        }
    }
    //触发信号检查
    public override void CheckIn()
    {
        if (signalfalse)
        {
            if (GSKDATA.OutInfo[ControlSignal])
            {
                GSKDATA.DeviceSignal[ControlSignal] = 0;
            }
            else 
            {
                if (!GSKDATA.SensorSignal[SensorSignal])
                {
                    if (!positionLimit())
                    {
                        GSKDATA.DeviceSignal[ControlSignal] = 1;
                    }      
                }
            }
        }
        else
        {
            if (GSKDATA.OutInfo[ControlSignal] && !GSKDATA.SensorSignal[SensorSignal])
            {
                if (!positionLimit())
                {
                    GSKDATA.DeviceSignal[ControlSignal] = 1;
                }
            }
            else
            {
                GSKDATA.DeviceSignal[ControlSignal] = 0;
            }
        }

        switch (GSKDATA.DeviceSignal[ControlSignal])
        {
            case 0:
                MoveState = 0;
                break;
            case 1:
                MoveState = 1;
                break;
        }
    }

    //检测是否超过行程
    private bool positionLimit()
    {
        if (direct == new Vector3(1, 0, 0))
        {
            float x = moveObject.transform.localEulerAngles.x;
            if (x > 180)
            {
                x = x - 360; 
            }

            if (x < angMin)
            {
                moveObject.transform.localEulerAngles = new Vector3(angMin, y, z);
                return true;
            }
            if (x > angMax)
            {
                moveObject.transform.localEulerAngles = new Vector3(angMax, y, z);
                return true;
            }
        }
        return false;
    }
}

//设备父子关系设定类
public class DeviceBelong : DeviceM
{
    public float detect_h_range = 0.025f;
    public float detect_r_range = 0.02f;
    Transform originalfather;//父物体
    GameObject father;//父物体
    GameObject father2;//父物体2
    string tool1 = "xp1";//场景一吸盘
    string tool1_ = "xp2";//场景二吸盘
	string tool3 = "RB50291";
	string tool3_ = "RB50301";
    int case_no;//应用场景，决定tool
    public RangeDetect range_detect;
    public RangeDetect range_detect2;
    public int tool_flag = 0;

    public Vector3 orignalScale = new Vector3(0.1f, 0.1f, 0.1f);
    public Vector3 newScale = new Vector3(10f, 10f, 10f);
    public DeviceBelong(string name, int signal,int scene)
    {
        moveObject = GameObject.Find(name);
        originalfather = moveObject.transform.parent;
        ControlSignal = signal;
        case_no = scene;
        SensorSignal = 0;
        definetool();
    }
    //设备运动
    public override void DMove()
    {
        switch (MoveState)
        {
            case 1:
                switch (tool_flag)
                {
                    case 1:
                        moveObject.transform.parent = father.transform;
                        moveObject.transform.localPosition = new Vector3(0, 0, 0);
                        moveObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                        moveObject.transform.localScale = newScale;
                        break;
                    case 2:
                        moveObject.transform.parent = father2.transform;
                        moveObject.transform.localPosition = new Vector3(0, 0, 0);
                        moveObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                        moveObject.transform.localScale = newScale;
                        break;
                    case 0:
                        moveObject.transform.parent = originalfather;
                        moveObject.transform.localScale = orignalScale;
                        break;
                }
                //Debug.Log("zhuazhua");
                break;
            case 0:
                moveObject.transform.parent = originalfather;
                moveObject.transform.localScale = orignalScale;
                break;
        }
        if (MoveState == 0)
        {
            tool_flag = Mathf.Max(range_detect.InRange(), range_detect2.InRange());
            if (tool_flag == 0)
            {
                range_detect.CloseHighlight();
            }
        }
    }
    //触发信号检查
    public override void CheckIn()
    {
        //Debug.Log(GSKDATA.OutInfo[ControlSignal]);
        //Debug.Log(GSKDATA.SensorSignal[SensorSignal]);
        if (GSKDATA.OutInfo[ControlSignal] && GSKDATA.SensorSignal[SensorSignal])
        {
            GSKDATA.DeviceSignal[ControlSignal] = 1;
            //Debug.Log("tt");
        }
        else
        {
            GSKDATA.DeviceSignal[ControlSignal] = 0;
            //Debug.Log("ff");
        }
        switch (GSKDATA.DeviceSignal[ControlSignal])
        {
            case 0:
                MoveState = 0;
                break;
            case 1:
                MoveState = 1;
                break;
        }
    }

    //根据场景确定tool
    private void definetool()
    {
        switch (case_no)
        {
            case 1:
                father = GameObject.Find(tool1);
                father2 = GameObject.Find(tool1_);
                range_detect = new RangeDetect(moveObject, father, detect_h_range,detect_r_range, 1);
                range_detect2 = new RangeDetect(moveObject, father2, detect_h_range, detect_r_range, 2);
                break;
			case 3:
                father = GameObject.Find("RB5028_tool");
                father2 = GameObject.Find("RB5028_tool");
                range_detect = new RangeDetect(moveObject, father, detect_h_range, detect_r_range, 1);
                range_detect2 = new RangeDetect(moveObject, father2, detect_h_range, detect_r_range, 1);
				break;
            case 4:
                father = GameObject.Find("ka_center");
                father2 = GameObject.Find("ka_center");
                range_detect = new RangeDetect(moveObject, father, detect_h_range, detect_r_range, 1, 1, true);
                range_detect2 = new RangeDetect(moveObject, father2, detect_h_range, detect_r_range, 1, 1, true);
                break;
            case 6:
                father = GameObject.Find("63_2_tool");
                father2 = GameObject.Find("63_2_tool");
                range_detect = new RangeDetect(moveObject, father, detect_h_range, detect_r_range, 1);
                range_detect2 = new RangeDetect(moveObject, father2, detect_h_range, detect_r_range, 1);
				break;
        }
    } 
}

//范围检测类
public class RangeDetect : MonoBehaviour
{
    
    GameObject object_son;//子物体 世界坐标
    GameObject object_father;//父物体 世界坐标+局部坐标
    public float range_h;//高度
    public float range_r;
    float range_angle;//角度
    int range_dir;//范围方向。默认的是ZY方向(1)
    Vector3 v_son = Vector3.zero;
    Vector3 v_father = Vector3.zero;
    float actual_h;//高度差
    float actual_r;//径向偏差
    float actual_r2;//径向偏差
    int tool_flag;//1代表爪一，2代表爪二
    bool fatherlight = false;//

    public RangeDetect(GameObject son, GameObject father, float h, float r, int flag, int dir = 1, bool father_light = false, float a = Mathf.PI/18)
    {
        object_son = son;
        object_father = father;
        range_h = h;
        range_r = r;
        tool_flag = flag;
        range_dir = dir;
        range_angle = a;
        fatherlight = father_light;
    }

    //是否在设定的范围内
    public int InRange()
    {
        getRangeDir();
        //Debug.Log("actual_r:" + actual_r);
        if (actual_r <= range_r && actual_r2 <= range_r)
        {
            //Debug.Log(Vector3.Dot(v_son, v_father) - Mathf.Cos(range_angle));
            if (Vector3.Dot(v_son, v_father) >= Mathf.Cos(range_angle))
            {
                //Debug.Log("actual_h:" + actual_h);
                if (actual_h <= range_h && actual_h > 0)
                {
                    if (fatherlight)
                    {
                        //高亮
                        if (object_father.GetComponent<FlashingController>() == null)
                        {
                            object_father.AddComponent<FlashingController>();
                        }
                    }
                    else
                    {
                        //高亮
                        if (object_son.GetComponent<FlashingController>() == null)
                        {
                            object_son.AddComponent<FlashingController>();
                        }
                    }
                   
                    return tool_flag;
                }
            }
        }
        return 0;
    }

    //关闭高光
    public void CloseHighlight()
    {
        if (fatherlight)
        {
            if (object_father.GetComponent<FlashingController>() != null)
            {
                Destroy(object_father.GetComponent<FlashingController>());
                Destroy(object_father.GetComponent<HighlightableObject>());
            }
        }
        else
        {
            if (object_son.GetComponent<FlashingController>() != null)
            {
                Destroy(object_son.GetComponent<FlashingController>());
                Destroy(object_son.GetComponent<HighlightableObject>());
            }
        }
        
    }

    //获取检测范围的方向
    private void getRangeDir()
    {
        switch (range_dir)
        {
            case 1:
                v_son = object_son.transform.forward;
                v_father = object_father.transform.forward;

                actual_h = Mathf.Abs(Vector3.Dot(object_father.transform.position - object_son.transform.position, v_son));
                actual_r = Mathf.Abs(Vector3.Dot(object_father.transform.position - object_son.transform.position, object_son.transform.up));
                actual_r2 = Mathf.Abs(Vector3.Dot(object_father.transform.position - object_son.transform.position, object_son.transform.right));

                break;

            case 2:
                v_son = object_son.transform.up;
                v_father = object_father.transform.right;

                actual_h = Mathf.Abs(Vector3.Dot(object_father.transform.position - object_son.transform.position, v_son));
                actual_r = Mathf.Abs(Vector3.Dot(object_father.transform.position - object_son.transform.position, object_son.transform.forward));
                actual_r2 = Mathf.Abs(Vector3.Dot(object_father.transform.position - object_son.transform.position, object_son.transform.right));

                break;
        }
    }

}


//传感器
public class Sensor
{
    public int in_signal;
    public Transform DetectObject;
    public Vector3 SensorPosition;
    public Sensor()
    {

    }
    public virtual void CheckIn()
    {

    }
}

//位置传感器
public class DistanceSeneor : Sensor
{
    const float distance = 0.01f;
    ButtonRespond button;
    public DistanceSeneor(int insignal, string objectname, Vector3 position)
    {
        in_signal = insignal;
        DetectObject = GameObject.Find(objectname).GetComponent<Transform>();
        SensorPosition = position;
        button = GameObject.Find("MyButton").GetComponent<ButtonRespond>();
    }

    public override void CheckIn()
    {
        if (Vector3.Distance(DetectObject.localPosition, SensorPosition) <= distance)
        {
            button.SetInfo(in_signal, true);
        }
        else
        {
            button.SetInfo(in_signal, false);
        }
    }
}
