//<summary>
//Camerascript#FILEEXTENSION#
//#PROJECTNAME#
//Created by #SMARTDEVELOPERS# on #CREATIONDATE#.
//Company: Sunnytech
//Function:
//添加到CameraFree相机
//
//<summary>
using UnityEngine;
using System.Collections;
using System.Data;
using System.Linq;
using System;
using System.Text.RegularExpressions;

public class Camerascript : MonoBehaviour
{

	// Use this for initialization
    GameObject Factory;//整个工厂
    GameObject Scene1;//场景1
    GameObject Xipan1;//吸盘
    GameObject Pen1;
    GameObject BigPaws;
    GameObject Scene2;
    GameObject Robot1;
    GameObject Scene3;
    GameObject Scene3_6;
    GameObject Scene3_table;
    GameObject Scene4;
    GameObject Scene5;
    GameObject Scene6;
    GameObject Scene10;//结构认知的场景
    GameObject axisGuid;//机器人引导箭头
    GameObject origianlWorkpiece;//加工之前的工件
    GameObject ConveyorBelt;//传送带
    GameObject TeXiao;//特效
    Transform upDownTrans;
    Transform rightLeftTrans;
    string ExcelPath;
    RobotMotion robotmove;
    string axisname = "";
    GameObject[] Rays = new GameObject[6];
    Camera panelcamera;
    bool mouseIn = false;


    //相机操作时的参数
    Vector3 axe;  //旋转轴参数（不用修改）
	private const float MOVE_RATIO = 1f;  //移动速率控制系数（摄像机缩放时，移动速率修正，一般不用改，可微调）
	private const float SCALE_RATIO = 1f;  //缩放速率控制系数（摄像机缩放时，缩放速率修正，一般不用改，可微调）
	private float rotateFactor = 7f;  //旋转速率调节(主要调这个)
	private float scaleFactor = 1.5f;  //缩放速率调节(主要调这个)
	private float moveFactor = 2f;  //移动速率调节(主要调这个)

   
	//JXL
	private bool _mobileMotion = false;
	public float xValue = 0;
	public float yValue = 0;
	public float zoomDis = 0;
	public float xMove = 0;
	public float yMove = 0;
	
	//JXL
	public void MobileMotion (bool state)
	{
		_mobileMotion = state;
		if (!state)
		{
			xValue = 0;
			yValue = 0;
			zoomDis = 0;
			xMove = 0;
			yMove = 0;
		}
	}
	
	//JXL
	public void RotateMotion (float x, float y)
	{
		xValue = x;
		yValue = y;
	}
	
	//JXL
	public void ZoomMotion (float dis)
	{
		zoomDis = dis;
	}
	
	//JXL
	public void MoveMotion (float x, float y)
	{
		xMove = x;
		yMove = y;
	}
	
	
	void RotateRemote (float x, float y)
	{
		axe.y = -y * rotateFactor;
		axe.x = x * rotateFactor;
		camera.transform.parent = upDownTrans;
		upDownTrans.Rotate(new Vector3(axe.y, 0, 0)); //上下旋转
		camera.transform.RotateAround(rightLeftTrans.position, Vector3.up, axe.x);  //左右旋转
		camera.transform.parent = null;
		upDownTrans.rotation = camera.transform.rotation;  //角度修正
	}
	
	void ZoomRemote (float dis)
	{
		float delta_z;
		delta_z = dis * scaleFactor * camera.orthographicSize / SCALE_RATIO * 0.3f;
		
		if (camera.orthographicSize > 0.02f || delta_z > 0)
		{
			camera.orthographicSize += delta_z;
		}
		else
			camera.orthographicSize = 0.02F;
		
		if (camera.orthographicSize < 0.02f)
		{
			camera.orthographicSize = 0.02F;
		}
	}
	
	void MoveRemote (float x, float y)
	{
		float delta_x, delta_y;
		delta_x = x * moveFactor * camera.orthographicSize * MOVE_RATIO;
		delta_y = y * moveFactor * camera.orthographicSize * MOVE_RATIO;
		camera.transform.Translate(new Vector3(-delta_x, -delta_y, 0) * Time.deltaTime, Space.Self);
	}

	void Awake()
	{
        //
        //float x1 = GameObject.Find("MechanicalArm_16").transform.position.z - GameObject.Find("MechanicalArm_26").transform.position.z;
        //float x2 = GameObject.Find("MechanicalArm_26").transform.position.y - GameObject.Find("MechanicalArm_36").transform.position.y;
        //float x3 = GameObject.Find("MechanicalArm_36").transform.position.z - GameObject.Find("MechanicalArm_56").transform.position.z;
        //float x4 = GameObject.Find("MechanicalArm_36").transform.position.y - GameObject.Find("MechanicalArm_56").transform.position.y;

        //Debug.Log(x1 + "," + x2 + "," + x3 + "," + x4);

        AddScript();
	}


	void Start () {
        panelcamera = GameObject.Find("CameraUI").GetComponent<Camera>();
        Rays[0] = GameObject.Find("ray1");
        Rays[1] = GameObject.Find("ray2");
        Rays[2] = GameObject.Find("ray3");
        Rays[3] = GameObject.Find("ray4");
        Rays[4] = GameObject.Find("ray5");
        Rays[5] = GameObject.Find("ray6");
        Closeray();
        TeXiao = GameObject.Find("texiao");
        Factory = GameObject.Find("Model");
        Scene1 = GameObject.Find("scene1");
        Xipan1 = GameObject.Find("xipan1");
        Pen1 = GameObject.Find("penpen");
        BigPaws = GameObject.Find("MechanicalArm_63_2");
        Scene3_6 = GameObject.Find("MechanicalArm_63");
        Scene3_table = GameObject.Find("scx17_1");
        Scene2 = GameObject.Find("scene2");
        Scene5 = GameObject.Find("scene5");
        Scene6 = GameObject.Find("scene6");
        Robot1 = GameObject.Find("Robot_01");
        Scene3 = GameObject.Find("scene3");
        Scene4 = GameObject.Find("scene4");
        Scene10 = GameObject.Find("scene0");
        axisGuid = GameObject.Find("AxisTips");
        upDownTrans = GameObject.Find("upDownTrans").GetComponent<Transform>();
        rightLeftTrans = GameObject.Find("rightLeftTrans").GetComponent<Transform>();
        ExcelPath = Application.dataPath + "/StreamingAssets/Excel/MyCamera.xls";
        this.camera.enabled = false;//自由相机不工作
        axisGuid.SetActive(false);
        robotmove = GameObject.Find("MyMotion").GetComponent<RobotMotion>();
        origianlWorkpiece = GameObject.Find("scx0261");
        ConveyorBelt = GameObject.Find("chuansongdai35");
        TeXiao.SetActive(false);
        BigPaws.SetActive(false);
        Scene3_table.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () 
    {
        //手动引导机器人的射线检测部分
        if (this.camera.enabled)
        {
            string name="";
            RaycastHit hit;
            Ray ray = this.camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log("object");
                name = hit.transform.name;
                if (name == "Axis_X" || name == "Axis_Y" || name == "Axis_Z")
                {
                    Debug.Log("axis");

                    if (Input.GetMouseButtonDown(0))
                    {
                        axisname = name;
                        Debug.Log("drag");
                        
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                axisname = "";
            }
            if (axisname != "")
            {
                axe.y = Input.GetAxis("Mouse Y");
                axe.x = Input.GetAxis("Mouse X");
                AxisGuid(axisname, new Vector2(axe.x, axe.y));
            }

        }
        //焊接特效部分
        if (GSKDATA.Scene_NO == 4&&this.camera.enabled&&GSKDATA.Weld)
        {
            TeXiao.SetActive(true);
        }
        else
        {
            TeXiao.SetActive(false);
        }


        if(panelcamera.enabled)
        {
            RaycastHit hhit;
            Ray rray = panelcamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(rray,out hhit))
            {
                //Debug.Log(hhit.transform.name);
                if (hhit.transform.name == "UI Root 1")
                {
                    mouseIn = true;
                }
                else
                {
                    mouseIn = false;
                }
            }
            else
            {
                mouseIn = false;
            }
        }
        else
        {
            mouseIn = false;
        }
	}

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isMouse && e.clickCount >= 2 && e.type == EventType.mouseDown && !mouseIn)
        {
            if(panelcamera.enabled&&GSKDATA.SoftCurrentMode!="Teach")
            {
                panelcamera.enabled = false;
            }
            else
            {
                panelcamera.enabled = true;
            }
        }
    }

    //显示射线
    public void Ray()
    {
        GSKDATA.RayShow=!GSKDATA.RayShow;
        if(GSKDATA.RayShow)
        {
            for(int i=0;i<5;i++)
            {
                Rays[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                Rays[i].SetActive(false);
            }
        }
        
    }

    public void Closeray()
    {
        for (int i = 0; i < 5; i++)
        {
            Rays[i].SetActive(false);
        }
     }

    public void Showray()
    {
        if (GSKDATA.RayShow)
        {
            for (int i = 0; i < 5; i++)
            {
                Rays[i].SetActive(true);
            }
        }
    }

    //切换相机
    public void SwitchCamera()
    {
        
        if (this.camera.enabled)
        {
            ReturnToScene();
        }
        else
        {
            //显示选择场景窗口
            if(GSKDATA.Scene_NO == 0)
            {

            }
            else
            {
                
                FocusOnScene();
            }
        }
        
    }

    //正交相机位置确定
    public void CameraPosition()
    {
        ExcelOperator excelClass = new ExcelOperator();
        DataTable camera_data = new DataTable("cameraTable");
        switch (GSKDATA.Scene_NO)
        {
            case 10:
                upDownTrans.position = GameObject.Find("Shape046").GetComponent<Transform>().position;
                rightLeftTrans.position = GameObject.Find("Shape046").GetComponent<Transform>().position;
                camera_data = excelClass.ExcelReader(ExcelPath, "case10");
                break;
            case 1:
                upDownTrans.position = GameObject.Find("Shape12").GetComponent<Transform>().position;
                rightLeftTrans.position = GameObject.Find("Shape12").GetComponent<Transform>().position;
                camera_data = excelClass.ExcelReader(ExcelPath, "case1");
                break;
            case 2:
                upDownTrans.position = GameObject.Find("Shape12").GetComponent<Transform>().position;
                rightLeftTrans.position = GameObject.Find("Shape12").GetComponent<Transform>().position;
                camera_data = excelClass.ExcelReader(ExcelPath, "case2");
                break;
            case 3:
                upDownTrans.position = GameObject.Find("scene3").GetComponent<Transform>().position;
                rightLeftTrans.position = GameObject.Find("scene3").GetComponent<Transform>().position;
                camera_data = excelClass.ExcelReader(ExcelPath, "case3");
                Scene3_table.SetActive(false);
                break;
            case 4:
                upDownTrans.position = GameObject.Find("scene4").GetComponent<Transform>().position;
                rightLeftTrans.position = GameObject.Find("scene4").GetComponent<Transform>().position;
                camera_data = excelClass.ExcelReader(ExcelPath, "case4");
                break;
            case 5:
                upDownTrans.position = GameObject.Find("scene6").GetComponent<Transform>().position;
                rightLeftTrans.position = GameObject.Find("scene6").GetComponent<Transform>().position;
                camera_data = excelClass.ExcelReader(ExcelPath, "case0");
                break;
            case 6:
                upDownTrans.position = GameObject.Find("scene3").GetComponent<Transform>().position;
                rightLeftTrans.position = GameObject.Find("scene3").GetComponent<Transform>().position;
                camera_data = excelClass.ExcelReader(ExcelPath, "case6");
                Scene3_table.SetActive(true);
                break;

        }
        //set position of camera
        this.camera.orthographicSize = Convert.ToSingle(camera_data.Rows[0][1]);
        this.transform.position = StrToVector(camera_data.Rows[0][2]);
        this.transform.eulerAngles = StrToVector(camera_data.Rows[0][3]);
    }
    //显示/关闭路径引导线
    public void SetPathGuid()
    {
        GSKDATA.PathGuid = !GSKDATA.PathGuid;
        //set axisGuid position 
        SetPathGuidPos();
        if (this.camera.enabled)
        {
            axisGuid.SetActive(GSKDATA.PathGuid);
        }
    }
    //关闭路径引导线
    //public void ClosePathGuid()
    //{
    //    GSKDATA.PathGuid = false;
    //    if (this.camera.enabled)
    //    {
    //        axisGuid.SetActive(false);
    //    }
    //}

    public void SetPathGuidPos()//设置路径引导的位置
    {
        switch (GSKDATA.Scene_NO)
        {
            case 1:
                axisGuid.transform.parent = GameObject.Find("TOOLKIT1").transform;
                break;
        }
    }
    //进入焦点模式
    public void FocusOnScene()
    {
        //图标变化
        GameObject.Find("MainScript").GetComponent<HidingMenu>().ObservateBtn(true);
        GameObject.Find("Main Camera").GetComponent<Camera>().enabled = false;
        this.camera.enabled = true;
        if (GSKDATA.PathGuid)
        {
            axisGuid.SetActive(true);
        }
        Showray();
        //gameObject.AddComponent<HighlightingEffect>();
        Factory.SetActive(false);
        Scene1.SetActive(false);
        Scene2.SetActive(false);
        Scene3.SetActive(false);
        Scene4.SetActive(false);
        Scene5.SetActive(false);
        Scene6.SetActive(false);
        Scene10.SetActive(false);
        switch (GSKDATA.Scene_NO)
        {
            case 1:
                Scene1.SetActive(true);
                Robot1.SetActive(true);
                ConveyorBelt.SetActive(false);
                robotmove.DefineJMove("case1");
                showXipan();
                GSKDATA.Painting = false;
                break;
            case 2:
                Scene2.SetActive(true);
                Robot1.SetActive(true);
                ConveyorBelt.SetActive(false);
                robotmove.DefineJMove("case2");
                showPen();
                GameObject.Find("MainScript").GetComponent<PaintScript>().ClearPanel();
                GSKDATA.Painting = true;
                break;
            case 3:
                Scene3.SetActive(true);
                Robot1.SetActive(false);
                ConveyorBelt.SetActive(true);
                GSKDATA.Painting = false;
                BigPaws.SetActive(false);
                Scene3_6.SetActive(true);
                robotmove.DefineJMove("case3"); 
				break;
            case 4:
                Scene4.SetActive(true);
                Robot1.SetActive(false);
                ConveyorBelt.SetActive(false);
				robotmove.DefineJMove("case4"); 
                GSKDATA.Painting = false;
                break;
            case 5:
                Robot1.SetActive(false);
                Scene6.SetActive(true);
                ConveyorBelt.SetActive(false);
                robotmove.DefineJMove("case5");
                GSKDATA.Painting = false;
                break;
            case 6:
                Scene3.SetActive(true);
                Robot1.SetActive(false);
                ConveyorBelt.SetActive(true);
                GSKDATA.Painting = false;
                BigPaws.SetActive(true);
                Scene3_6.SetActive(false);
                robotmove.DefineJMove("case6");
                break;
            case 10:
                Scene10.SetActive(true);
                Robot1.SetActive(false);
                GSKDATA.Painting = false;
                break;
        }
        CameraPosition();//相机位置调整到位

    }

    private void showPen()
    {
        Xipan1.SetActive(false);
        Pen1.SetActive(true);
        Pen1.transform.localPosition = new Vector3(0.5197525f, 0, 0);
        Pen1.transform.localEulerAngles = new Vector3(0, -90, 0);
    }
    private void showXipan()
    {
        Xipan1.SetActive(true);
        Pen1.SetActive(false);
    }
    private void showXipanAndPen()
    {
        Xipan1.SetActive(true);
        Pen1.SetActive(true);
        Pen1.transform.localPosition = new Vector3(-25.71765f, -116.8349f, -2.984238f);
        Pen1.transform.localEulerAngles = new Vector3(-90, -90, 0);
    }

    //进入漫游模式
    public void ReturnToScene()
    {
        
        //图标变化
        GameObject.Find("MainScript").GetComponent<HidingMenu>().ObservateBtn(false);
        this.camera.enabled = false;
        Factory.SetActive(true);
        Scene10.SetActive(true);
        Scene1.SetActive(true);
        Scene2.SetActive(true);
        Scene3.SetActive(true);
        Scene4.SetActive(true);
        Scene5.SetActive(true);
        Scene6.SetActive(true);
        Robot1.SetActive(true);
        showXipanAndPen();
        GameObject.Find("Main Camera").GetComponent<Camera>().enabled = true;
        /*只在camerafree中显示的部件*/
        axisGuid.SetActive(false);
        Closeray();
        switch (GSKDATA.SoftCurrentMode)
        {
            case "Structure":
                ExitStructure();
                break;
            case "FreeOperation":
                GameObject.Find("MainScript").GetComponent<HierarchyMenu>().ExitFreeOperation();
                break;
            case "Teach":
            case "Exercise":
                if(GSKDATA.CaseName!="")
                    GameObject.Find("MainScript").GetComponent<TeachWinow>().ExitCurrentCase();
                GSKDATA.SoftCurrentMode = "FreeOperation";
                break;
        }
    }
    //还原场景//out//in//device
    public void ResetScene()
    {
        switch (GSKDATA.Scene_NO)
        {
            case 1:
                DevicePosition_case1();
                break;
            case 3:
                DevicePosition_case3();
                break;
            case 4:
                DevicePosition_case4();
                break;
            case 6:
                DevicePosition_case6();
                break;
        }
        for(int i = 0; i < GSKDATA.IO_MAX_NUM; i++)
        {
            GSKDATA.InInfo[i] = false;
            GSKDATA.OutInfo[i] = false;
        }
        //GSKDATA.InInfo = new bool[32] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        //GSKDATA.OutInfo = new bool[32] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
    }
    //案例一的周边设备的还原
    private void DevicePosition_case1()
    {
        string ExcelPath = Application.dataPath + "/StreamingAssets/Excel/MyDevice.xls";
        Vector3 position = Vector3.zero;
        Vector3 posture = Vector3.zero;
        string fathername = "";
        string devicename = "";
        ExcelOperator excelClass = new ExcelOperator();
        DataTable DeviceData = new DataTable("DeviceData");
        DeviceData = excelClass.ExcelReader(ExcelPath, "case1");
        
        devicename = DeviceData.Rows[0][0].ToString();
        fathername = DeviceData.Rows[0][1].ToString();
        position = StrToVector(DeviceData.Rows[0][2]);
        posture = StrToVector(DeviceData.Rows[0][3]);
        
        Transform device = GameObject.Find(devicename).GetComponent<Transform>();
        device.parent = GameObject.Find(fathername).GetComponent<Transform>();
        device.localPosition = position;
        device.localEulerAngles = posture;
    }
    //案例三周边设备的还原
    private void DevicePosition_case3()
    {
        string ExcelPath = Application.dataPath + "/StreamingAssets/Excel/MyDevice.xls";
        Vector3 position = Vector3.zero;
        Vector3 posture = Vector3.zero;
        Vector3 scale = Vector3.zero;
        string fathername = "";
        string devicename = "";
        ExcelOperator excelClass = new ExcelOperator();
        DataTable DeviceData = new DataTable("DeviceData");
        DeviceData = excelClass.ExcelReader(ExcelPath, "case3");

        devicename = DeviceData.Rows[0][0].ToString();
        fathername = DeviceData.Rows[0][1].ToString();
        position = StrToVector(DeviceData.Rows[0][2]);
        posture = StrToVector(DeviceData.Rows[0][3]);
        scale = StrToVector(DeviceData.Rows[0][4]);
        if (DeviceData.Rows[0][5].ToString() == "1")
        {
            origianlWorkpiece.SetActive(true);
        }
        else
        {
            origianlWorkpiece.SetActive(false);
        }

        Transform device = GameObject.Find(devicename).GetComponent<Transform>();
        device.parent = GameObject.Find(fathername).GetComponent<Transform>();
        device.localPosition = position;
        device.localEulerAngles = posture;
        device.localScale = scale;
        origianlWorkpiece.SetActive(true);
    }
    //案例四焊缝还原
    private void DevicePosition_case4()
    {
        GameObject.Find("CameraFree").GetComponent<WeldLine>().RemoveWeldLine();
    }
    void AxisGuid(string axis_name, Vector2 mouse_dir)
    {
        Vector3 axis_start = this.camera.WorldToScreenPoint(axisGuid.GetComponent<Transform>().position);
        Vector3 axis_end = this.camera.WorldToScreenPoint(GameObject.Find(axis_name).GetComponent<Transform>().position);
        Vector2 axis_dir = new Vector2(axis_end.x - axis_start.x, axis_end.y - axis_start.y);
        axis_dir = axis_dir.normalized;
        Vector3 robot_move_dir = Vector2.Dot(axis_dir, mouse_dir) * GameObject.Find(axis_name).transform.forward;
        if (axis_name == "Axis_X")
        {
            robot_move_dir = -robot_move_dir;
        }
        else if (axis_name == "Axis_Y")
        {
            robot_move_dir = RotateX(robot_move_dir, false);
        }
        else
        {
            robot_move_dir = RotateX(robot_move_dir, true);
        }
        robotmove.GuidMove(robot_move_dir);
    }

    //案例六周边设备的还原
    private void DevicePosition_case6()
    {
        Debug.Log("device return");
        GameObject.Find("MyMotion").GetComponent<InstantiateObjects>().DestoryBox();
        string ExcelPath = Application.dataPath + "/StreamingAssets/Excel/MyDevice.xls";
        Vector3 position = Vector3.zero;
        Vector3 posture = Vector3.zero;
        Vector3 scale = Vector3.zero;
        string fathername = "";
        string devicename = "";
        ExcelOperator excelClass = new ExcelOperator();
        DataTable DeviceData = new DataTable("DeviceData");
        DeviceData = excelClass.ExcelReader(ExcelPath, "case6");

        devicename = DeviceData.Rows[0][0].ToString();
        fathername = DeviceData.Rows[0][1].ToString();
        position = StrToVector(DeviceData.Rows[0][2]);
        posture = StrToVector(DeviceData.Rows[0][3]);
        scale = StrToVector(DeviceData.Rows[0][4]);

        Transform device = GameObject.Find(devicename).GetComponent<Transform>();
        device.parent = GameObject.Find(fathername).GetComponent<Transform>();
        device.localPosition = position;
        device.localEulerAngles = posture;
        device.localScale = scale;
    }

    //将Excel表中的数据转换成Vector3 
    Vector3 StrToVector(object str)
    {
        string tstr = (string)str;
        Vector3 vect = Vector3.zero;
        tstr = Regex.Replace(tstr, @"\(", "", RegexOptions.IgnoreCase);
        tstr = Regex.Replace(tstr, @"\)", "", RegexOptions.IgnoreCase);
        string[] strs = tstr.Split(',');
        try
        {
            vect.x = Convert.ToSingle(strs[0]);
            vect.y = Convert.ToSingle(strs[1]);
            vect.z = Convert.ToSingle(strs[2]);
        }
        catch
        {
            Debug.LogError("convert the object into vetor3 error");
        }
        return vect;
    }

    //将Vector绕世界坐标x/y轴旋转90度
    Vector3 RotateX(Vector3 original, bool dir)
    {
        Vector3 v = Vector3.zero;
        Quaternion q;
        if (dir)
        {
            q.z = 0; q.y = 0; 
            q.x = Mathf.Sin(45f / 180f * Mathf.PI);
            q.w = Mathf.Cos(45f / 180f * Mathf.PI);
        }
        else
        {
            q.z = 0; q.y = 0; 
            q.x = Mathf.Sin(-45f / 180f * Mathf.PI);
            q.w = Mathf.Cos(-45f / 180f * Mathf.PI);
        }
        return v = q * original;
    }

    //画线
    void OnPostRender()
    {
        if (GSKDATA.PathShow && this.camera.enabled && GSKDATA.SoftCurrentMode == "FreeOperation")
        {
            Material lineMaterial = (Material)Resources.Load(@"LineResources/LineColor/linecolor", typeof(Material));
            GL.PushMatrix();
            lineMaterial.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(Color.green);
            for (int i = 0; i < GSKDATA.VList.Count - 1; i++)
            {
                GL.Vertex3(GSKDATA.VList[i].x, GSKDATA.VList[i].y, GSKDATA.VList[i].z);
                GL.Vertex3(GSKDATA.VList[i + 1].x, GSKDATA.VList[i + 1].y, GSKDATA.VList[i + 1].z);
            }
            GL.End();
            GL.PopMatrix();
        }
    }

    //相机的旋转、缩放、平移等操作
    void LateUpdate()
    {
        if (this.camera.enabled)
        {
            //中键旋转		
            if (Input.GetMouseButton(2) && !Input.GetMouseButton(1))
            {
                axe.y = -Input.GetAxis("Mouse Y") * rotateFactor;
                axe.x = Input.GetAxis("Mouse X") * rotateFactor;
                camera.transform.parent = upDownTrans;
                upDownTrans.Rotate(new Vector3(axe.y, 0, 0)); //上下旋转
                camera.transform.RotateAround(rightLeftTrans.position, Vector3.up, axe.x);  //左右旋转
                camera.transform.parent = null;
                upDownTrans.rotation = camera.transform.rotation;  //角度修正
                if (BasicOperate.Rotate)
                {
                    FuncPara.loopControl = 14;
                    BasicOperate.Rotate = false;
                }
            }
            //中键+右键平移	
            if (Input.GetMouseButton(2) && Input.GetMouseButton(1))
            {
                float delta_x, delta_y;
                delta_x = Input.GetAxis("Mouse X") * moveFactor * camera.orthographicSize * MOVE_RATIO;
                delta_y = Input.GetAxis("Mouse Y") * moveFactor * camera.orthographicSize * MOVE_RATIO;
                camera.transform.Translate(new Vector3(-delta_x, -delta_y, 0) * Time.deltaTime, Space.Self);
                if (BasicOperate.Translate)
                {
                    FuncPara.loopControl = 14;
                    BasicOperate.Translate = false;
                }
            }

            //滚轮放大
            if (Input.GetAxis("Mouse ScrollWheel") != 0 && !Input.GetMouseButton(0) && !mouseIn)
            {
                float delta_z;
                delta_z = Input.GetAxis("Mouse ScrollWheel") * SCALE_RATIO * camera.orthographicSize / SCALE_RATIO;

                if (camera.orthographicSize > 0.02f || delta_z > 0)
                {
                    camera.orthographicSize += delta_z;
                }
                else
                    camera.orthographicSize = 0.02F;

                if (camera.orthographicSize < 0.02f)
                {
                    camera.orthographicSize = 0.02F;
                }
                if (BasicOperate.Scale)
                {
                    FuncPara.loopControl = 14;
                    BasicOperate.Scale = false;
                }
            }

			//JXL
			if (_mobileMotion)
			{
				RotateRemote(xValue, yValue);
				ZoomRemote(zoomDis);
				MoveRemote(xMove, yMove);
			}
        }

        if(mouseIn && panelcamera.enabled)
        {
            float delta_z;
            delta_z = Input.GetAxis("Mouse ScrollWheel") * SCALE_RATIO * panelcamera.orthographicSize / SCALE_RATIO;

            if (panelcamera.orthographicSize > 0.5f || delta_z > 0)
            {
                panelcamera.orthographicSize += delta_z;
            }
            else
                panelcamera.orthographicSize = 0.5F;

            if (panelcamera.orthographicSize < 3.5f || delta_z < 0)
            {
                panelcamera.orthographicSize += delta_z;
            }
            else
                panelcamera.orthographicSize = 3.5F;

            if (panelcamera.orthographicSize < 0.5f)
            {
                panelcamera.orthographicSize = 0.5F;
            }
            if (panelcamera.orthographicSize > 3.5f)
            {
                panelcamera.orthographicSize = 3.5F;
            }
        }
    }

    //显示结构认知
    public void ShowStructure()
    {
        GSKDATA.Scene_NO = 10;
        FocusOnScene();
        FuncPara.outlineOn = true;
        GameObject.Find("MainScript").GetComponent<HidingMenu>().ObservateBtn(true);
        GSKDATA.CaseStage = 0;
        GameObject.Find("MyButton").GetComponent<ButtonRespond>().ExitPanel();
    }
    //退出结构认知
    public void ExitStructure()
    {
        GSKDATA.SoftCurrentMode = "";
        GSKDATA.Scene_NO = 1;
        FuncPara.outlineOn = false;
        //ReturnToScene();
    }
    

    //自动添加脚本
    void AddScript()
    {
        gameObject.AddComponent("RightClickMenu");//鼠标小手脚本
        if (GameObject.Find("MainScript") == null) 
        {
            GameObject MainScript = new GameObject("MainScript");
            MainScript.AddComponent("GUIInitial"); //UI初始化
            MainScript.AddComponent("ComponentsControl"); //部件提示
            MainScript.AddComponent("TipsWindow"); //部件提示
            MainScript.AddComponent("PaintScript"); //绘画
            MainScript.AddComponent("DeviceMove"); //设备运动
            MainScript.AddComponent("HidingMenu");//隐匿式菜单
            MainScript.AddComponent("HierarchyMenu");//层级菜单
            MainScript.AddComponent("BasicOperate");//新手上路
            MainScript.AddComponent("MySystemConfiguration");//系统配置
            MainScript.AddComponent("Introduction");//单个黑板报|关于我们|介绍|注意事项|
            MainScript.AddComponent("CaseIntro");//多个黑板报
            MainScript.AddComponent("TeachWinow");//教学窗口
            MainScript.AddComponent("MySpriteAnimation");//
            MainScript.AddComponent("MyAward");//
            MainScript.AddComponent("ExamControl");//确认结束考试|退出程序|考试信息录入窗口|
            MainScript.AddComponent("EncryptionManager");//加密脚本
            MainScript.AddComponent("TipsWindow");//提示ui脚本
            MainScript.AddComponent("doVoiceExe");//语音脚本
            MainScript.AddComponent("CursorMove_zw");//鼠标小手脚本
            MainScript.AddComponent("DeviceMove");//周边设备运动脚本
            MainScript.AddComponent("CaseClass");//
            MainScript.AddComponent("Case0");//
            MainScript.AddComponent("Case1");// 
            MainScript.AddComponent("Case2");//
            MainScript.AddComponent("Case3");//
            MainScript.AddComponent("Case4");//
            MainScript.AddComponent("Case6");//
            MainScript.AddComponent("ResultDetect");//结果检测脚本
            MainScript.AddComponent("AudioSource");
            MainScript.AddComponent("CollisionDetect"); //pengzhuang
            MainScript.AddComponent("LatheOperate"); //车床操作
        }

        //碰撞传感器的添加
        GameObject ka1_sensor = GameObject.Find("zhua1_sensor");
        ka1_sensor.AddComponent<CollideSensor>();
        ka1_sensor.GetComponent<CollideSensor>().SensorSignal = 1;
        GameObject ka2_sensor = GameObject.Find("zhua2_sensor");
        ka2_sensor.AddComponent<CollideSensor>();
        ka2_sensor.GetComponent<CollideSensor>().SensorSignal = 1;
        GameObject ka3_sensor = GameObject.Find("zhua3_sensor");
        ka3_sensor.AddComponent<CollideSensor>();
        ka3_sensor.GetComponent<CollideSensor>().SensorSignal = 1;
        GameObject zhua1_sensor = GameObject.Find("RB50301_sensor");
        zhua1_sensor.AddComponent<CollideSensor>();
        zhua1_sensor.GetComponent<CollideSensor>().SensorSignal = 2;
        GameObject zhua2_sensor = GameObject.Find("RB50291_sensor");
        zhua2_sensor.AddComponent<CollideSensor>();
        zhua2_sensor.GetComponent<CollideSensor>().SensorSignal = 2;
        GameObject zhua1_sensor2 = GameObject.Find("RB50301_sensor2");
        zhua1_sensor2.AddComponent<CollideSensor>();
        zhua1_sensor2.GetComponent<CollideSensor>().SensorSignal = 2;
        zhua1_sensor2.GetComponent<CollideSensor>().feedback = true;
        GameObject zhua2_sensor2 = GameObject.Find("RB50291_sensor2");
        zhua2_sensor2.AddComponent<CollideSensor>();
        zhua2_sensor2.GetComponent<CollideSensor>().SensorSignal = 2;
        zhua2_sensor2.GetComponent<CollideSensor>().feedback = true;
        GameObject big_sensor_1 = GameObject.Find("63_2_1_sensor");
        GameObject big_sensor_2 = GameObject.Find("63_2_2_sensor");
        big_sensor_1.AddComponent<CollideSensor>();
        big_sensor_1.GetComponent<CollideSensor>().SensorSignal = 1;
        big_sensor_1.GetComponent<CollideSensor>().feedback = true;

    }
}
