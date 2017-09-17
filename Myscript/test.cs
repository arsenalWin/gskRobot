//<summary>
//test#FILEEXTENSION#
//#PROJECTNAME#
//Created by #SMARTDEVELOPERS# on #CREATIONDATE#.
//Company: Sunnytech
//Function:
//
//
//<summary>
using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
    public Texture2D paletter1;//画板1
	// Use this for initialization
	void Start () {
		Debug.Log("a1:"+(GameObject.Find("MechanicalArm_14").transform.position.x - GameObject.Find("MechanicalArm_24").transform.position.x));
		Debug.Log("a2:"+(GameObject.Find("MechanicalArm_14").transform.position.y - GameObject.Find("MechanicalArm_34").transform.position.y));
		Debug.Log("a3:"+(GameObject.Find("MechanicalArm_34").transform.position.y - GameObject.Find("MechanicalArm_54").transform.position.y));
		Debug.Log("d4:"+(GameObject.Find("MechanicalArm_34").transform.position.x - GameObject.Find("MechanicalArm_54").transform.position.x));
        //paletter1 = (Texture2D)GameObject.Find("xqhh01").GetComponent<MeshRenderer>().materials[1].mainTexture;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        RobotMotion motionss = GameObject.Find("MyMotion").GetComponent<RobotMotion>();
        //if(GUILayout.Button("go"))
        //{
        //    float[] actual = motionss.CurrentAngle_All();
        //    Vector3 StartPoint = motionss.IKA.SolutionOfKinematics(actual);
        //    Quaternion Posture = motionss.IKA.SolutionOfKinematics_posture(actual);
        //    float[] theory = motionss.IKA.AcceptInterPointPosture(StartPoint, Posture, actual);
        //    for (int i = 0; i < 6; i++)
        //    {
        //        Debug.Log("第"+i+"轴的转动角度："+theory[i]);
        //        if (Mathf.Abs(actual[i] - theory[i])>0.5)
        //        {
        //            Debug.Log("机械臂运动学逆解出现错误！！" + "实际角度：" + actual[i] + ",理论角度：" + theory[i]+".");
        //        }
        //    }
        //}
        if (GUILayout.Button("camera"))
        {
            Camerascript cameras = GameObject.Find("CameraFree").GetComponent<Camerascript>();
            cameras.SwitchCamera();
        }
        //if (GUILayout.Button("add scene"))
        //{
        //    GSKDATA.Scene_NO++;
        //}
        if (GUILayout.Button("部件提示功能"))
        {
            FuncPara.componentTips=!FuncPara.componentTips;
        }

        //if (GUILayout.Button("姿态检测"))
        //{
        //    float[] actual = motionss.CurrentAngle_All();
        //    Quaternion Posture = motionss.IKA.SolutionOfKinematics_posture(actual);
        //    Quaternion Posture2 = GameObject.Find("Shape13").GetComponent<Transform>().rotation;
        //    Debug.Log("理论姿态" + Posture.ToString("0.000"));
        //    Debug.Log("实际姿态" + Posture2.ToString("0.000"));
        //}

        if (GUILayout.Button("IO面板"))
        {
            GameObject.Find("MyButton").GetComponent<ButtonRespond>().ShowIOPanel();
        }

        if (GUILayout.Button("示教器面板"))
        {
            GameObject.Find("MyButton").GetComponent<ButtonRespond>().ShowPanel();
        }

        if (GUILayout.Button("返回零点"))
        {
            GameObject.Find("MyMotion").GetComponent<RobotMotion>().ReturnToZero();
        }

        if (GUILayout.Button("视角还原"))
        {
            GameObject.Find("CameraFree").GetComponent<Camerascript>().CameraPosition();
        }

        if (GUILayout.Button("清空画板"))
        {
            GUILine.TextureInitial(paletter1);
        }
        if (GUILayout.Button("画画"))
        {
            GSKDATA.Painting = true;
        }
        if (GUILayout.Button("案例一"))
        {
            GSKDATA.Painting = true;
        }
        if (GUILayout.Button("案例二"))
        {
            GSKDATA.Painting = true;
        }
        if (GUILayout.Button("方向"))
        {
            Transform tooldir = GameObject.Find("Circle0571").GetComponent<Transform>();
            Transform tooldir2 = GameObject.Find("qt208").GetComponent<Transform>();
            Debug.Log(tooldir.forward.ToString("0.00"));
            Debug.Log(tooldir2.forward.ToString("0.00"));
        }
        if (GUILayout.Button("光晕"))
        {
            GameObject.Find("qt208").AddComponent<FlashingController>();
        }
        if (GUILayout.Button("GuidPath"))
        {
            GameObject.Find("CameraFree").GetComponent<Camerascript>().SetPathGuid();
        }
        if (GUILayout.Button("教学"))
        {

        }
        if (GUILayout.Button("练习"))
        {

        }
    }
}
