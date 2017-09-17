using UnityEngine;
using System.Collections;

//右键菜单。，，只在焦点模式下出现
public class RightClickMenu : MonoBehaviour
{
    Rect rightclick_rect; //右键窗口
    float menu_height = 25f; //每个选项的高度
    float menu_subsection = 10f; //分割线的高度

    public const int CAMERA_LEFT = 0;
    public const int CAMERA_RIGHT = 1;
    public const int CAMERA_FACELOOK = 2;
    public const int CAMERA_OVERLOOK = 3;
    public const int PRESET_ONE = 4;
    public const int PRESET_TWO = 5;
    public const int CAMERA_CUSTOM = 6;
    public const int CUSTOM_SETTINGS = 7;
    public const int TOOL_SETTINGS = 8;
    public const int BLANK_SETTINGS = 9;

    int current_state = -1;
    public static bool IsMouseInRightMenu = false;
    // Use this for initialization
    void Start()
    {
        rightclick_rect = new Rect(0, 0, 200f, 150f); //248
        current_state = -1;
    }

    void OnGUI()
    {
        Event mouse_e = Event.current;

        //右键菜单显示
        if (mouse_e.isMouse && mouse_e.type == EventType.MouseDown && mouse_e.button == 1 && mouse_e.button != 2)
        {
            if (GSKDATA.SoftCurrentMode == "Teach"||Input.GetMouseButton(2))
            {
                return;
            }
            //JXL
            if (ServerCenter.InServerMode)
            {
                return;
            }

            if (GSKDATA.SoftCurrentMode != "FreeOperation")
            {
                rightclick_rect = new Rect(0, 0, 200f, 140f);
            }
            else
            {
                rightclick_rect = new Rect(0, 0, 200f, 210f);
            }
            rightclick_rect.x = mouse_e.mousePosition.x;
            rightclick_rect.y = mouse_e.mousePosition.y;
            FuncPara.rightclick_menu_on = true;
        }
        if (!camera.enabled || GSKDATA.SoftCurrentMode == "BasicOperation" || GSKDATA.SoftCurrentMode == "Structure")
        {
            FuncPara.rightclick_menu_on = false;
        }

        //右键菜单消失
        if (mouse_e.isMouse && mouse_e.type == EventType.MouseDown && mouse_e.button != 1)
        {
            FuncPara.rightclick_menu_on = false;
        }

        if (FuncPara.rightclick_menu_on)
        {            
            rightclick_rect = GUI.Window(2, rightclick_rect, RightclickMenu, "", FuncPara.sty_Rightclick);
            GUI.BringWindowToFront(1);
            ///屏蔽右键菜单
            if (rightclick_rect.Contains(mouse_e.mousePosition))
            {
                IsMouseInRightMenu = true;
            }
            else
            {
                IsMouseInRightMenu = false;
            }
        }
        else
        {
            if (mouse_e.isMouse && mouse_e.type == EventType.MouseUp && mouse_e.button == 0)
            {
                IsMouseInRightMenu = false;
            }
        }
    }

    void RightclickMenu(int WindowID)
    {
        Event rightclick_e = Event.current;

        if ((new Rect(0, 0, 200, menu_height).Contains(rightclick_e.mousePosition)))
            current_state = CAMERA_LEFT;
        else if ((new Rect(0, (CAMERA_RIGHT) * menu_height, 200, menu_height).Contains(rightclick_e.mousePosition)))
            current_state = CAMERA_RIGHT;
        else if ((new Rect(0, (CAMERA_FACELOOK) * menu_height, 200, menu_height).Contains(rightclick_e.mousePosition)))
            current_state = CAMERA_FACELOOK;
        else if ((new Rect(0, (CAMERA_OVERLOOK) * menu_height, 200, menu_height).Contains(rightclick_e.mousePosition)))
            current_state = CAMERA_OVERLOOK;
        else if ((new Rect(0, (PRESET_ONE) * menu_height + menu_subsection, 200, menu_height).Contains(rightclick_e.mousePosition)))
            current_state = PRESET_ONE;
       
        else if (GSKDATA.SoftCurrentMode == "FreeOperation")
        {
            if ((new Rect(0, (CAMERA_CUSTOM) * menu_height + menu_subsection, 200, menu_height).Contains(rightclick_e.mousePosition)))
                current_state = CAMERA_CUSTOM;
            else if ((new Rect(0, (CUSTOM_SETTINGS) * menu_height + menu_subsection, 200, menu_height).Contains(rightclick_e.mousePosition)))
                current_state = CUSTOM_SETTINGS;
            else if ((new Rect(0, (PRESET_TWO) * menu_height + menu_subsection, 200, menu_height).Contains(rightclick_e.mousePosition)))
                current_state = PRESET_TWO;
        }
        
        //else if ((new Rect(0, (BLANK_SETTINGS) * menu_height + 2 * menu_subsection, 200, menu_height).Contains(rightclick_e.mousePosition)))
        //    current_state = BLANK_SETTINGS;
        //else if ((new Rect(0, (TOOL_SETTINGS) * menu_height + 2 * menu_subsection, 200, menu_height).Contains(rightclick_e.mousePosition)))
        //    current_state = TOOL_SETTINGS;
        else
            current_state = -1;



        switch (current_state)
        {
            case CAMERA_LEFT:
                GUI.Label(new Rect(0, (CAMERA_LEFT) * menu_height, 194, menu_height), "", FuncPara.sty_RightCursor);
                break;
            case CAMERA_RIGHT:
                GUI.Label(new Rect(0, (CAMERA_RIGHT) * menu_height, 194, menu_height), "", FuncPara.sty_RightCursor);
                break;
            case CAMERA_FACELOOK:
                GUI.Label(new Rect(0, (CAMERA_FACELOOK) * menu_height, 194, menu_height), "", FuncPara.sty_RightCursor);
                break;
            case CAMERA_OVERLOOK:
                GUI.Label(new Rect(0, (CAMERA_OVERLOOK) * menu_height, 194, menu_height), "", FuncPara.sty_RightCursor);
                break;
            case PRESET_ONE:
                GUI.Label(new Rect(0, (PRESET_ONE) * menu_height + menu_subsection, 194, menu_height), "", FuncPara.sty_RightCursor);
                break;
            case PRESET_TWO:
                GUI.Label(new Rect(0, (PRESET_TWO) * menu_height + menu_subsection, 194, menu_height), "", FuncPara.sty_RightCursor);
                break;
            case CAMERA_CUSTOM:
                GUI.Label(new Rect(0, (CAMERA_CUSTOM) * menu_height + menu_subsection, 194, menu_height), "", FuncPara.sty_RightCursor);
                break;
            case CUSTOM_SETTINGS:
                GUI.Label(new Rect(0, (CUSTOM_SETTINGS) * menu_height + menu_subsection, 194, menu_height), "", FuncPara.sty_RightCursor);
                break;
            //case BLANK_SETTINGS:
            //    GUI.Label(new Rect(0, (BLANK_SETTINGS) * menu_height + 2 * menu_subsection, 194, menu_height), "", FuncPara.sty_RightCursor);
            //    break;
            //case TOOL_SETTINGS:
            //    GUI.Label(new Rect(0, (TOOL_SETTINGS) * menu_height + 2 * menu_subsection, 194, menu_height), "", FuncPara.sty_RightCursor);
            //    break;
            default:
                break;
        }


        GUI.Label(new Rect(4, 0, 146, menu_height), "场景还原", FuncPara.sty_RightclickFont);
        //GUI.Label(new Rect(137, 0, 146, menu_height), "Ctrl + ←", FuncPara.sty_RightclickFont);

        GUI.Label(new Rect(4, (CAMERA_RIGHT) * menu_height, 146, menu_height), "机器人返回到零点", FuncPara.sty_RightclickFont);
        //GUI.Label(new Rect(137, (CAMERA_RIGHT) * menu_height, 146, menu_height), "Ctrl + →", FuncPara.sty_RightclickFont);

        GUI.Label(new Rect(4, (CAMERA_FACELOOK) * menu_height, 146, menu_height), "显示示教器", FuncPara.sty_RightclickFont);
        //GUI.Label(new Rect(137, (CAMERA_FACELOOK) * menu_height, 146, menu_height), "Ctrl +  ↑", FuncPara.sty_RightclickFont);

        GUI.Label(new Rect(4, (CAMERA_OVERLOOK) * menu_height, 146, menu_height), "显示I/O面板", FuncPara.sty_RightclickFont);
        //GUI.Label(new Rect(137, (CAMERA_OVERLOOK) * menu_height, 146, menu_height), "Ctrl +  ↓", FuncPara.sty_RightclickFont);

        GUI.Label(new Rect(2, (CAMERA_OVERLOOK + 1) * menu_height, 190, menu_subsection), "", FuncPara.sty_RightLine);

        GUI.Label(new Rect(4, (PRESET_ONE) * menu_height + menu_subsection, 146, menu_height), "清空画板", FuncPara.sty_RightclickFont);
        //GUI.Label(new Rect(137, (CUSTOM_SETTINGS) * menu_height + menu_subsection, 146, menu_height), "Ctrl + F4", FuncPara.sty_RightclickFont);

        if (GSKDATA.SoftCurrentMode == "FreeOperation")
        {
            GUI.Label(new Rect(4, (PRESET_TWO) * menu_height + menu_subsection, 146, menu_height), "连接到移动端", FuncPara.sty_RightclickFont);
            //GUI.Label(new Rect(137, (PRESET_ONE) * menu_height + menu_subsection, 146, menu_height), "Ctrl + F1", FuncPara.sty_RightclickFont);

            GUI.Label(new Rect(4, (CAMERA_CUSTOM) * menu_height + menu_subsection, 146, menu_height), "开启/关闭辅助射线", FuncPara.sty_RightclickFont);
            //GUI.Label(new Rect(137, (PRESET_TWO) * menu_height + menu_subsection, 146, menu_height), "Ctrl + F2", FuncPara.sty_RightclickFont);

            //GUI.Label(new Rect(4, (CUSTOM_SETTINGS) * menu_height + menu_subsection, 146, menu_height), "切换辅助视角", FuncPara.sty_RightclickFont);
            //GUI.Label(new Rect(137, (CAMERA_CUSTOM) * menu_height + menu_subsection, 146, menu_height), "Ctrl + F3", FuncPara.sty_RightclickFont);

            GUI.Label(new Rect(4, (CUSTOM_SETTINGS) * menu_height + menu_subsection, 146, menu_height), "开启/关闭轨迹显示", FuncPara.sty_RightclickFont);
        }
        
        

        //GUI.Label(new Rect(2, (CUSTOM_SETTINGS + 1) * menu_height + menu_subsection, 190, menu_subsection), "", FuncPara.sty_RightLine);

        //GUI.Label(new Rect(4, (BLANK_SETTINGS) * menu_height + 2 * menu_subsection, 146, menu_height), "毛坯管理", FuncPara.sty_RightclickFont);
        //GUI.Label(new Rect(137, (BLANK_SETTINGS) * menu_height + 2 * menu_subsection, 146, menu_height), "Ctrl + B", FuncPara.sty_RightclickFont);

        //GUI.Label(new Rect(4, (TOOL_SETTINGS) * menu_height + 2 * menu_subsection, 146, menu_height), "刀具管理", FuncPara.sty_RightclickFont);
        //GUI.Label(new Rect(137, (TOOL_SETTINGS) * menu_height + 2 * menu_subsection, 146, menu_height), "Ctrl + C", FuncPara.sty_RightclickFont);


        if (rightclick_e.isMouse && rightclick_e.type == EventType.MouseDown && rightclick_e.button == 0 && current_state != -1)
        {
            if (GSKDATA.SoftCurrentMode == "Teach")
            {
                return ;
            }
			
            switch (current_state)
            {
                case 0://场景还原
                    gameObject.GetComponent<Camerascript>().ResetScene();
                    GameObject.Find("MainScript").GetComponent<ResultDetect>().DetectResetScene();
                    break;
                case 1://机器人返回到零点
                    GameObject.Find("MyMotion").GetComponent<RobotMotion>().ReturnToZero();
                    break;
                case 2://显示示教器
                    if (EncryptionManager.IsEncryped)
                    {
                        GameObject.Find("MainScript").GetComponent<EncryptionManager>().CheckEncryption(true);
                        break;
                    }
                    GameObject.Find("MyButton").GetComponent<ButtonRespond>().ShowPanel();
                    break;
                case 3://显示IO面板
                    GameObject.Find("MyButton").GetComponent<ButtonRespond>().ShowIOPanel();
                    break;
                case 4://清空画板
                    GameObject.Find("MainScript").GetComponent<PaintScript>().ClearPanel();
                    GameObject.Find("MainScript").GetComponent<ResultDetect>().ClearPanel();
                    break;
                case 5://链接到移动端
					ServerMode();
                    break;
                case 6://开启或者关闭辅助射线
                    gameObject.GetComponent<Camerascript>().Ray();
                    break;
                case 7://轨迹显示
                    GSKDATA.PathShow = !GSKDATA.PathShow;
                    break;
            }
        }
    }


    //外部控制右键菜单的方法
    public void RightClickControl(bool switch_on, Vector2 position)
    {
        if (switch_on)
        {
            FuncPara.rightclick_menu_on = true;
			rightclick_rect = new Rect(0, 0, 200f, 140f);
            rightclick_rect.x = position.x;
            rightclick_rect.y = position.y;
            FuncPara.rightRect.x = position.x;
            FuncPara.rightRect.y = position.y;
            FuncPara.cursorPosition.x = FuncPara.cursorPosition.x - FuncPara.rightRect.x;
            FuncPara.cursorPosition.y = FuncPara.cursorPosition.y - FuncPara.rightRect.y;
        }
        else
        {
            FuncPara.rightclick_menu_on = false;
        }

    }


  
	/// <summary>
	/// 启用服务器模式，连接Pad；
	/// </summary>
	private void ServerMode ()
	{
		
		//3、启动Pad端，自动判断当前情况进行处理
		ServerMediation.EnterServerMode();
	}
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			if (ServerCenter.InServerMode)
			{
				ServerMediation.ExitServerMode();            
			}
		}
	}
}
