using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
//<summary>
//ScreenBuild#FILEEXTENSION#
//#PROJECTNAME#
//Created by #SMARTDEVELOPERS# on #CREATIONDATE#.
//Company: Sunnytech
//Function:
//
//
//<summary>
using UnityEngine;
using interpreter;

public class ScreenBuild : MonoBehaviour
{
    #region-----------------------外部DLL函数的声明----------------
    //[DllImport("VR_RBP", EntryPoint = "?RBP_CleanParse@@YAHXZ")]
    //private static extern int RBP_CleanParse();//清空译码内存
    //[DllImport("VR_RBP", EntryPoint = "?RBP_Set_RUNFileName@@YAHQAD@Z")]
    //private static extern int RBP_Set_RUNFileName(char[] name);//清空译码内存
    //[DllImport("VR_RBP", EntryPoint = "?RBP_RunParse@@YAHHQAHHQAN0@Z")]
    //private static extern int RBP_RunParse(int runNum, int[] nextNUM, int dir, double[] pathdata, int[] ipdata);//运行译码数据
    //[DllImport("VR_RBP", EntryPoint = "?RBP_Get_RUNROW@@YAHXZ")]
    //private static extern int RBP_Get_RUNROW();//获取当前运行的行号
    //[DllImport("VR_RBP", EntryPoint = "?RBP_Set_RUNROW@@YAHH@Z")]
    //private static extern int RBP_Set_RUNROW(int rowNum);//设置当前运行的行号
    //[DllImport("VR_RBP", EntryPoint = "?RBP_GetParse_MOVCPOINT@@YAHQAN00@Z")]
    //private static extern int RBP_GetParse_MOVCPOINT(double[] p1, double[] p2, double[] p3);
    //[DllImport("VR_RBP", EntryPoint = "?RBP_Set_IN_Data@@YAHH_N@Z")]
    //private static extern int RBP_Set_IN_Data(int num, bool value);
    //[DllImport("VR_RBP", EntryPoint = "?RBP_Get_IN_Data@@YAHHAA_N@Z")]
    //private static extern int RBP_Get_IN_Data(int num, ref bool value);
    //[DllImport("VR_RBP", EntryPoint = "?RBP_Get_OT_Data@@YAHHAA_N@Z")]
    //private static extern int RBP_Get_OT_Data(int num, ref bool value);
    //[DllImport("VR_RBP", EntryPoint = "?RBP_Set_OT_Data@@YAHH_N@Z")]
    //private static extern int RBP_Set_OT_Data(int num, bool value);
    //[DllImport("VR_RBP", EntryPoint = "?RBP_Get_PX_Data@@YAHHQAN@Z")]
    //private static extern int RBP_Get_PX_Data(int num, double[] pxdata);
    //[DllImport("VR_RBP", EntryPoint = "?RBP_Set_PX_Data@@YAHHQAN@Z")]
    //private static extern int RBP_Set_PX_Data(int num, double[] pxdata);
    //[DllImport("VR_RBP", EntryPoint = "?RBP_Get_I_Data@@YAHHAAH@Z")]
    //private static extern int RBP_Get_I_Data(int num, ref int value);
    //[DllImport("VR_RBP", EntryPoint = "?RBP_Set_I_Data@@YAHHH@Z")]
    //private static extern int RBP_Set_I_Data(int num, int value);
    #endregion

    #region ---窗口常量的声明---
    //WINDOW NUMBER
    const int ZHUYEMIAN_NO = 100;
    const int CHENGXU_NO = 200;
    const int BIANJI_NO = 300;
    const int BIANJI_MODIFY = 310;
    const int XIANSHI_NO = 400;
    const int GONGJU_NO = 500;
    const int XITONGSHEZHI_NO = 101;//100
    const int CHENGXUGUANLI_NO = 102;
    const int CANSHUSHEZHI_NO = 103;
    const int YINGYONG_NO = 104;
    const int BIANLIANG_NO = 105;
    const int XITONGXINXI_NO = 106;
    const int SHURUSHUCHU_NO = 1710;//
    const int SHIJIAODIAN_NO = 1810;//
    const int JIQISHEZHI_NO = 109;
    const int ZAIXIANBANGZHU_NO = 110;
    //
    //
    const int JUEDUILINGDIAN_FUNCTION_NO = 1110;//1100 系统设置
    const int GONGJUZUOBIAO_FUNCTION_NO = 1120;
    const int YONGHUZUOBIAO_FUNCTION_NO = 1130;
    const int XITONGSHIJIAN_FUNCTION_NO = 1140;
    const int KOULINGSHEZHI_FUNCTION_NO = 1150;
    const int MOSHIQIEHUAN_FUNCTION_NO = 1160;
    const int XITONGSUDU_FUNCTION_NO = 1170;
    const int ZHUCHENGXU_FUNCTION_NO = 1180;

    const int TOOL_ZUOBIAOHAO = 1121;//1120 工具坐标
    const int GONGJUZUOBIAO_SET = 1125;
    const int GET_THREE_ONE = 1121;
    const int GET_THREE_TWO = 1122;
    const int GET_THREE_THREE = 1123;
    const int GET_FIVE_ONE = 1125;
    const int GET_FIVE_TWO = 1126;
    const int GET_FIVE_THREE = 1127;
    const int GET_FIVE_FOUR = 1128;
    const int GET_FIVE_FIVE = 1129;
    const int TOOL_ZHIJIESHURU = 1127;//设置工具坐标
    const int TOOL_SHANDIANFA = 1128;
    const int TOOL_WUDIANFA = 1129;

    const int USER_ZUOBIAOHAO = 1131;//1130 用户坐标
    const int YONGHUZUOBIAO_SET = 1135;
    const int GET_THREE_ONE_ = 1131;
    const int GET_THREE_TWO_ = 1132;
    const int GET_THREE_THREE_ = 1133;
    const int USER_ZHIJIESHURU = 1137;//法设置用户坐标
    const int USER_SHANDIANFA = 1138;

    const int MOSHIQIEHUAN_FUNCTION = 1165;//1160 模式

    const int XITONGSUDU_SHEZHI = 1175;//1170 系统速度
    const int XITONGSUDU_MOREN = 1176;
    const int XITONGSUDU_KAIJI = 1177;

    //
    //
    const int XINJIANCHENGXU_FUNCTION_NO = 1210;//1200 程序管理
    const int CHENGXUYILAN_FUNCTION_NO = 1220;
    const int COPY_PROGRAM = 1225;
    const int DELETE_PROGRAM = 1226;
    const int SEARCH_PROGRAM = 1227;
    const int RENAME_PROGRAM = 1228;

    const int XINJIANCHENGXU = 1215;
    //
    //
    const int GUANJIECANSHU_FUNCTION_NO = 1310;//1300 参数设置
    const int ZHOUCANSHU_FUNCTION_NO = 1320;
    const int YUNDONGCANSHU_FUNCTION_NO = 1330;
    const int SIFUCANSHU_FUNCTION_NO = 1340;
    const int LIANGANCANSHU_FUNCTION_NO = 1350;

    const int YINHUTIAOJIAN_FUNCTION_NO = 1410;//1400 应用
    const int XIHUTIAOJIAN_FUNCTION_NO = 1420;
    const int BAIHAN_FUNCTION_NO = 1430;
    const int SHUZIHANJI_FUNCTION_NO = 1440;
    const int HANJIESHEZHI_FUNCTION_NO = 1450;
    const int HANJIKONGZHI_FUNCTION_NO = 1460;

    const int ZHENGSHUXING_FUNCTION_NO = 1510;//1500 变量
    const int DIKAERWEIZI_FUNCTION_NO = 1520;

    const int BAOJING_INFORMATION_NO = 1610;//1600 系统信息
    const int ANJIAN_INFORMATION_NO = 1620; 
    const int BANBEN_INFORMATION_NO = 1630;
    const int GONGJU_INFORMATION_NO = 1640;
    const int YONGHU_INFORMATION_NO = 1650;

    const int ZAIXIANYUNXINGFANGSHI_FUNCTION_NO = 1910;//1900 机器设置
    const int RUANJIXIAN_FUNCTION_NO = 1920;
    const int GANSHEQU_FUNCTION_NO = 1930;

    const int ZHILING_HELP_NO = 1010;//1000 在线帮助
    const int CAOZUO_HELP_NO = 1020;
    const int ZHILING_LABEL = 1015;//指令标签
    const int CAOZUO_LABEL = 1025;

    //程序界面
    const int SETCURRENTCHENGXU = 210;//设置当前程序
    const int DELETE_NO = 220;//删除窗口
    const int DELETE_YES = 222;
    const int DELETE_NOO = 225;
    const int START_WIN = 226;//从当前位置启动程序窗口
    const int START_YES = 227;
    const int START_NO = 228;


    const int ADD = 301;
    const int ADD_1 = 302;
    const int ADD_2 = 303;
    const int ADD_3 = 304;
    const int ADD_4 = 305;
    const int ADD_5 = 306;
    const int ADD_6 = 307;
    const int ADD_MOVJ = 311;
    const int ADD_MOVL = 312;
    const int ADD_MOVC = 313;
    const int ADD_DOUT = 321;
    const int ADD_DIN = 322;
    const int ADD_DELAY = 323;
    const int ADD_WAIT = 324;
    const int ADD_PULSE = 325;
    const int ADD_LAB = 331;
    const int ADD_JUMP = 332;
    const int ADD_JUMP_R = 333;
    const int ADD_JUMP_IN = 334;
    const int ADD_JH = 335;
    const int ADD_END = 336;
    const int ADD_MAIN = 337;
    const int ADD_CALL = 338;
    const int ADD_R = 341;
    const int ADD_INC = 342;
    const int ADD_DEC = 343;
    const int ADD_PX = 351;
    const int ADD_SHIFTON = 352;
    const int ADD_SHIFTOFF = 353;
    const int ADD_MSHIFT = 354;
    const int ADD_ARCON = 361;
    const int ADD_ARCOF = 362;


    const int POINTREPEAT = 370;//示教点重复窗
    const int POINTREPEAT_YES = 380;
    const int POINTREPEAT_NO = 390;

    const int RUANJIANPAN = 214914;//软键盘
    const int RUANJIANPAN2 = 215914;//软键盘

    const int UPDATELIST = 520;//更新列表


    //A~Z,a~z,fuhao
    const int LETTER_A = 2000 + 'A';
    const int LETTER_B = 2000 + 'B';
    const int LETTER_C = 2000 + 'C';
    const int LETTER_D = 2000 + 'D';
    const int LETTER_E = 2000 + 'E';
    const int LETTER_F = 2000 + 'F';
    const int LETTER_G = 2000 + 'G';
    const int LETTER_H = 2000 + 'H';
    const int LETTER_I = 2000 + 'I';
    const int LETTER_J = 2000 + 'J';
    const int LETTER_K = 2000 + 'K';
    const int LETTER_L = 2000 + 'L';
    const int LETTER_M = 2000 + 'M';
    const int LETTER_N = 2000 + 'N';
    const int LETTER_O = 2000 + 'O';
    const int LETTER_P = 2000 + 'P';
    const int LETTER_Q = 2000 + 'Q';
    const int LETTER_R = 2000 + 'R';
    const int LETTER_S = 2000 + 'S';
    const int LETTER_T = 2000 + 'T';
    const int LETTER_U = 2000 + 'U';
    const int LETTER_V = 2000 + 'V';
    const int LETTER_W = 2000 + 'W';
    const int LETTER_X = 2000 + 'X';
    const int LETTER_Y = 2000 + 'Y';
    const int LETTER_Z = 2000 + 'Z';
   
    #endregion

    const string SUFFIX = ".prl";//后缀名

    #region ---按钮、窗口的声明----
    //公共
    MyButton zhuYeMian;
    MyButton chengXu;
    MyButton bianJi;
    MyButton xianShi;
    MyButton gongJu;

    //主页面
    MyButton xtSheZhi;
    MyButton cxGuanLi;
    MyButton csSheZhi;
    MyButton YingYong;
    MyButton BianLiang;
    MyButton xtXinXi;
    MyButton srShuChu;
    MyButton sJiaoDian;
    MyButton jqSheZhi;
    MyButton zxBangZhu;
    MySprite f1Line1;
    MySprite f1Line2;
    MySprite f1Line3;
    MySprite f1Line4;
    MySprite f1Line5;
    MySprite f1Line6;
    ButtonPart F1part1;
    ButtonPart F1part2;
    ScrollPart F1part3;
    Window_depth F1window;

    //系统设置二级菜单
    //系统设置
    MySprite jdLingDian;
    MySprite gjZuoBiao;
    MySprite yhZuoBiao;
    MySprite xtShiJian;
    MySprite klSheZhi;
    MySprite msQieHuan;
    MySprite xtShuDu;
    MySprite zcxSheZhi;
    ButtonPartSecond xtSheZhi_part;
    Window_alpha xtSheZhi_win;
    //程序管理
    MySprite xjChengXu;
    MySprite cxYiLan;
    ButtonPartSecond cxGuanli_part;
    Window_alpha cxGuanli_win;
    //参数设置 
    MySprite gjCanShu;
    MySprite zCanShu;
    MySprite ydCanShu;
    MySprite sfCanShu; 
    MySprite lgCanShu;
    ButtonPartSecond csSheZhi_part;
    Window_alpha csSheZhi_win;
    //应用
    MySprite yhTiaoJian;
    MySprite xhTiaoJian;
    MySprite BaiHan;
    MySprite szHanJi;
    MySprite hjSheZhi;
    MySprite hjKongZhi;
    ButtonPartSecond YingYong_part;
    Window_alpha YingYong_win;
    //变量
    MySprite zShuXing;
    MySprite dkeWeiZi;
    ButtonPartSecond BianLiang_part;
    Window_alpha BianLiang_win;
    //系统信息
    MySprite bjXinXi;
    MySprite ajXinXi;
    MySprite bbXinXi;
    MySprite gjZuoBiao2;
    MySprite yhZuoBiao2;
    ButtonPartSecond xtXinXi_part;
    Window_alpha xtXinXi_win;
    //机器设置
    MySprite zxyxFangShi;
    MySprite rJiXian;
    MySprite gSheQu;
    ButtonPartSecond jqSheZhi_part;
    Window_alpha jqSheZhi_win;
    //在线帮助
    MySprite ZhiLing;
    MySprite CaoZuo;
    ButtonPartSecond zxBangZu_part;
    Window_alpha zxBangZu_win;

    //绝对零点位置设置界面


    //工具坐标系设置方法选择界面
    MySprite setToolNo;
    OneButtonPart setToolF_1;

    MyWid toolDirect;
    MyWid toolDirect2;
    MyWid toolThree;
    MyWid toolThree2;
    MyWid toolFive;
    MyWid toolFive2;
    OptionPart setToolF_2;

    MyButton setToolXZ;
    MyButton setToolTC;
    ButtonPart setToolF_3;
    Window_depth setToolF_win;

    //直接输入法设置工具坐标界面
    MyText tool_X;
    MyText tool_Y;
    MyText tool_Z;
    MyText tool_W;
    MyText tool_P;
    MyText tool_R;
    TextPart toolD_1;

    MyButton toolD_SZ;
    MyButton toolD_TC;
    ButtonPart toolD_2;
    Window_depth toolD_win;

    //三点法设置工具坐标界面
    MyWid tool_T_point1;
    MyWid tool_T_point2;
    MyWid tool_T_point3;
    ButtonPart toolT_1;

    MyButton toolT_SZ;
    MyButton toolT_TC;
    ButtonPart toolT_2;
    Window_depth toolT_win;

    //五点法设置工具坐标界面

    MyWid tool_F_point1;
    MyWid tool_F_point2;
    MyWid tool_F_point3;
    MyWid tool_F_point4;
    MyWid tool_F_point5;
    ButtonPart toolF_1;

    MyButton toolF_SZ;
    MyButton toolF_TC;
    ButtonPart toolF_2;
    Window_depth toolF_win;

    //用户坐标系设置方法选择界面
    MySprite setUserNo;
    OneButtonPart setUserF_1;

    MyWid userDirect;
    MyWid userDirect2;
    MyWid userThree;
    MyWid userThree2;
    OptionPart setUserF_2;

    MyButton setUserXZ;
    MyButton setUserTC;
    ButtonPart setUserF_3;
    Window_depth setUserF_win;

    //直接输入法设置用户坐标界面
    MyText user_X;
    MyText user_Y;
    MyText user_Z;
    MyText user_W;
    MyText user_P;
    MyText user_R;
    TextPart userD_1;

    MyButton userD_SZ;
    MyButton userD_TC;
    ButtonPart userD_2;
    Window_depth userD_win;

    //三点法设置用户坐标界面
    MyWid user_T_point1;
    MyWid user_T_point2;
    MyWid user_T_point3;
    ButtonPart userT_1;

    MyButton userT_SZ;
    MyButton userT_TC;
    ButtonPart userT_2;
    Window_depth userT_win;

    //口令设置界面


    //模式切换设置界面
    MyWid operationMode;
    MyWid operationMode2;
    MyWid editMode;
    MyWid editMode2;
    MyWid managementMode;
    MyWid managementMode2;
    OptionPart modeF_1;

    MyButton modeXZ;
    MyButton modeTC;
    ButtonPart modeF_2;
    Window_depth modeF_win;

    //系统速度界面
    MyText speed_I;
    MyText speed_L;
    MyText speed_M;
    MyText speed_H;
    MyText speed_S;
    MyWid speed_kaiji;
    TextPart speedF_1;

    MyButton speed_SZ;
    MyButton speed_MRZ;
    MyButton speed_TC;
    ButtonPart speedF_2;
    Window_depth speedF_win;

    //新建程序
    MySprite newBuildLine1;
    MySprite newBuildLine2;
    MySprite newBuildLine3;
    MySprite newBuildLine4;
    MySprite newBuildLine5;
    ScrollPart newBuildPart_1;

    MyText2 newBuildName;
    ButtonPart newBuildPart_2;

    MyButton newBuild_XJ;
    MyButton newBuild_TC;

    ButtonPart newBuildPart_3;
    Window_depth newBuild_win;

    //软键盘
    MyText2 ruanJianPanName;

    MyButton XiaoXie;
    MyButton DaXie;
    MyButton FuHao;

    MyButton letterA; MyButton letterB; MyButton letterC; MyButton letterD; MyButton letterE; MyButton letterF;
    MyButton letterG; MyButton letterH; MyButton letterI; MyButton letterJ; MyButton letterK; MyButton letterL;
    MyButton letterM; MyButton letterN; MyButton letterO; MyButton letterP; MyButton letterQ; MyButton letterR;
    MyButton letterS; MyButton letterT; MyButton letterU; MyButton letterV; MyButton letterW; MyButton letterX;
    MyButton letterY; MyButton letterZ; MyButton letter27; MyButton letter28; MyButton letter29; MyButton letter30;
    ButtonMulti letterPart;
    Window_a letterWin;

    //程序一览
    MySprite programV_line1;
    MySprite programV_line2;
    MySprite programV_line3;
    MySprite programV_line4;
    MySprite programV_line5;
    MySprite programV_line6;
    MySprite programV_line7;
    ScrollPart programVPart_1;

    MyText2 newName;
    ButtonPart programVPart_2;

    MyButton programV_copy;
    MyButton programV_delete;
    MyButton programV_search;
    MyButton programV_rename;
    MyButton programV_exit;
    ButtonPart programVPart_3;
    Window_depth programV_win;

    //在线帮助
    //指令
    MyWid ZL_movj;
    MyWid ZL_movl;
    MyWid ZL_movc;
    MyWid ZL_dout;
    MyWid ZL_din;
    MyWid ZL_delay;
    MyWid ZL_wait;
    MyWid ZL_label;
    MyWid ZL_jump;
    MyWid ZL_end;
    LabelPart ZL_Part;
    Window_depth ZL_win;

    //操作
    MyWid CZ_lingdian;
    LabelPart CZ_Part;
    Window_depth CZ_win;

    //程序界面部分
    ProgramWinClass ProgramWin;
    //程序界面的删除确认部分
    Window_a DeleteWin;
    MyButton DeleteYes;
    MyButton DeleteNo;
    ButtonPart DeletePart;
    //程序界面的示教点重复界面
    Window_a PointRepeatWin;
    MyButton PointRepeatYes;
    MyButton PointRepeatNo;
    ButtonPart PointRepeatPart;
    //添加界面
    MyButton AddButton1;
    MyButton AddButton2;
    MyButton AddButton3;
    MyButton AddButton4;
    MyButton AddButton5;
    MyButton AddButton6;
    ButtonPart AddPart;
    Window_alpha AddWin;
    //add1界面
    MyButton AddButton_movj;
    MyButton AddButton_movl;
    MyButton AddButton_movc;
    ButtonPart AddPart1;
    Window_alpha AddWin1;
    //add2界面
    MyButton AddButton_dout;
    MyButton AddButton_din;
    MyButton AddButton_delay;
    MyButton AddButton_wait;
    MyButton AddButton_pulse;
    ButtonPart AddPart2;
    Window_alpha AddWin2;
    //add3界面
    MyButton AddButton_lab;
    MyButton AddButton_jump;
    MyButton AddButton_jump_r;
    MyButton AddButton_jump_in;
    MyButton AddButton_jh;
    MyButton AddButton_end;
    MyButton AddButton_main;
    MyButton AddButton_call;
    ButtonPart AddPart3;
    Window_alpha AddWin3;
    //add4界面
    MyButton AddButton_r;
    MyButton AddButton_inc;
    MyButton AddButton_dec;
    ButtonPart AddPart4;
    Window_alpha AddWin4;
    //add5界面
    MyButton AddButton_px;
    MyButton AddButton_shifton;
    MyButton AddButton_shiftoff;
    MyButton AddButton_mshift;
    ButtonPart AddPart5;
    Window_alpha AddWin5;
    //add6界面
    MyButton AddButton_arcon;
    MyButton AddButton_arcof;
    ButtonPart AddPart6;
    Window_alpha AddWin6;
    //启动程序界面
    MyButton StartButton_yes;
    MyButton StartButton_no;
    ButtonPart StartPart;
    Window_a StartWin;


    //显示界面
    Window_depth_no F4Window;
    //工具界面
    Window_depth_no F5Window;
    

    #endregion

    public Win CurrentWindow;//现在正在进行操作的窗口

    //定义文件
    ROBOTFILE GSKFile;
    string FILEPATH;//文件位置
    //LexicalAnalysis LAnalysis;//词法分析
    RobotMotion MotionScript;//运动脚本
    ResultDetect detectresult;
    ServerCenter ServerCall;
    WeldLine wlscript;//焊缝控制脚本

    GSKInterpreter interpreter;


    #region----------//程序列表-----------------------
    string[] StrNo1 = new string[6] { "no1", "no2", "no3", "no4", "no5", "no6" };
    string[] StrName1 = new string[6] { "name1", "name2", "name3", "name4", "name5", "name6" };
    string[] StrSize1 = new string[6] { "size1", "size2", "size3", "size4", "size5", "size6" };
    string[] StrTime1 = new string[6] { "time1", "time2", "time3", "time4", "time5", "time6" };

    string[] StrNo2 = new string[6] { "F118_no1", "F118_no2", "F118_no3", "F118_no4", "F118_no5", "F118_no6" };
    string[] StrName2 = new string[6] { "F118_name1", "F118_name2", "F118_name3", "F118_name4", "F118_name5", "F118_name6" };
    string[] StrSize2 = new string[6] { "F118_size1", "F118_size2", "F118_size3", "F118_size4", "F118_size5", "F118_size6" };
    string[] StrTime2 = new string[6] { "F118_time1", "F118_time2", "F118_time3", "F118_time4", "F118_time5", "F118_time6" };

    string[] StrNo3 = new string[5] { "F121_no1", "F121_no2", "F121_no3", "F121_no4", "F121_no5" };
    string[] StrName3 = new string[5] { "F121_name1", "F121_name2", "F121_name3", "F121_name4", "F121_name5" };
    string[] StrSize3 = new string[5] { "F121_size1", "F121_size2", "F121_size3", "F121_size4", "F121_size5" };
    string[] StrTime3 = new string[5] { "F121_time1", "F121_time2", "F121_time3", "F121_time4", "F121_time5" };

    string[] StrNo4 = new string[7] { "F122_no1", "F122_no2", "F122_no3", "F122_no4", "F122_no5", "F122_no6", "F122_no7" };
    string[] StrName4 = new string[7] { "F122_name1", "F122_name2", "F122_name3", "F122_name4", "F122_name5", "F122_name6", "F122_name7" };
    string[] StrSize4 = new string[7] { "F122_size1", "F122_size2", "F122_size3", "F122_size4", "F122_size5", "F122_size6", "F122_size7" };
    string[] StrTime4 = new string[7] { "F122_time1", "F122_time2", "F122_time3", "F122_time4", "F122_time5", "F122_time6", "F122_time7" };

    string[] StrNo8 = new string[9] { "F18XH0", "F18XH1", "F18XH2", "F18XH3", "F18XH4", "F18XH5", "F18XH6", "F18XH7", "F18XH8" };
    string[] StrName8 = new string[9] { "F18Name0", "F18Name1", "F18Name2", "F18Name3", "F18Name4", "F18Name5", "F18Name6", "F18Name7", "F18Name8" };
    string[] StrSize8 = new string[9] { "F18Size0", "F18Size1", "F18Size2", "F18Size3", "F18Size4", "F18Size5", "F18Size6", "F18Size7", "F18Size8" };
    #endregion


    //子程序
    GSKInterpreter tmpInterpreter;
    string childFileName;

    // Use this for initialization
    void Start ()
    {
        FILEPATH = Application.dataPath + "\\StreamingAssets\\Programs\\";
        GSKDATA.CurrentProgramName = "777777" + SUFFIX;
        GSKFile = new ROBOTFILE(FILEPATH);
        //LAnalysis = new LexicalAnalysis(FILEPATH);
        MotionScript = GameObject.Find("MyMotion").GetComponent<RobotMotion>();
        PanelPosition(true);//面板位于屏幕的右侧
        #region ---按钮、窗口的定义---
        //f1~f5
        zhuYeMian = new MyButton("F1", "zhuyemian", ZHUYEMIAN_NO, "ButtonNormal", "ButtonActive");
        chengXu = new MyButton("F2", "chengxu", CHENGXU_NO);
        bianJi = new MyButton("F3", "bianji", BIANJI_NO);
        xianShi = new MyButton("F4", "xianshi", XIANSHI_NO);
        gongJu = new MyButton("F5", "gongju", GONGJU_NO);
        List<Element> part_up = new List<Element>();
        part_up.Add(zhuYeMian); part_up.Add(chengXu); part_up.Add(bianJi); part_up.Add(xianShi); part_up.Add(gongJu);

        //主页面
        xtSheZhi = new MyButton("F1_1", "xitongshezhi", XITONGSHEZHI_NO);
        cxGuanLi = new MyButton("F1_2", "chengxuguanli", CHENGXUGUANLI_NO);
        csSheZhi = new MyButton("F1_3", "canshushezhi", CANSHUSHEZHI_NO);
        YingYong = new MyButton("F1_4", "yingyong", YINGYONG_NO);
        BianLiang = new MyButton("F1_5", "bianliang", BIANLIANG_NO);
        xtXinXi = new MyButton("F1_6", "xitongxinxi", XITONGXINXI_NO);
        srShuChu = new MyButton("F1_7", "shurushuchu", SHURUSHUCHU_NO);
        sJiaoDian = new MyButton("F1_8", "shijiaodian", SHIJIAODIAN_NO);
        jqSheZhi = new MyButton("F1_9", "jiqishezhi", JIQISHEZHI_NO);
        zxBangZhu = new MyButton("F1_10", "zaixianbangzu", ZAIXIANBANGZHU_NO);
        List<Element> part_f1_2 = new List<Element>();
        part_f1_2.Add(xtSheZhi); part_f1_2.Add(cxGuanLi); part_f1_2.Add(csSheZhi);
        part_f1_2.Add(YingYong); part_f1_2.Add(BianLiang); part_f1_2.Add(xtXinXi);
        part_f1_2.Add(srShuChu); part_f1_2.Add(sJiaoDian); part_f1_2.Add(jqSheZhi);
        part_f1_2.Add(zxBangZhu);

        f1Line1 = new MySprite("Line1", SETCURRENTCHENGXU, "greybackground", "whitebg");
        f1Line2 = new MySprite("Line2", SETCURRENTCHENGXU);
        f1Line3 = new MySprite("Line3", SETCURRENTCHENGXU);
        f1Line4 = new MySprite("Line4", SETCURRENTCHENGXU);
        f1Line5 = new MySprite("Line5", SETCURRENTCHENGXU);
        f1Line6 = new MySprite("Line6", SETCURRENTCHENGXU);
        List<Element> part_f1_3 = new List<Element>();
        part_f1_3.Add(f1Line1); part_f1_3.Add(f1Line2); part_f1_3.Add(f1Line3);
        part_f1_3.Add(f1Line4); part_f1_3.Add(f1Line5); part_f1_3.Add(f1Line6);

        F1part1 = new ButtonPart(part_up);
        F1part2 = new ButtonPart(part_f1_2);
        F1part3 = new ScrollPart(part_f1_3, GSKDATA.ChengXuCount, "Scroll_F1");
        List<Part> f1parts = new List<Part>();
        f1parts.Add(F1part1); f1parts.Add(F1part2); f1parts.Add(F1part3);
        F1window = new Window_depth(f1parts, "Panel_F1", ZHUYEMIAN_NO);

        //系统设置二级菜单
        //系统设置
        jdLingDian = new MySprite("Zeroset", JUEDUILINGDIAN_FUNCTION_NO, "yellowbg", "whitebg");
        gjZuoBiao = new MySprite("GJZBset", GONGJUZUOBIAO_FUNCTION_NO, "yellowbg");
        yhZuoBiao = new MySprite("YFZBset", YONGHUZUOBIAO_FUNCTION_NO, "yellowbg");
        xtShiJian = new MySprite("Timeset", XITONGSHIJIAN_FUNCTION_NO, "yellowbg");
        klSheZhi = new MySprite("KLset", KOULINGSHEZHI_FUNCTION_NO, "yellowbg");
        msQieHuan = new MySprite("Modeset", MOSHIQIEHUAN_FUNCTION_NO, "yellowbg");
        xtShuDu = new MySprite("XTspeedset", XITONGSUDU_FUNCTION_NO, "yellowbg");
        zcxSheZhi = new MySprite("ZCXset", ZHUCHENGXU_FUNCTION_NO, "yellowbg");
        List<Element> xtsz_parts= new List<Element>();
        xtsz_parts.Add(jdLingDian);xtsz_parts.Add(gjZuoBiao);xtsz_parts.Add(yhZuoBiao);
        xtsz_parts.Add(xtShiJian);xtsz_parts.Add(klSheZhi);xtsz_parts.Add(msQieHuan);
        xtsz_parts.Add(xtShuDu);xtsz_parts.Add(zcxSheZhi);
        xtSheZhi_part = new ButtonPartSecond(xtsz_parts);
        List<Part> xtsz_win = new List<Part>();
        xtsz_win.Add(xtSheZhi_part);
        xtSheZhi_win = new Window_alpha(xtsz_win, "XTset", ZHUYEMIAN_NO);
        //程序管理
        xjChengXu = new MySprite("Buidcx", XINJIANCHENGXU_FUNCTION_NO, "yellowbg", "whitebg");
        cxYiLan = new MySprite("CXsee", CHENGXUYILAN_FUNCTION_NO, "yellowbg");
        List<Element> cxgl_parts = new List<Element>();
        cxgl_parts.Add(xjChengXu); cxgl_parts.Add(cxYiLan);
        cxGuanli_part = new ButtonPartSecond(cxgl_parts);
        List<Part> cxgl_win = new List<Part>();
        cxgl_win.Add(cxGuanli_part);
        cxGuanli_win = new Window_alpha(cxgl_win, "CXmanage", ZHUYEMIAN_NO);
        //参数设置
        gjCanShu = new MySprite("GJcs", GUANJIECANSHU_FUNCTION_NO, "yellowbg", "whitebg");
        zCanShu = new MySprite("Zcs", ZHOUCANSHU_FUNCTION_NO, "yellowbg");
        ydCanShu = new MySprite("YDcs", YUNDONGCANSHU_FUNCTION_NO, "yellowbg");
        sfCanShu = new MySprite("SFcs", SIFUCANSHU_FUNCTION_NO, "yellowbg");
        lgCanShu = new MySprite("LGcs", LIANGANCANSHU_FUNCTION_NO, "yellowbg");
        List<Element> cssz_parts = new List<Element>();
        cssz_parts.Add(gjCanShu); cssz_parts.Add(zCanShu); cssz_parts.Add(ydCanShu);
        cssz_parts.Add(sfCanShu); cssz_parts.Add(lgCanShu);
        csSheZhi_part = new ButtonPartSecond(cssz_parts);
        List<Part> cssz_win = new List<Part>();
        cssz_win.Add(csSheZhi_part);
        csSheZhi_win = new Window_alpha(cssz_win, "CSset", ZHUYEMIAN_NO);
        //应用
        yhTiaoJian = new MySprite("YHtj", YINHUTIAOJIAN_FUNCTION_NO, "yellowbg", "whitebg");
        xhTiaoJian = new MySprite("XHtj", XIHUTIAOJIAN_FUNCTION_NO, "yellowbg");
        BaiHan = new MySprite("BH", BAIHAN_FUNCTION_NO, "yellowbg");
        szHanJi = new MySprite("SZhj", SHUZIHANJI_FUNCTION_NO, "yellowbg");
        hjSheZhi = new MySprite("HJset", HANJIESHEZHI_FUNCTION_NO, "yellowbg");
        hjKongZhi = new MySprite("Hjkz", HANJIKONGZHI_FUNCTION_NO, "yellowbg");
        List<Element> yy_parts = new List<Element>();
        yy_parts.Add(yhTiaoJian); yy_parts.Add(xhTiaoJian); yy_parts.Add(BaiHan);
        yy_parts.Add(szHanJi); yy_parts.Add(hjSheZhi); yy_parts.Add(hjKongZhi);
        YingYong_part = new ButtonPartSecond(yy_parts);
        List<Part> yy_win = new List<Part>();
        yy_win.Add(YingYong_part);
        YingYong_win = new Window_alpha(yy_win, "YY", ZHUYEMIAN_NO);
        //变量
        zShuXing = new MySprite("Int", ZHENGSHUXING_FUNCTION_NO, "yellowbg", "whitebg");
        dkeWeiZi = new MySprite("DKER", DIKAERWEIZI_FUNCTION_NO, "yellowbg", "whitebg");
        List<Element> bl_parts = new List<Element>();
        bl_parts.Add(zShuXing); bl_parts.Add(dkeWeiZi);
        BianLiang_part = new ButtonPartSecond(bl_parts);
        List<Part> bl_win = new List<Part>();
        bl_win.Add(BianLiang_part);
        BianLiang_win = new Window_alpha(bl_win, "BL", ZHUYEMIAN_NO);
        //系统信息
        bjXinXi = new MySprite("BJxx", BAOJING_INFORMATION_NO, "yellowbg", "whitebg");
        ajXinXi = new MySprite("AJxx", ANJIAN_INFORMATION_NO, "yellowbg");
        bbXinXi = new MySprite("BBxx", BANBEN_INFORMATION_NO, "yellowbg");
        gjZuoBiao2 = new MySprite("GJzb", GONGJU_INFORMATION_NO, "yellowbg");
        yhZuoBiao2 = new MySprite("YHzb", YONGHU_INFORMATION_NO, "yellowbg");
        List<Element> xtxx_parts = new List<Element>();
        xtxx_parts.Add(bjXinXi); xtxx_parts.Add(ajXinXi); xtxx_parts.Add(bbXinXi);
        xtxx_parts.Add(gjZuoBiao2); xtxx_parts.Add(yhZuoBiao2);
        xtXinXi_part = new ButtonPartSecond(xtxx_parts);
        List<Part> xtxx_win = new List<Part>();
        xtxx_win.Add(xtXinXi_part);
        xtXinXi_win = new Window_alpha(xtxx_win, "XTxx", ZHUYEMIAN_NO);
        //机器设置
        zxyxFangShi = new MySprite("ZYX", ZAIXIANYUNXINGFANGSHI_FUNCTION_NO, "yellowbg", "whitebg");
        rJiXian = new MySprite("RJX", RUANJIXIAN_FUNCTION_NO, "yellowbg");
        gSheQu = new MySprite("GSQ", GANSHEQU_FUNCTION_NO, "yellowbg");
        List<Element> jqsz_parts = new List<Element>();
        jqsz_parts.Add(zxyxFangShi); jqsz_parts.Add(rJiXian); jqsz_parts.Add(gSheQu);
        jqSheZhi_part = new ButtonPartSecond(jqsz_parts);
        List<Part> jqsz_win = new List<Part>();
        jqsz_win.Add(jqSheZhi_part);
        jqSheZhi_win = new Window_alpha(jqsz_win, "JQset", ZHUYEMIAN_NO);
        //在线帮助
        ZhiLing = new MySprite("ZL", ZHILING_HELP_NO, "yellowbg", "whitebg");
        CaoZuo = new MySprite("CZ", CAOZUO_HELP_NO, "yellowbg");
        List<Element> zxbz_parts = new List<Element>();
        zxbz_parts.Add(ZhiLing); zxbz_parts.Add(CaoZuo);
        zxBangZu_part = new ButtonPartSecond(zxbz_parts);
        List<Part> zxbz_win = new List<Part>();
        zxbz_win.Add(zxBangZu_part);
        zxBangZu_win = new Window_alpha(zxbz_win, "Help", ZHUYEMIAN_NO);


        //工具坐标系设置方法选择界面
        setToolNo = new MySprite("F1_1_2_N_bg", 1, "yellowbg");
        setToolF_1 = new OneButtonPart(setToolNo, TOOL_ZUOBIAOHAO);

        toolDirect = new MyWid("F1_1_2_1bt");
        toolDirect2 = new MyWid("F1_1_2_1bg");
        toolThree = new MyWid("F1_1_2_2bt");
        toolThree2 = new MyWid("F1_1_2_2bg");
        toolFive = new MyWid("F1_1_2_3bt");
        toolFive2 = new MyWid("F1_1_2_3bg");
        List<Element> toolF = new List<Element>();
        toolF.Add(toolDirect); toolF.Add(toolThree); toolF.Add(toolFive);
        List<Element> toolF2 = new List<Element>();
        toolF2.Add(toolDirect2); toolF2.Add(toolThree2); toolF2.Add(toolFive2);
        setToolF_2 = new OptionPart(toolF, toolF2);

        setToolXZ = new MyButton("F1_1_2_choose", "F112_c", GONGJUZUOBIAO_SET);
        setToolTC = new MyButton("F1_1_2_exit", "F112_t", ZHUYEMIAN_NO);
        List<Element> toolF_xt = new List<Element>();
        toolF_xt.Add(setToolXZ); toolF_xt.Add(setToolTC);
        setToolF_3 = new ButtonPart(toolF_xt);
        List<Part> toolF_parts = new List<Part>();
        toolF_parts.Add(setToolF_1); toolF_parts.Add(setToolF_2); toolF_parts.Add(setToolF_3);
        setToolF_win = new Window_depth(toolF_parts, "Panel_F1_1_2", ZHUYEMIAN_NO);

        //直接输入法设置工具坐标
        tool_X = new MyText("F1_1_2_1_X", "F1_1_2_1_X_bg", "4.2");
        tool_Y = new MyText("F1_1_2_1_Y", "F1_1_2_1_Y_bg", "4.2");
        tool_Z = new MyText("F1_1_2_1_Z", "F1_1_2_1_Z_bg", "4.2");
        tool_W = new MyText("F1_1_2_1_W", "F1_1_2_1_W_bg", "4.2");
        tool_P = new MyText("F1_1_2_1_P", "F1_1_2_1_P_bg", "4.2");
        tool_R = new MyText("F1_1_2_1_R", "F1_1_2_1_R_bg", "4.2");
        List<Element> toolD_1_members=new List<Element>();
        toolD_1_members.Add(tool_X);toolD_1_members.Add(tool_Y);toolD_1_members.Add(tool_Z);
        toolD_1_members.Add(tool_W);toolD_1_members.Add(tool_P);toolD_1_members.Add(tool_R);
        toolD_1 = new TextPart(toolD_1_members);

        toolD_SZ = new MyButton("F1_1_2_1_set", "F1121_s", TOOL_ZHIJIESHURU);
        toolD_TC = new MyButton("F1_1_2_1_exit", "F1121_t", GONGJUZUOBIAO_FUNCTION_NO);
        List<Element> toolD_2_members = new List<Element>();
        toolD_2_members.Add(toolD_SZ); toolD_2_members.Add(toolD_TC);
        toolD_2 = new ButtonPart(toolD_2_members);
        List<Part> toolD_win_members = new List<Part>();
        toolD_win_members.Add(toolD_1); toolD_win_members.Add(toolD_2);
        toolD_win = new Window_depth(toolD_win_members, "Panel_F1_1_2_1", GONGJUZUOBIAO_FUNCTION_NO);

        //三点法设置工具坐标
        tool_T_point1 = new MyWid("F1_1_2_2_1_bg", GET_THREE_ONE);
        tool_T_point2 = new MyWid("F1_1_2_2_2_bg", GET_THREE_TWO);
        tool_T_point3 = new MyWid("F1_1_2_2_3_bg", GET_THREE_THREE);
        List<Element> toolT_1_members = new List<Element>();
        toolT_1_members.Add(tool_T_point1); toolT_1_members.Add(tool_T_point2); toolT_1_members.Add(tool_T_point3);
        toolT_1 = new ButtonPart(toolT_1_members);

        toolT_SZ = new MyButton("F1_1_2_2_set", "F1122_s", TOOL_SHANDIANFA);
        toolT_TC = new MyButton("F1_1_2_2_exit", "F1122_t", GONGJUZUOBIAO_FUNCTION_NO);
        List<Element> toolT_2_members = new List<Element>();
        toolT_2_members.Add(toolT_SZ); toolT_2_members.Add(toolT_TC);
        toolT_2 = new ButtonPart(toolT_2_members);
        List<Part> toolT_win_members = new List<Part>();
        toolT_win_members.Add(toolT_1); toolT_win_members.Add(toolT_2);
        toolT_win = new Window_depth(toolT_win_members, "Panel_F1_1_2_2", GONGJUZUOBIAO_FUNCTION_NO);

        //五点法设置工具坐标
        tool_F_point1 = new MyWid("F1_1_2_3_1_bg", GET_FIVE_ONE);
        tool_F_point2 = new MyWid("F1_1_2_3_2_bg", GET_FIVE_TWO);
        tool_F_point3 = new MyWid("F1_1_2_3_3_bg", GET_FIVE_THREE);
        tool_F_point4 = new MyWid("F1_1_2_3_4_bg", GET_FIVE_FOUR);
        tool_F_point5 = new MyWid("F1_1_2_3_5_bg", GET_FIVE_FIVE);
        List<Element> toolF_1_members = new List<Element>();
        toolF_1_members.Add(tool_F_point1); toolF_1_members.Add(tool_F_point2); toolF_1_members.Add(tool_F_point3);
        toolF_1_members.Add(tool_F_point4); toolF_1_members.Add(tool_F_point5);
        toolF_1 = new ButtonPart(toolF_1_members);

        toolF_SZ = new MyButton("F1_1_2_3_set", "F1123_s", TOOL_WUDIANFA);
        toolF_TC = new MyButton("F1_1_2_3_exit", "F1123_t", GONGJUZUOBIAO_FUNCTION_NO);
        List<Element> toolF_2_members = new List<Element>();
        toolF_2_members.Add(toolF_SZ); toolF_2_members.Add(toolF_TC);
        toolF_2 = new ButtonPart(toolF_2_members);
        List<Part> toolF_win_members = new List<Part>();
        toolF_win_members.Add(toolF_1); toolF_win_members.Add(toolF_2);
        toolF_win = new Window_depth(toolF_win_members, "Panel_F1_1_2_3", GONGJUZUOBIAO_FUNCTION_NO);


        //用户坐标系设置方法选择界面
        setUserNo = new MySprite("F1_1_3_N_bg", 1, "yellowbg");
        setUserF_1 = new OneButtonPart(setUserNo, USER_ZUOBIAOHAO);

        userDirect = new MyWid("F1_1_3_1bt");
        userDirect2 = new MyWid("F1_1_3_1bg");
        userThree = new MyWid("F1_1_3_2bt");
        userThree2 = new MyWid("F1_1_3_2bg");
        List<Element> userF = new List<Element>();
        userF.Add(userDirect); userF.Add(userThree);
        List<Element> userF2 = new List<Element>();
        userF2.Add(userDirect2); userF2.Add(userThree2);
        setUserF_2 = new OptionPart(userF, userF2);

        setUserXZ = new MyButton("F1_1_3_choose", "F113_c", YONGHUZUOBIAO_SET);
        setUserTC = new MyButton("F1_1_3_exit", "F113_t", ZHUYEMIAN_NO);
        List<Element> userF_xt = new List<Element>();
        userF_xt.Add(setUserXZ); userF_xt.Add(setUserTC);
        setUserF_3 = new ButtonPart(userF_xt);
        List<Part> userF_parts = new List<Part>();
        userF_parts.Add(setUserF_1); userF_parts.Add(setUserF_2); userF_parts.Add(setUserF_3);
        setUserF_win = new Window_depth(userF_parts, "Panel_F1_1_3", ZHUYEMIAN_NO);

        //直接输入法设置用户坐标
        user_X = new MyText("F1_1_3_1_X", "F1_1_3_1_X_bg", "4.2");
        user_Y = new MyText("F1_1_3_1_Y", "F1_1_3_1_Y_bg", "4.2");
        user_Z = new MyText("F1_1_3_1_Z", "F1_1_3_1_Z_bg", "4.2");
        user_W = new MyText("F1_1_3_1_W", "F1_1_3_1_W_bg", "4.2");
        user_P = new MyText("F1_1_3_1_P", "F1_1_3_1_P_bg", "4.2");
        user_R = new MyText("F1_1_3_1_R", "F1_1_3_1_R_bg", "4.2");
        List<Element> userD_1_members = new List<Element>();
        userD_1_members.Add(user_X); userD_1_members.Add(user_Y); userD_1_members.Add(user_Z);
        userD_1_members.Add(user_W); userD_1_members.Add(user_P); userD_1_members.Add(user_R);
        userD_1 = new TextPart(userD_1_members);

        userD_SZ = new MyButton("F1_1_3_1_set", "F1131_s", USER_ZHIJIESHURU);
        userD_TC = new MyButton("F1_1_3_1_exit", "F1131_t", YONGHUZUOBIAO_FUNCTION_NO);
        List<Element> userD_2_members = new List<Element>();
        userD_2_members.Add(userD_SZ); userD_2_members.Add(userD_TC);
        userD_2 = new ButtonPart(userD_2_members);
        List<Part> userD_win_members = new List<Part>();
        userD_win_members.Add(userD_1); userD_win_members.Add(userD_2);
        userD_win = new Window_depth(userD_win_members, "Panel_F1_1_3_1", YONGHUZUOBIAO_FUNCTION_NO);

        //三点法设置工具坐标
        user_T_point1 = new MyWid("F1_1_3_2_1_bg", GET_THREE_ONE);
        user_T_point2 = new MyWid("F1_1_3_2_2_bg", GET_THREE_TWO_);
        user_T_point3 = new MyWid("F1_1_3_2_3_bg", GET_THREE_THREE_);
        List<Element> userT_1_members = new List<Element>();
        userT_1_members.Add(user_T_point1); userT_1_members.Add(user_T_point2); userT_1_members.Add(user_T_point3);
        userT_1 = new ButtonPart(userT_1_members);

        userT_SZ = new MyButton("F1_1_3_2_set", "F1132_s", USER_SHANDIANFA);
        userT_TC = new MyButton("F1_1_3_2_exit", "F1132_t", YONGHUZUOBIAO_FUNCTION_NO);
        List<Element> userT_2_members = new List<Element>();
        userT_2_members.Add(userT_SZ); userT_2_members.Add(userT_TC);
        userT_2 = new ButtonPart(userT_2_members);
        List<Part> userT_win_members = new List<Part>();
        userT_win_members.Add(userT_1); userT_win_members.Add(userT_2);
        userT_win = new Window_depth(userT_win_members, "Panel_F1_1_3_2", YONGHUZUOBIAO_FUNCTION_NO);

        //模式切换界面
        operationMode = new MyWid("F1_1_6_1bt");
        operationMode2 = new MyWid("F1_1_6_1bg");
        editMode = new MyWid("F1_1_6_2bt");
        editMode2 = new MyWid("F1_1_6_2bg");
        managementMode = new MyWid("F1_1_6_3bt");
        managementMode2 = new MyWid("F1_1_6_3bg");
        List<Element> modeF_1_members = new List<Element>();
        modeF_1_members.Add(operationMode); modeF_1_members.Add(editMode); modeF_1_members.Add(managementMode);
        List<Element> modeF_1_members2 = new List<Element>();
        modeF_1_members2.Add(operationMode2); modeF_1_members2.Add(editMode2); modeF_1_members2.Add(managementMode2);
        modeF_1 = new OptionPart(modeF_1_members, modeF_1_members2);
        modeF_1.partNum = 1;//设置当前安全模式为编辑模式

        modeXZ = new MyButton("F1_1_6_choose", "F116_c",MOSHIQIEHUAN_FUNCTION);
        modeTC = new MyButton("F1_1_6_exit", "F116_t", ZHUYEMIAN_NO);
        List<Element> modeF_2_members = new List<Element>();
        modeF_2_members.Add(modeXZ);modeF_2_members.Add(modeTC);
        modeF_2 = new ButtonPart(modeF_2_members);
        List<Part> modeF_win_members = new List<Part>();
        modeF_win_members.Add(modeF_1); modeF_win_members.Add(modeF_2);
        modeF_win = new Window_depth(modeF_win_members, "Panel_F1_1_6", ZHUYEMIAN_NO);

        //系统速度界面
        speed_I = new MyText("F117_I", "F1_1_7_I", "3.0", 1, 10);
        speed_L = new MyText("F117_L", "F1_1_7_L", "3.0", 11, 25);
        speed_M = new MyText("F117_M", "F1_1_7_M", "3.0", 26, 50);
        speed_H = new MyText("F117_H", "F1_1_7_H", "3.0", 51, 75);
        speed_S = new MyText("F117_S", "F1_1_7_S", "3.0", 76, 100);
        speed_kaiji = new MyWid("F1_1_7_MM", XITONGSUDU_KAIJI);
        List<Element> speedF_1_members = new List<Element>();
        speedF_1_members.Add(speed_I); speedF_1_members.Add(speed_L); speedF_1_members.Add(speed_M);
        speedF_1_members.Add(speed_H); speedF_1_members.Add(speed_S); speedF_1_members.Add(speed_kaiji);
        speedF_1 = new TextPart(speedF_1_members);

        speed_SZ = new MyButton("F1_1_7_set", "F117_s", XITONGSUDU_SHEZHI);
        speed_MRZ = new MyButton("F1_1_7_mr", "F117_m", XITONGSUDU_MOREN);
        speed_TC = new MyButton("F1_1_7_exit", "F117_t", ZHUYEMIAN_NO);
        List<Element> speedF_2_members = new List<Element>();
        speedF_2_members.Add(speed_SZ); speedF_2_members.Add(speed_MRZ); speedF_2_members.Add(speed_TC);
        speedF_2 = new ButtonPart(speedF_2_members);
        List<Part> speedF_win_members = new List<Part>();
        speedF_win_members.Add(speedF_1); speedF_win_members.Add(speedF_2);
        speedF_win = new Window_depth(speedF_win_members, "Panel_F1_1_7", ZHUYEMIAN_NO);


        //程序管理
        //新建程序
        newBuildLine1 = new MySprite("F121_line1", 1, "greybackground");
        newBuildLine2 = new MySprite("F121_line2", 1, "greybackground");
        newBuildLine3 = new MySprite("F121_line3", 1, "greybackground");
        newBuildLine4 = new MySprite("F121_line4", 1, "greybackground");
        newBuildLine5 = new MySprite("F121_line5", 1, "greybackground");
        List<Element> newBuildPart_1_members = new List<Element>();
        newBuildPart_1_members.Add(newBuildLine1);newBuildPart_1_members.Add(newBuildLine2);newBuildPart_1_members.Add(newBuildLine3);
        newBuildPart_1_members.Add(newBuildLine4);newBuildPart_1_members.Add(newBuildLine5);
        newBuildPart_1 = new ScrollPart(newBuildPart_1_members, GSKDATA.ChengXuCount, "Scroll_F121");

        newBuildName = new MyText2("F121NN", "F121name", "filename", RUANJIANPAN);
        List<Element> newBuildPart_2_members = new List<Element>();
        newBuildPart_2_members.Add(newBuildName);
        newBuildPart_2 = new ButtonPart(newBuildPart_2_members);

        newBuild_XJ = new MyButton("F1_2_1_build", "F121_x", XINJIANCHENGXU);
        newBuild_TC = new MyButton("F121_exit", "F121_t", ZHUYEMIAN_NO);
        List<Element> newBuildPart_3_members = new List<Element>();
        newBuildPart_3_members.Add(newBuild_XJ); newBuildPart_3_members.Add(newBuild_TC);
        newBuildPart_3 = new ButtonPart(newBuildPart_3_members);
        List<Part> newBuild_win_members = new List<Part>();
        newBuild_win_members.Add(newBuildPart_1); newBuild_win_members.Add(newBuildPart_2); newBuild_win_members.Add(newBuildPart_3);
        newBuild_win = new Window_depth(newBuild_win_members, "Panel_F1_2_1", ZHUYEMIAN_NO);
        
        //软键盘
        ruanJianPanName = new MyText2("keyInput", "keyInputSprite", "filename", 1, "yellowbg", "greybackground");

        XiaoXie = new MyButton("XX", "LabelXX", 1);
        DaXie = new MyButton("DX", "LabelDX", 1);
        FuHao = new MyButton("FH", "LabelFH", 1);

        letterA = new MyButton("key00", "Labelk0", LETTER_A, "yellowbg", "lin");
        letterB = new MyButton("key01", "Labelk1", LETTER_B, "yellowbg", "lin");
        letterC = new MyButton("key02", "Labelk2", LETTER_C, "yellowbg", "lin");
        letterD = new MyButton("key03", "Labelk3", LETTER_D, "yellowbg", "lin");
        letterE = new MyButton("key04", "Labelk4", LETTER_E, "yellowbg", "lin");
        letterF = new MyButton("key05", "Labelk5", LETTER_F, "yellowbg", "lin");
        letterG = new MyButton("key10", "Labelk6", LETTER_G, "yellowbg", "lin");
        letterH = new MyButton("key11", "Labelk7", LETTER_H, "yellowbg", "lin");
        letterI = new MyButton("key12", "Labelk8", LETTER_I, "yellowbg", "lin");
        letterJ = new MyButton("key13", "Labelk9", LETTER_J, "yellowbg", "lin");
        letterK = new MyButton("key14", "Labelk10", LETTER_K, "yellowbg", "lin");
        letterL = new MyButton("key15", "Labelk11", LETTER_L, "yellowbg", "lin");
        letterM = new MyButton("key20", "Labelk12", LETTER_M, "yellowbg", "lin");
        letterN = new MyButton("key21", "Labelk13", LETTER_N, "yellowbg", "lin");
        letterO = new MyButton("key22", "Labelk14", LETTER_O, "yellowbg", "lin");
        letterP = new MyButton("key23", "Labelk15", LETTER_P, "yellowbg", "lin");
        letterQ = new MyButton("key24", "Labelk16", LETTER_Q, "yellowbg", "lin");
        letterR = new MyButton("key25", "Labelk17", LETTER_R, "yellowbg", "lin");
        letterS = new MyButton("key30", "Labelk18", LETTER_S, "yellowbg", "lin");
        letterT = new MyButton("key31", "Labelk19", LETTER_T, "yellowbg", "lin");
        letterU = new MyButton("key32", "Labelk20", LETTER_U, "yellowbg", "lin");
        letterV = new MyButton("key33", "Labelk21", LETTER_V, "yellowbg", "lin");
        letterW = new MyButton("key34", "Labelk22", LETTER_W, "yellowbg", "lin");
        letterX = new MyButton("key35", "Labelk23", LETTER_X, "yellowbg", "lin");
        letterY = new MyButton("key40", "Labelk24", LETTER_Y, "yellowbg", "lin");
        letterZ = new MyButton("key41", "Labelk25", LETTER_Z, "yellowbg", "lin");
        letter27 = new MyButton("key42", "Labelk26", 1, "yellowbg", "lin");
        letter28 = new MyButton("key43", "Labelk27", 1, "yellowbg", "lin");
        letter29 = new MyButton("key44", "Labelk28", 1, "yellowbg", "lin");
        letter30 = new MyButton("key45", "Labelk29", 1, "yellowbg", "lin");
        List<Element> letterPart_members = new List<Element>();
        letterPart_members.Add(letterA); letterPart_members.Add(letterB); letterPart_members.Add(letterC); letterPart_members.Add(letterD);
        letterPart_members.Add(letterE); letterPart_members.Add(letterF); letterPart_members.Add(letterG); letterPart_members.Add(letterH);
        letterPart_members.Add(letterI); letterPart_members.Add(letterJ); letterPart_members.Add(letterK); letterPart_members.Add(letterL);
        letterPart_members.Add(letterM); letterPart_members.Add(letterN); letterPart_members.Add(letterO); letterPart_members.Add(letterP);
        letterPart_members.Add(letterQ); letterPart_members.Add(letterR); letterPart_members.Add(letterS); letterPart_members.Add(letterT);
        letterPart_members.Add(letterU); letterPart_members.Add(letterV); letterPart_members.Add(letterW); letterPart_members.Add(letterX);
        letterPart_members.Add(letterY); letterPart_members.Add(letterZ); letterPart_members.Add(letter27); letterPart_members.Add(letter28);
        letterPart_members.Add(letter29); letterPart_members.Add(letter30);
        letterPart = new ButtonMulti(letterPart_members, 5, 6);
        List<Part> letterWin_members = new List<Part>();
        letterWin_members.Add(letterPart);
        letterWin = new Window_a(letterWin_members, "keyboard", XINJIANCHENGXU_FUNCTION_NO);


        //程序一览
        programV_line1 = new MySprite("F122_line1", 1);
        programV_line2 = new MySprite("F122_line2", 1);
        programV_line3 = new MySprite("F122_line3", 1);
        programV_line4 = new MySprite("F122_line4", 1);
        programV_line5 = new MySprite("F122_line5", 1);
        programV_line6 = new MySprite("F122_line6", 1);
        programV_line7 = new MySprite("F122_line7", 1);
        List<Element> programVPart_1_members = new List<Element>();
        programVPart_1_members.Add(programV_line1); programVPart_1_members.Add(programV_line2); programVPart_1_members.Add(programV_line3);
        programVPart_1_members.Add(programV_line4); programVPart_1_members.Add(programV_line5); programVPart_1_members.Add(programV_line6);
        programVPart_1_members.Add(programV_line7);
        programVPart_1 = new ScrollPart(programVPart_1_members, GSKDATA.ChengXuCount, "Scroll_F122");

        newName = new MyText2("F122NN", "F122name", "filename", RUANJIANPAN2);
        List<Element> programVPart_2_members = new List<Element>();
        programVPart_2_members.Add(newName);
        programVPart_2 = new ButtonPart(programVPart_2_members);

        programV_copy = new MyButton("F122_bt1", "F122_1", COPY_PROGRAM);
        programV_delete = new MyButton("F122_bt2", "F122_2", DELETE_PROGRAM);
        programV_search = new MyButton("F122_bt3", "F122_3", SEARCH_PROGRAM);
        programV_rename = new MyButton("F122_bt4", "F122_4", RENAME_PROGRAM);
        programV_exit = new MyButton("F122_bt5", "F122_5", ZHUYEMIAN_NO);
        List<Element> programVPart_3_members = new List<Element>();
        programVPart_3_members.Add(programV_copy); programVPart_3_members.Add(programV_delete); programVPart_3_members.Add(programV_search);
        programVPart_3_members.Add(programV_rename); programVPart_3_members.Add(programV_exit);
        programVPart_3 = new ButtonPart(programVPart_3_members);
        List<Part> programV_win_members = new List<Part>();
        programV_win_members.Add(programVPart_1);programV_win_members.Add(programVPart_2);programV_win_members.Add(programVPart_3);
        programV_win = new Window_depth(programV_win_members, "Panel_F1_2_2", ZHUYEMIAN_NO);

        //在线帮助
        //指令
        ZL_movj = new MyWid("F1101V1");
        ZL_movl = new MyWid("F1101V2");
        ZL_movc = new MyWid("F1101V3");
        ZL_dout = new MyWid("F1101V4");
        ZL_din = new MyWid("F1101V5");
        ZL_delay = new MyWid("F1101V6");
        ZL_wait = new MyWid("F1101V7");
        ZL_label = new MyWid("F1101V8");
        ZL_jump = new MyWid("F1101V9");
        ZL_end = new MyWid("F1101V10");
        List<Element> ZL_Part_members = new List<Element>();
        ZL_Part_members.Add(ZL_movj); ZL_Part_members.Add(ZL_movl); ZL_Part_members.Add(ZL_movc);
        ZL_Part_members.Add(ZL_dout); ZL_Part_members.Add(ZL_din); ZL_Part_members.Add(ZL_delay);
        ZL_Part_members.Add(ZL_wait); ZL_Part_members.Add(ZL_label); ZL_Part_members.Add(ZL_jump);
        ZL_Part_members.Add(ZL_end);
        ZL_Part = new LabelPart(ZL_Part_members,ZHILING_LABEL);
        List<Part> ZL_win_members = new List<Part>();
        ZL_win_members.Add(ZL_Part);
        ZL_win = new Window_depth(ZL_win_members, "Panel_F1_10_1", ZHUYEMIAN_NO);

        //操作
        CZ_lingdian = new MyWid("F1101V12");
        List<Element> CZ_Part_members = new List<Element>();
        CZ_Part_members.Add(CZ_lingdian);
        CZ_Part = new LabelPart(CZ_Part_members, CAOZUO_LABEL);
        List<Part> CZ_win_members = new List<Part>();
        CZ_win_members.Add(CZ_Part);
        CZ_win = new Window_depth(CZ_win_members, "Panel_F1_10_2", ZHUYEMIAN_NO);

              
        //程序界面部分
        ProgramWin = new ProgramWinClass("Panel_F3", 1, FILEPATH+GSKDATA.CurrentProgramName);
        //删除界面
        DeleteYes = new MyButton("Dyes", "DLy", DELETE_YES, "r");
        DeleteNo = new MyButton("Dno", "DLn", DELETE_NOO, "r");
        List<Element> deletepart_members = new List<Element>();
        deletepart_members.Add(DeleteYes);
        deletepart_members.Add(DeleteNo);
        DeletePart = new ButtonPart(deletepart_members);
        List<Part> deletewin_members = new List<Part>();
        deletewin_members.Add(DeletePart);
        DeleteWin = new Window_a(deletewin_members, "DeleteLabel", CHENGXU_NO);
        //示教点重复界面
        PointRepeatYes = new MyButton("Pyes", "PLy", POINTREPEAT_YES, "r");
        PointRepeatNo = new MyButton("Pno", "PLn", POINTREPEAT_NO, "r");
        List<Element> pointrepeatpart_members = new List<Element>();
        pointrepeatpart_members.Add(PointRepeatNo);
        pointrepeatpart_members.Add(PointRepeatYes);
        PointRepeatPart = new ButtonPart(pointrepeatpart_members);
        List<Part> pointrepeatwin_members = new List<Part>();
        pointrepeatwin_members.Add(PointRepeatPart);
        PointRepeatWin = new Window_a(pointrepeatwin_members, "PointLabel", BIANJI_NO);
        //添加界面
        AddButton1 = new MyButton("F31", "LabelF31", ADD_1, "yellowbg", "w1");
        AddButton2 = new MyButton("F32", "LabelF32", ADD_2, "yellowbg", "w1");
        AddButton3 = new MyButton("F33", "LabelF33", ADD_3, "yellowbg", "w1");
        AddButton4 = new MyButton("F34", "LabelF34", ADD_4, "yellowbg", "w1");
        AddButton5 = new MyButton("F35", "LabelF35", ADD_5, "yellowbg", "w1");
        AddButton6 = new MyButton("F36", "LabelF36", ADD_6, "yellowbg", "w1");
        List<Element> addpart_members = new List<Element>();
        addpart_members.Add(AddButton1); addpart_members.Add(AddButton2); addpart_members.Add(AddButton3);
        addpart_members.Add(AddButton4); addpart_members.Add(AddButton5); addpart_members.Add(AddButton6);
        AddPart = new ButtonPart(addpart_members);
        List<Part> addwin_member = new List<Part>();
        addwin_member.Add(AddPart);
        AddWin = new Window_alpha(addwin_member, "Addorder", BIANJI_MODIFY);
        //ADD1界面
        AddButton_movj = new MyButton("F311", "LabelF311", ADD_MOVJ, "yellowbg", "w1");
        AddButton_movl = new MyButton("F312", "LabelF312", ADD_MOVL, "yellowbg", "w1");
        AddButton_movc = new MyButton("F313", "LabelF313", ADD_MOVC, "yellowbg", "w1");
        List<Element> addpart1_members = new List<Element>();
        addpart1_members.Add(AddButton_movj); addpart1_members.Add(AddButton_movl); addpart1_members.Add(AddButton_movc);
        AddPart1 = new ButtonPart(addpart1_members);
        List<Part> addwin1_member = new List<Part>();
        addwin1_member.Add(AddPart1);
        AddWin1 = new Window_alpha(addwin1_member, "F31order", ADD);
        //ADD2界面
        AddButton_dout = new MyButton("F321", "LabelF321", ADD_DOUT, "yellowbg", "w1");
        AddButton_din = new MyButton("F322", "LabelF322", ADD_DIN, "yellowbg", "w1");
        AddButton_delay = new MyButton("F323", "LabelF323", ADD_DELAY, "yellowbg", "w1");
        AddButton_wait = new MyButton("F324", "LabelF324", ADD_WAIT, "yellowbg", "w1");
        AddButton_pulse = new MyButton("F325", "LabelF325", ADD_PULSE, "yellowbg", "w1");
        List<Element> addpart2_members = new List<Element>();
        addpart2_members.Add(AddButton_dout); addpart2_members.Add(AddButton_din); addpart2_members.Add(AddButton_delay);
        addpart2_members.Add(AddButton_wait); addpart2_members.Add(AddButton_pulse);
        AddPart2 = new ButtonPart(addpart2_members);
        List<Part> addwin2_member = new List<Part>();
        addwin2_member.Add(AddPart2);
        AddWin2 = new Window_alpha(addwin2_member, "F32order", ADD);
        //ADD3界面
        AddButton_lab = new MyButton("F331", "LabelF331", ADD_LAB, "yellowbg", "w1");
        AddButton_jump = new MyButton("F332", "LabelF332", ADD_JUMP, "yellowbg", "w1");
        AddButton_jump_r = new MyButton("F333", "LabelF333", ADD_JUMP_R, "yellowbg", "w1");
        AddButton_jump_in = new MyButton("F334", "LabelF334", ADD_JUMP_IN, "yellowbg", "w1");
        AddButton_jh = new MyButton("F335", "LabelF335", ADD_JH, "yellowbg", "w1");
        AddButton_end = new MyButton("F336", "LabelF336", ADD_END, "yellowbg", "w1");
        AddButton_main = new MyButton("F337", "LabelF337", ADD_MAIN, "yellowbg", "w1");
        AddButton_call = new MyButton("F338", "LabelF338", ADD_CALL, "yellowbg", "w1");
        List<Element> addpart3_members = new List<Element>();
        addpart3_members.Add(AddButton_lab); addpart3_members.Add(AddButton_jump); addpart3_members.Add(AddButton_jump_r);
        addpart3_members.Add(AddButton_jump_in); addpart2_members.Add(AddButton_jh); addpart3_members.Add(AddButton_end);
        addpart3_members.Add(AddButton_main); addpart3_members.Add(AddButton_call);
        AddPart3 = new ButtonPart(addpart3_members);
        List<Part> addwin3_member = new List<Part>();
        addwin3_member.Add(AddPart3);
        AddWin3 = new Window_alpha(addwin3_member, "F33order", ADD);
        //ADD4界面
        AddButton_r = new MyButton("F341", "LabelF341", ADD_R, "yellowbg", "w1");
        AddButton_inc = new MyButton("F342", "LabelF342", ADD_INC, "yellowbg", "w1");
        AddButton_dec = new MyButton("F343", "LabelF343", ADD_DEC, "yellowbg", "w1");
        List<Element> addpart4_members = new List<Element>();
        addpart4_members.Add(AddButton_r); addpart4_members.Add(AddButton_inc); addpart4_members.Add(AddButton_dec);
        AddPart4 = new ButtonPart(addpart4_members);
        List<Part> addwin4_member = new List<Part>();
        addwin4_member.Add(AddPart4);
        AddWin4 = new Window_alpha(addwin4_member, "F34order", ADD);
        //ADD5界面
        AddButton_px = new MyButton("F351", "LabelF351", ADD_PX, "yellowbg", "w1");
        AddButton_shifton = new MyButton("F352", "LabelF352", ADD_SHIFTON, "yellowbg", "w1");
        AddButton_shiftoff = new MyButton("F353", "LabelF353", ADD_SHIFTOFF, "yellowbg", "w1");
        AddButton_mshift = new MyButton("F354", "LabelF354", ADD_MSHIFT, "yellowbg", "w1");
        List<Element> addpart5_members = new List<Element>();
        addpart5_members.Add(AddButton_px); addpart5_members.Add(AddButton_shifton); addpart5_members.Add(AddButton_shiftoff);
        addpart5_members.Add(AddButton_mshift);
        AddPart5 = new ButtonPart(addpart5_members);
        List<Part> addwin5_member = new List<Part>();
        addwin5_member.Add(AddPart5);
        AddWin5 = new Window_alpha(addwin5_member, "F35order", ADD);
        //ADD6界面
        AddButton_arcon = new MyButton("F361", "LabelF361", ADD_ARCON, "yellowbg", "w1");
        AddButton_arcof = new MyButton("F362", "LabelF362", ADD_ARCOF, "yellowbg", "w1");
        List<Element> addpart6_members = new List<Element>();
        addpart6_members.Add(AddButton_arcon); addpart6_members.Add(AddButton_arcof); 
        AddPart6 = new ButtonPart(addpart6_members);
        List<Part> addwin6_member = new List<Part>();
        addwin6_member.Add(AddPart6);
        AddWin6 = new Window_alpha(addwin6_member, "F36order", ADD);
        //程序启动界面
        StartButton_yes = new MyButton("F2Dy", "F2Ly", START_YES, "r","whitebg");
        StartButton_no = new MyButton("F2Dn", "F2Ln", START_NO, "r", "whitebg");
        List<Element> StartPart_members = new List<Element>();
        StartPart_members.Add(StartButton_no);
        StartPart_members.Add(StartButton_yes);
        StartPart = new ButtonPart(StartPart_members);
        List<Part> startwin_members = new List<Part>();
        startwin_members.Add(StartPart);
        StartWin = new Window_a(startwin_members, "appearW", CHENGXU_NO);

        //显示界面
        F4Window = new Window_depth_no("Panel_F4",ZHUYEMIAN_NO);
        //工具界面
        F5Window = new Window_depth_no("Panel_F5", ZHUYEMIAN_NO); 

        #endregion
        //设置当前窗口
        CurrentWindow = F1window;
        CurrentWindow.isChosen();
        zhuYeMian_InfoSet();
        detectresult = GameObject.Find("MainScript").GetComponent<ResultDetect>();
        ServerCall = GameObject.Find("Network").GetComponent<ServerCenter>();
        wlscript = GameObject.Find("CameraFree").GetComponent<WeldLine>();

        interpreter = new GSKInterpreter(FILEPATH);
    }	
	// Update is called once per frame
	void Update () {
        //连续运行
        if (GSKDATA.GoPress)
        {
            if (GSKDATA.ActionCycle)//如果连续
            {
                if (!GSKDATA.AxisRunning)
                {
                    RunProgram(0);
                    //Debug.Log("运行下一行");
                }
            }
        }
        if (GSKDATA.BackPress)
        {
            if (GSKDATA.ActionCycle)//如果连续
            {
                if (!GSKDATA.AxisRunning)
                {
                    RunProgram(1);
                }
            }
        }
        if (GSKDATA.ReappearRun)//reappear
        {
            if (!GSKDATA.AxisRunning)
            {
                if (GSKDATA.SoftCurrentMode == "Teach")
                {
                    if (FuncPara.isPlaying && GSKDATA.RBP_result != "END")
                    {
                        RunProgram(0);// reappear can stop when teach
                    }
                }
                else
                {
                    RunProgram(0);
                }
            }
        }

        //等待信号输入
        if (GSKDATA.WaitInput != -1)
        {
            if (GSKDATA.InInfo[GSKDATA.WaitInput] && GSKDATA.WaitInputState == 1)
            {
                GSKDATA.AxisRunning = false;
                GSKDATA.WaitInput = -1;
            }
            else if (!GSKDATA.InInfo[GSKDATA.WaitInput] && GSKDATA.WaitInputState == 0)
            {
                GSKDATA.AxisRunning = false;
                GSKDATA.WaitInput = -1;
            }
        }

        for(int i = 0; i < GSKDATA.IO_MAX_NUM; i++)
        {
            GSKDATA.OutInfo[i] = (interpreter.getOutState(i) == 1) ? true : false;
            GSKDATA.InInfo[i] = (interpreter.getInState(i) == 1) ? true : false;
        }
	}

    void OnGUI()
    {
        GameObject.Find("CurrentPosition").GetComponent<UILabel>().text = GSKDATA.CurrentScreenPositon;//当前位置
        GameObject.Find("Programname").GetComponent<UILabel>().text = GSKDATA.CurrentProgramName;//当前程序名
        float[] angle; float[] position;
        if (GameObject.Find("Panel_F1").GetComponent<UIPanel>().depth == 1)
        {
            //时间的显示
            GameObject.Find("Day").GetComponent<UILabel>().text = System.DateTime.Now.ToString("yyyy-MM-dd");
            GameObject.Find("Hour").GetComponent<UILabel>().text = System.DateTime.Now.ToString("HH:mm:ss");
            //主页面上的位姿与关节实际位置显示
            angle = MotionScript.CurrentAngle_All();
            position = MotionScript.PostureAndPosition();

            GameObject.Find("S").GetComponent<UILabel>().text = angle[0].ToString("0.00");
            GameObject.Find("L").GetComponent<UILabel>().text = angle[1].ToString("0.00");
            GameObject.Find("U").GetComponent<UILabel>().text = angle[2].ToString("0.00");
            GameObject.Find("R2").GetComponent<UILabel>().text = angle[3].ToString("0.00");
            GameObject.Find("B").GetComponent<UILabel>().text = angle[4].ToString("0.00");
            GameObject.Find("T").GetComponent<UILabel>().text = angle[5].ToString("0.00");
            GameObject.Find("X").GetComponent<UILabel>().text = position[0].ToString("0.00");
            GameObject.Find("Y").GetComponent<UILabel>().text = position[1].ToString("0.00");
            GameObject.Find("Z").GetComponent<UILabel>().text = position[2].ToString("0.00");
            GameObject.Find("W").GetComponent<UILabel>().text = position[3].ToString("0.00");
            GameObject.Find("P").GetComponent<UILabel>().text = position[4].ToString("0.00");
            GameObject.Find("R").GetComponent<UILabel>().text = position[5].ToString("0.00");
        }
        else if (GameObject.Find("Panel_F3").GetComponent<UIPanel>().depth == 1)
        {
            //程序页面上的位姿与关节实际位置显示
            angle = MotionScript.CurrentAngle_All();
            position = MotionScript.PostureAndPosition();
            GameObject.Find("F3_S").GetComponent<UILabel>().text = angle[0].ToString("0.00");
            GameObject.Find("F3_L").GetComponent<UILabel>().text = angle[1].ToString("0.00");
            GameObject.Find("F3_U").GetComponent<UILabel>().text = angle[2].ToString("0.00");
            GameObject.Find("F3_R2").GetComponent<UILabel>().text = angle[3].ToString("0.00");
            GameObject.Find("F3_B").GetComponent<UILabel>().text = angle[4].ToString("0.00");
            GameObject.Find("F3_T").GetComponent<UILabel>().text = angle[5].ToString("0.00");
            GameObject.Find("F3_X").GetComponent<UILabel>().text = position[0].ToString("0.00");
            GameObject.Find("F3_Y").GetComponent<UILabel>().text = position[1].ToString("0.00");
            GameObject.Find("F3_Z").GetComponent<UILabel>().text = position[2].ToString("0.00");
            GameObject.Find("F3_W").GetComponent<UILabel>().text = position[3].ToString("0.00");
            GameObject.Find("F3_P").GetComponent<UILabel>().text = position[4].ToString("0.00");
            GameObject.Find("F3_R").GetComponent<UILabel>().text = position[5].ToString("0.00");
        }

    }   
    public void OnStart()
    {
        if (GSKDATA.RobotMode == 1)//再现模式
        {
            if (GSKDATA.IsReady)
            {
                if (GameObject.Find("Panel_F3").GetComponent<UIPanel>().depth == 1)
                {
                    GameObject.Find("Start").GetComponent<UISprite>().spriteName = "U3-1";
                    GSKDATA.RBP_result = "";
                    GSKDATA.VList.Clear();//清空路径点
                    GSKDATA.nextNUM[0] = ProgramWin.fline * 10 + ProgramWin.line;
                    //show window "confirm appear"
                    if (!GSKDATA.ReappearRun)
                    {
                        StartPart.partNum = 0;
                        SetCurrentWin(StartWin);
                        ProgramWin.isChosen();
                    }
                }
                else
                {
                    StartCoroutine(WarningYellow("请进入程序界面"));
                }
            }
            else
            {
                StartCoroutine(WarningYellow("请先上开使能"));
            }
        }
        else
        {
            StartCoroutine(WarningYellow("请切换到[再现模式]"));
        }
    }
    public void OnStartUp()
    {
        GameObject.Find("Start").GetComponent<UISprite>().spriteName = "U3";
    }
    public void OnStop()
    {
        if (GSKDATA.RobotMode == 1)
        {
            GameObject.Find("Stop").GetComponent<UISprite>().spriteName = "U2-1";
            GSKDATA.ReappearRun = false;
        }
    }
    public void OnStopUp()
    {
        GameObject.Find("Stop").GetComponent<UISprite>().spriteName = "U2";
    }
    public void OnScram()
    {
        GSKDATA.Scram = !GSKDATA.Scram;
        if (GSKDATA.Scram)
        {
            GameObject.Find("Scram").GetComponent<UIButton>().normalSprite = "U1-1";
            GameObject.Find("State").GetComponent<UISprite>().spriteName = "Scraming";
            WarningRed("紧急停止");
        }
        else
        {
            GameObject.Find("Scram").GetComponent<UIButton>().normalSprite = "U1";
            GameObject.Find("State").GetComponent<UISprite>().spriteName = "Stoping";
            ClearRedWarning();
        }
    }
    public void OnReappear()
    {
        if (CurrentWindow == ProgramWin && F1part1.partNum == 1)
        {
            GSKDATA.RobotMode = 1;
            setModeIcon();
        }
        else
        {
            StartCoroutine(WarningYellow("请先切换到程序界面"));
        }
    }
    public void OnTeach()
    {
        if (GSKDATA.RobotMode == 1)
        {
            //退出再现
            ExitReappear();
        }
        GSKDATA.RobotMode = 2;
        setModeIcon();
    }  
    public void OnRemote()
    {
        if (GSKDATA.RobotMode == 1)
        {
            //退出再现
            ExitReappear();
        }
        GSKDATA.RobotMode = 3;
        setModeIcon();
    }
    public void OnF1()
    {
        if (GSKDATA.RobotMode == 2)
        {
            if (CurrentWindow == ProgramWin || CurrentWindow == F1window || CurrentWindow == F4Window || CurrentWindow == F5Window)
            {
                F1part1.partNum = 0;
                F1part1.isChosen();
                SetCurrentWin(F1window);
            }
        }
        else
        {
            StartCoroutine(WarningYellow("请先切换到示教模式"));
        }
    }
    public void OnF2()
    {
        if (CurrentWindow == F1window || (CurrentWindow == ProgramWin && F1part1.partNum == 2) || CurrentWindow == F4Window || CurrentWindow == F5Window)
        {
            F1part1.partNum = 1;
            F1part1.isChosen(); 
            SetCurrentWin(ProgramWin);
            ProgramWin.onCancelBt();//取消修改窗口
            GSKDATA.CurrentScreenPositon = "";
            GrammarTodata();
        }
        
    }
    public void OnF3()
    {
        if (GSKDATA.RobotMode == 2)
        {
            if (CurrentWindow == F1window || (CurrentWindow == ProgramWin && F1part1.partNum == 1) || CurrentWindow == F4Window || CurrentWindow == F5Window)
            {
                
                JumpToEdit();
            }
        }
        else
        {
            StartCoroutine(WarningYellow("请先切换到示教模式"));
        }
    }
    public void OnF4()
    {
        if (GSKDATA.RobotMode == 2)
        {
            if (CurrentWindow == F1window || CurrentWindow == ProgramWin || CurrentWindow == F5Window)
            {
                F1part1.partNum = 3;
                F1part1.isChosen();
                SetCurrentWin(F4Window);
                GSKDATA.CurrentScreenPositon = "";
            }
        }
        else
        {
            StartCoroutine(WarningYellow("请先切换到示教模式"));
        }
        
    }
    public void OnF5()
    {
        if (GSKDATA.RobotMode == 2)
        {
            if (CurrentWindow == F1window || CurrentWindow == ProgramWin || CurrentWindow == F4Window)
            {
                F1part1.partNum = 4;
                F1part1.isChosen();
                SetCurrentWin(F5Window);
                GSKDATA.CurrentScreenPositon = "";
            }
        }
        else
        {
            StartCoroutine(WarningYellow("请先切换到示教模式"));
        }
    }
    public void OnTab()
    {
       int tab_result = CurrentWindow.onTabBt();
       if (tab_result < 0)//发生错误
       {
           DirectionKeyWarning(tab_result);
       }
    }
    public void OnUp()
    {
        if (GSKDATA.RobotMode != 2)
        {
            StartCoroutine(WarningYellow("请先切换到示教模式"));
            return;
        }
        int up_result = CurrentWindow.onUpBt();
        if (up_result < 0)//发生错误
        {
            DirectionKeyWarning(up_result);
        }
        else if (up_result != 1)
        {
            switch (up_result)
            {
                case TOOL_ZUOBIAOHAO:
                    //tool number plus
                    currentToolNumPlus(true);
                    break;
                case USER_ZUOBIAOHAO:
                    currentUserNumPlus(true);
                    break;
                case ZHILING_LABEL:
                    zhiLingHelp();
                    break;
                case UPDATELIST:
                    updateList_UP_DOWN();//更新列表
                    break;
            }
        }
        else
        {
            if (CurrentWindow == ProgramWin)
            {
                GSKDATA.AxisRunning = false;
            }
        }
        GSKDATA.nextNUM[0] = ProgramWin.fline * 10 + ProgramWin.line;
        
    }
    public void OnDown()
    {
        if (GSKDATA.RobotMode != 2)
        {
            StartCoroutine(WarningYellow("请先切换到示教模式"));
            return;
        }
        int down_result = CurrentWindow.onDownBt();
        if (down_result < 0)//发生错误
        {
            DirectionKeyWarning(down_result);
        }
        else if (down_result != 1)
        {
            switch (down_result)
            {
                case TOOL_ZUOBIAOHAO:
                    //tool number plus
                    currentToolNumPlus(false);
                    break;
                case USER_ZUOBIAOHAO:
                    currentUserNumPlus(false);
                    break;
                case ZHILING_LABEL:
                    zhiLingHelp();
                    break;
                case UPDATELIST:
                    updateList_UP_DOWN();//更新列表
                    break;
                
            }
        }
        else
        {
            if (CurrentWindow == ProgramWin)
            {
                GSKDATA.AxisRunning = false;
            }
        }
        GSKDATA.nextNUM[0] = ProgramWin.fline * 10 + ProgramWin.line;
    }
    public void OnRight()
    {
        int right_result = CurrentWindow.onRightBt();
        //Debug.Log(right_result);
        if (right_result <0)//发生错误
        {
            DirectionKeyWarning(right_result);
        }
        else if (right_result != 1)
        {
            switch (right_result / 100)
            {
                case 1://从十个二级菜单返回到主页面
                    switch (right_result)
                    {
                        case ZHUYEMIAN_NO:
                            SetCurrentWin(F1window);
                            break;
                    }
                    break;
                case 3://F3界面
                    ChooseResult_3(right_result);
                    break;
            }
        }
        
    }
    public void OnLeft()
    {
        int left_result = CurrentWindow.onLeftBt();
        Debug.Log(left_result);
        if (left_result < 0)//发生错误
        {
            DirectionKeyWarning(left_result);
        }
        else if (left_result != 1)
        {
            switch (left_result / 100)
            {
                case 1:
                    switch (left_result)
                    {
                        case ZHUYEMIAN_NO://从十个二级菜单返回到主页面
                            SetCurrentWin(F1window);
                            break;
                    }
                    break;
                case 3://F3界面
                    ChooseResult_3(left_result);
                    break;
            }
        }
    }
    public void OnReady()
    {
        if (GSKDATA.RobotMode == 1)
        {
            GSKDATA.IsReady = !GSKDATA.IsReady;

            if (GSKDATA.IsReady)
            {
                GameObject.Find("Ready").GetComponent<UIButton>().normalSprite = "_11";
            }
            else
            {
                GameObject.Find("Ready").GetComponent<UIButton>().normalSprite = "11";
            }
        }
        else
        {
            //提示该模式下无效
            StartCoroutine(WarningYellow("请切换到[再现模式]"));
        }
    }
    public void OnChoose()
    {
        int choose_result = CurrentWindow.onChooseBt();
        //Debug.Log(choose_result);
        if (choose_result != 1)
        {
            switch (choose_result / 100)
            {
                case 1://F1界面的十个选项按钮
                    ChooseResult_1(choose_result);
                    break;
                case 2://f2界面
                    ChooseResult_2(choose_result);
                    break;
                case 3://F3界面
                    ChooseResult_3(choose_result);
                    break;
                case 4://显示界面
                    ChooseResult_4(choose_result);
                    break;
                case 5://工具界面
                    ChooseResult_5(choose_result);
                    break;
                case 11://系统设置按钮
                    ChooseResult_11(choose_result);
                    break;
                case 12://程序管理按钮
                    ChooseResult_12(choose_result);
                    break;
                case 10://再现帮助
                    ChooseResult_10(choose_result);
                    break;
                case 20://软键盘按钮
                    ChooseResult_20(choose_result);
                    break;
                case 2149: //调出软键盘
                    ChooseResult_214(XINJIANCHENGXU_FUNCTION_NO);                    
                    break;
                case 2159: //调出软键盘
                    ChooseResult_214(CHENGXUYILAN_FUNCTION_NO);
                    break;

            }
        }
    }

    public void OnSwitch()
    {
        int choose_result = CurrentWindow.onSwitch();
    }
    public void OnCancel()
    {
        int cancel_result = CurrentWindow.onCancelBt();
        Debug.Log(cancel_result);
        if (cancel_result != 1)
        {
            switch (cancel_result / 100)
            {
                case 1://F1界面的十个选项按钮
                    ChooseResult_1(cancel_result);
                    break;
                case 2://f2界面
                    ChooseResult_2(cancel_result);
                    break;
                case 3://F3界面
                    ChooseResult_3(cancel_result);
                    break;
                case 11://系统设置按钮
                    ChooseResult_11(cancel_result);
                    break;
                case 12:
                    ChooseResult_12(cancel_result);
                    break;
               
            }
        }
    }
    public void OnClear()
    {
        if (!GSKDATA.Scram)
        {
            ClearRedWarning();
        }
    }
    public void OnInput()
    {
        int input_result = CurrentWindow.onInputBt();
        //Debug.Log(input_result);
        switch (input_result)
        {
            case 1:
                StartCoroutine(WarningYellow("输入完成"));
                break;
            case 2:
                StartCoroutine(WarningYellow("输入值不能为空"));
                break;
            case POINTREPEAT://提示示教点重复
                //pointRepeatHint();
                SetCurrentWin(PointRepeatWin);
                ProgramWin.issChosen();
                ServerCall.PointRepeatWin_RPC();
                //PAD
                break;
            case 214://软键盘
                ruanJianPanName.onInputBt();
                if (letterWin.lastWin == XINJIANCHENGXU_FUNCTION_NO)
                {
                    newBuildName.label.text = ruanJianPanName.label.text;
                    newBuildName.tempstr = string.Empty;
                    SetCurrentWin(newBuild_win);
                }
                else
                {
                    newName.label.text = ruanJianPanName.label.text;
                    newName.tempstr = string.Empty;
                    SetCurrentWin(programV_win);
                }
                
                StartCoroutine(WarningYellow("输入完成"));
                break;
            default:
                int min = input_result / 1000;
                int max = input_result - min * 1000;
                StartCoroutine(WarningYellow("输入范围["+min+","+max+"]"));
                break;
        }
        
    }
    public void OnNumber(string num)
    {
        string number_result = CurrentWindow.onNumBt(num);
        Debug.Log(number_result);
        if (number_result != "ok")
        {
            if (number_result == "214")
            {
                number_result = ruanJianPanName.onNumBt(num);
                if(number_result != "ok")
                    StartCoroutine(WarningYellow(number_result));
            }else
                StartCoroutine(WarningYellow(number_result));
        }

    }
    public void OnLeft_()
    {
        int _left_result = CurrentWindow.on_LeftBt();
        if (_left_result != 1)
        {
            switch (_left_result)
            {
                case 214://软键盘
                    ruanJianPanName.on_LeftBt();
                    break;
            }
        }
    }
    public void OnCopy()
    {
        int copy_result = 0;
        if (CurrentWindow == ProgramWin && F1part1.partNum == 2)//在程序界面
        {
            copy_result = ProgramWin.CopyFunction();
            Debug.Log("copy_result" + copy_result);
        }
        //复制提示
        switch (copy_result)
        {
            case -5:
                StartCoroutine(WarningYellow("首行不可以复制"));
                break;
            case -6:
                StartCoroutine(WarningYellow("末行不可以复制"));
                break;
            case -7:
                StartCoroutine(WarningYellow("粘贴位置不对"));
                break;
            case 1:
                switch (ProgramWin.copy)
                {
                    case 1:
                        StartCoroutine(WarningYellow("选择复制区域"));
                        break;
                    case 2:
                        StartCoroutine(WarningYellow("选择粘贴行"));
                        break;
                    case 3:
                        StartCoroutine(WarningYellow("复制完成"));
                        break;
                }
                break;
        }
    }
    public void OnCut()
    {
        int cut_result = 0;
        if (CurrentWindow == ProgramWin && F1part1.partNum == 2)//在程序界面
        {
            cut_result = ProgramWin.CutFunction();
        }
        //复制提示
        switch (cut_result)
        {
            case -5:
                StartCoroutine(WarningYellow("首行不可以剪切"));
                break;
            case -6:
                StartCoroutine(WarningYellow("末行不可以剪切"));
                break;
            case -7:
                StartCoroutine(WarningYellow("粘贴位置不对"));
                break;
            case 1:
                switch (ProgramWin.cut)
                {
                    case 1:
                        StartCoroutine(WarningYellow("选择剪切区域"));
                        break;
                    case 2:
                        StartCoroutine(WarningYellow("选择粘贴行"));
                        break;
                    case 3:
                        StartCoroutine(WarningYellow("剪切完成"));
                        break;
                }
                break;
        }
    }
    public void OnDelete()
    {
        int delete_result = 0;
        if (CurrentWindow == ProgramWin && F1part1.partNum == 2)//在编辑界面
        {
            delete_result = ProgramWin.DeleteChoose();
            ProgramWin.cut = 0;
            ProgramWin.copy = 0;
        }
        switch (delete_result)
        {
            case -1:
                StartCoroutine(WarningYellow("首行不能删除"));
                break;
            case -2:
                StartCoroutine(WarningYellow("尾行不能删除"));
                break;
            case 220:
                DeletePart.partNum = 0;//删除窗口显示
                SetCurrentWin(DeleteWin);
                ProgramWin.isChosen();
                break;
        }
    }
    public void OnModify()
    {
        if (CurrentWindow == ProgramWin && F1part1.partNum == 2)//在编辑界面
        {
            ProgramWin.ModifyFunction();
        }
    }

    public void OnAdd()
    {
        if (CurrentWindow == ProgramWin && F1part1.partNum == 2)//在编辑界面
        {
            if (ProgramWin.fline * 10 + ProgramWin.line == ProgramWin.fileContents.Count - 1)
            {
                StartCoroutine(WarningYellow("当前位置不能添加程序"));
            }
            else
            {
                AddPart.partNum = 0;
                SetCurrentWin(AddWin);
                ProgramWin.isChosen();
            }
        }
    }

    public void OnGet()
    {
        int get_result = CurrentWindow.onGet();
        switch (get_result)
        {
            case -1:
                StartCoroutine(WarningYellow("获取示教点成功"));
                break;
            default:
                updatePointInfo(get_result);//更新示教点信息
                StartCoroutine(WarningYellow("获取示教点成功"));
                break;
            
        }
    }

    public void OnSpeedUp()
    {
        GSKDATA.ManualSpeed = GSKDATA.ManualSpeedR.SpeedAdd();
        setSpeedIcon();//速度图标变化
    }
    public void OnSpeedDown()
    {
        GSKDATA.ManualSpeed = GSKDATA.ManualSpeedR.SpeedMinus();
        setSpeedIcon();//速度图标变化
    }
    public void OnSet()
    {
        if ((++GSKDATA.MotionCoordinateSystem) > 4)
        {
            GSKDATA.MotionCoordinateSystem = 1;
        }
        setCoordinateIcon();
    }

    public void OnIsContinue()
    {
        GSKDATA.ActionCycle = !GSKDATA.ActionCycle;
        setContinueIcon();
    }
    public void OnOutShaft()
    {
        if (GSKDATA.Scene_NO == 5)
        {
            GSKDATA.MotionCoordinateSystem = 5;
            setCoordinateIcon();
        }
        else
        {
            StartCoroutine(WarningYellow("当前无变位机"));
        }
    }

    public void AxisOnPress(ref bool axis)
    {
        if (GSKDATA.CurrentSafeMode == 2)//示教模式
        {
            if (!GSKDATA.SystemWrong)
            {
                axis = true;
                GSKDATA.AxisRunning = true;
            }
        }
        else
        {
            StartCoroutine(WarningYellow("切换到[示教模式]"));
        }
    }

    public void AxisOnRelease(ref bool axis)
    {
        axis = false;
        GSKDATA.AxisRunning = false;
    }

    public void OnGoforwardPress()
    {
        if (GSKDATA.CurrentSafeMode == 2)//示教模式
        {
            GSKDATA.GoPress = true;
            StartCoroutine(WarningYellow("[前进]键按下"));
        }
        else
        {
            StartCoroutine(WarningYellow("请切换到示教模式"));
        }
    }

    public void OnGoforwardRelease()
    {
        GSKDATA.GoPress = false;
        StartCoroutine(WarningYellow("[前进]键弹起"));
    }

    public void OnGoforwardClick()
    {
        if (GSKDATA.RobotMode == 2)
        {
            RunProgram(0);
            
        }
        else
        {
            StartCoroutine(WarningYellow("请切换到示教模式"));
        }
    }
    //退出再现
    private void ExitReappear()
    {
        GSKDATA.IsReady = false;
        GSKDATA.ReappearRun = false;
        GameObject.Find("Ready").GetComponent<UIButton>().normalSprite = "11";
    }

    //窗口的初始化//案例中用到
    public void WindowInitial()
    {
        F1window.InitialWin();
        xtSheZhi_win.InitialWin();
        cxGuanli_win.InitialWin();
        csSheZhi_win.InitialWin();
        YingYong_win.InitialWin();
        BianLiang_win.InitialWin();
        xtXinXi_win.InitialWin();
        jqSheZhi_win.InitialWin();
        zxBangZu_win.InitialWin();
        setToolF_win.InitialWin();
        toolD_win.InitialWin();
        toolF_win.InitialWin();
        toolF_win.InitialWin();
        setUserF_win.InitialWin();
        userD_win.InitialWin();
        userT_win.InitialWin();
        modeF_win.InitialWin();
        speedF_win.InitialWin();
        newBuild_win.InitialWin();
        programV_win.InitialWin();
        CZ_win.InitialWin();
        AddWin.InitialWin();
        AddWin1.InitialWin();
        AddWin2.InitialWin();
        AddWin3.InitialWin();
        AddWin4.InitialWin();
        AddWin5.InitialWin();
        PointRepeatWin.InitialWin();
        DeleteWin.InitialWin();
        //设置当前窗口
        CurrentWindow = F1window;
        CurrentWindow.isChosen();
        zhuYeMian_InfoSet();
        //按钮的初始化
        GSKDATA.GSKDATA_INITIAL();
    }
    //设置新的窗口并显示
    private void SetCurrentWin(Win win)
    {
        F1window.noChosen();//和二级选项菜单一起消失
        CurrentWindow.noChosen();
        CurrentWindow = win;
        CurrentWindow.isChosen();
    }
    public void SetCurrentWin_Teach(string winname)
    {
        switch (winname)
        {
            case "F1":
                F1window.InitialWin();
                F1part2.InitalPart();
                SetCurrentWin(F1window);
                break;
            case "F2":
                EditInitial();
                F1part1.partNum = 1;
                F1part1.isChosen();
                ProgramWin.fline = 0;
                ProgramWin.line = 0;
                SetCurrentWin(ProgramWin);
                GSKDATA.CurrentScreenPositon = "";
                GrammarTodata();
                break;
            case "F3":
                //Debug.Log(GSKDATA.CurrentProgramName);
                EditInitial();
                int codeline = GSKFile.GetCodeLine(GSKDATA.CurrentProgramName);
                ProgramWin.fline = codeline / 10;
                ProgramWin.line = codeline % 10;
                ProgramWin.ContentsSplit();
                JumpToEdit();
                break;
            case "REAPPEAR":
                F1part1.partNum = 1;
                F1part1.isChosen();
                ProgramWin.fline = 0;
                ProgramWin.line = 0;
                SetCurrentWin(ProgramWin);
                GSKDATA.CurrentScreenPositon = "";
                GrammarTodata();
                OnReappear();
                break;
        }
    }
    //清除所有警告信息
    public void ClearAllWarning()
    {
        ClearRedWarning();
    }

    //警告信息
    public IEnumerator WarningYellow(string content)
    {
        ServerCall.WarningMes(content);
        GameObject.Find("TipsAndWarning").GetComponent<UILabel>().text = content;
        GameObject.Find("TipBlink").GetComponent<UIWidget>().alpha = 1;
        yield return new WaitForSeconds(1f);//1秒后消失
        GameObject.Find("TipBlink").GetComponent<UIWidget>().alpha = 0;
        content = "";
    }
    //红色错误信息
    public void WarningRed(string Content)
    {
        GameObject.Find("Warning").GetComponent<UILabel>().text = Content;
        GameObject.Find("WarningBlink").GetComponent<UIWidget>().alpha = 1;
        GSKDATA.SystemWrong = true;
    }
    //清除红色错误
    public void ClearRedWarning()
    {
        GameObject.Find("Warning").GetComponent<UILabel>().text = "";
        GameObject.Find("WarningBlink").GetComponent<UIWidget>().alpha = 0;
        GSKDATA.SystemWrong = false;
    }
    //程序跳转到指定行
    public void programJumpToLine(int nextline)
    {
        if(nextline == -1)
        {
            return;
        }
        //int nextline = GSKDATA.nextNUM[0];
        ProgramWin.line = nextline % 10;
        ProgramWin.fline = nextline / 10;
        ProgramWin.ContentsSplit();
        ProgramWin.doMutexPart();
        //PAD
        ServerCall.CursorToLine(nextline);
    }

    public void programJumpToLine2(int nextline)
    {
        if (nextline == -1)
        {
            return;
        }
        //int nextline = GSKDATA.nextNUM[0];
        ProgramWin.line = nextline % 10;
        ProgramWin.fline = nextline / 10;
    }
    //输出灯控制
    public void OutLight()
    {
        ///输入信号灯的定义
        for (int i = 0; i < 31; i++)
        {
            if (GSKDATA.OutInfo[i])
            {
                GameObject.Find("OUT" + (i + 1)).GetComponent<UISprite>().spriteName = "light1";
            }
            else
            {
                GameObject.Find("OUT" + (i + 1)).GetComponent<UISprite>().spriteName = "light2";
            }
        }
    }
    //示教面板的位置控制
    public void PanelPosition(bool right)
    {
        Transform panel_trans = GameObject.Find("Panel").GetComponent<Transform>();
        float panel_width = GameObject.Find("Panel").GetComponent<UISprite>().width/2;
        Camera ui_camera= GameObject.Find("CameraUI").GetComponent<Camera>();
        if (right)
        {
            panel_trans.position = ui_camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 7));
        }
        else
        {
            panel_trans.position = ui_camera.ScreenToWorldPoint(new Vector3(panel_width, Screen.height, 7));
        }
    }
    //运行程序
    private void RunProgram(int dir)
    {
        if (CurrentWindow == ProgramWin && F1part1.partNum == 1)
        {
            RobotMotion MotionScript = GameObject.Find("MyMotion").GetComponent<RobotMotion>();
            //double[] pathdata_result = new double[8];
            //int[] ipdata = new int[50];
            
            int line = ProgramWin.fline * 10 + ProgramWin.line;

            tmpInterpreter = (interpreter.getChild()) ? interpreter.childInterpreter : interpreter;     

            if (!GSKDATA.AxisRunning)
            {
                //GSKDATA.RBP_result = RBP_RunParse(ProgramWin.fline * 10 + ProgramWin.line, GSKDATA.nextNUM, dir, pathdata_result, ipdata);//运行译码数据

                programJumpToLine(line);
                GSKDATA.RBP_result = tmpInterpreter.runCode(ref line, 1);
                programJumpToLine2(line);
                List<string> ipdata = tmpInterpreter.GetDataInfo();

                switch (GSKDATA.RBP_result)
                {
                    
                    case "MOVJ":
                        GSKDATA.MOVJ_A1 = (MotionScript.CurrentAngle_All());
                        GSKDATA.MOVJ_A2 = tmpInterpreter.GetPathData();
                        //Debug.Log(ipdata[0]);
                        GSKDATA.Speed = 0.3f * int.Parse(ipdata[1])/100;//

                        GSKDATA.MOVJ_P1 = MotionScript.IKA.SolutionOfKinematics(GSKDATA.MOVJ_A1);
                        GSKDATA.MOVJ_P2 = MotionScript.IKA.SolutionOfKinematics(GSKDATA.MOVJ_A2);

                        GSKDATA.MOVJ_t = 0;
                        //info of axis7
                        outShaftInfo(ipdata);
                        GSKDATA.AxisRunning = true;
                        break;
                    case "MOVL":
                        GSKDATA.MOVL_P1 = MotionScript.IKA.SolutionOfKinematics(MotionScript.CurrentAngle_All());
                        GSKDATA.MOVL_Z1 = MotionScript.IKA.SolutionOfKinematics_posture(MotionScript.CurrentAngle_All());
                        GSKDATA.MOVL_t = 0;
                        if (GSKDATA.SHIFT)
                        {
                            Vector3 px = new Vector3(GSKDATA.SHIFT_PX.X, GSKDATA.SHIFT_PX.Y, GSKDATA.SHIFT_PX.Z)/100f;
                            GSKDATA.MOVL_P2 = MotionScript.IKA.SolutionOfKinematics(tmpInterpreter.GetPathData()) + px;
                            GSKDATA.MOVL_Z2 = MotionScript.IKA.SolutionOfKinematics_posture(tmpInterpreter.GetPathData());
                        }
                        else
                        {
                            //float[] po0 = tmpInterpreter.GetPathData();
                            //Debug.Log("origin:"+po0[0] + "," + po0[1] + "," + po0[2] + "," + po0[3] + "," + po0[4] + "," + po0[5]);
                            GSKDATA.MOVL_P2 = MotionScript.IKA.SolutionOfKinematics(tmpInterpreter.GetPathData());
                            GSKDATA.MOVL_Z2 = MotionScript.IKA.SolutionOfKinematics_posture(tmpInterpreter.GetPathData());
                            //float[] po = MotionScript.IKA.AcceptInterPointPosture(GSKDATA.MOVL_P2, GSKDATA.MOVL_Z2, MotionScript.CurrentAngle_All());
                            //Debug.Log("change:"+po[0] + "," + po[1] + "," + po[2] + "," + po[3] + "," + po[4] + "," + po[5]);
                        }
                        GSKDATA.Speed = 0.3f * int.Parse(ipdata[1]) / 100;
                        GSKDATA.AxisRunning = true;
                        break;
                    case "MOVC":
                        //RBP_GetParse_MOVCPOINT(GSKDATA.MOVC_A1, GSKDATA.MOVC_A2, GSKDATA.MOVC_A3);
                        List<float[]> movcPoints = tmpInterpreter.GetMovcData();

                        if (GSKDATA.SHIFT)
                        {
                            GSKDATA.MOVC_P1 = MotionScript.IKA.SolutionOfKinematics(movcPoints[0]);
                            GSKDATA.MOVC_P2 = MotionScript.IKA.SolutionOfKinematics(movcPoints[1]);
                            GSKDATA.MOVC_P3 = MotionScript.IKA.SolutionOfKinematics(movcPoints[2]);
                            GSKDATA.MOVC_Z1 = MotionScript.IKA.SolutionOfKinematics_posture(movcPoints[0]);
                            GSKDATA.MOVC_Z2 = MotionScript.IKA.SolutionOfKinematics_posture(movcPoints[1]);
                            GSKDATA.MOVC_Z3 = MotionScript.IKA.SolutionOfKinematics_posture(movcPoints[2]);
                        }
                        else
                        {
                            GSKDATA.MOVC_P1 = MotionScript.IKA.SolutionOfKinematics(movcPoints[0]);
                            GSKDATA.MOVC_P2 = MotionScript.IKA.SolutionOfKinematics(movcPoints[1]);
                            GSKDATA.MOVC_P3 = MotionScript.IKA.SolutionOfKinematics(movcPoints[2]);
                            GSKDATA.MOVC_Z1 = MotionScript.IKA.SolutionOfKinematics_posture(movcPoints[0]);
                            GSKDATA.MOVC_Z2 = MotionScript.IKA.SolutionOfKinematics_posture(movcPoints[1]);
                            GSKDATA.MOVC_Z3 = MotionScript.IKA.SolutionOfKinematics_posture(movcPoints[2]);
                        }
                        GSKDATA.Speed = 0.3f * int.Parse(ipdata[1]) / 100;
                        GSKDATA.MOVC_t = 0;
                        GSKDATA.AxisRunning = true;
                        break;
                    case "WAIT"://wait
                        GSKDATA.AxisRunning = true;
                        GSKDATA.WaitInput = int.Parse(ipdata[0]);
                        GSKDATA.WaitInputState = 0;
                        if(ipdata[0] == "ON")
                        {
                            GSKDATA.WaitInputState = 1;
                        }
                        if (int.Parse(ipdata[2]) != 0)
                        {
                            StartCoroutine(Waitfors(int.Parse(ipdata[2])));
                        }
                        break;
                    case "DELAY"://delay
                        GSKDATA.AxisRunning = true;
                        StartCoroutine(Waitfors(float.Parse(ipdata[0])));
                        break;
                    case "PULSE"://PULSE指令
                        //Debug.Log(ipdata[0]);
                        GSKDATA.OutInfo[int.Parse(ipdata[0])] = true;
                        OutLight();
                        StartCoroutine(Waitfor(int.Parse(ipdata[1]), int.Parse(ipdata[0])));
                        break;
                    case "SHIFTON"://SHIFTON指令//只有MOVL\MOVC有效
                        //GSKDATA.SHIFT = true;
                        //GSKDATA.SHIFT_int++;
                        //for (int i = 0; i < 6; i++)
                        //{
                        //    GSKDATA.SHIFT_data[i] = GSKDATA.PX_data[ipdata[0], i];
                        //}
                        //GSKDATA.SHIFT_data = new float[6] { 5, 10, 5, 0, 0, 0 };//测试用
                        //Debug.Log("SHIFTON");
                        //Debug.Log(ipdata[0]);
                        break;
                    case "SHIFTOFF"://SHIFTOFF
                        //Debug.Log("SHIFTOFF");
                        //GSKDATA.SHIFT = false;
                        break;
                    case "MSHIFT"://MSHIFT指令
                        //Debug.Log("MSHIFT");
                        //Debug.Log(ipdata[0]);Debug.Log(ipdata[1]);Debug.Log(ipdata[2]);
                        break;
                    case "ARCON"://ARCON
                        Debug.Log("ARCON");
                        if (!GSKDATA.Weld)
                        {
                            GSKDATA.Weld = true;
                            wlscript.SePosition();
                        }
                        break;
                    case "ARCOF"://ARCOF
                        Debug.Log("ARCOF");
                        GSKDATA.Weld = false;
                        GSKDATA.Speed = 0.3f;
                        break;
                    case "END"://程序运行结束
                        if (GSKDATA.SoftCurrentMode == "Teach" && (GSKDATA.Scene_NO != 5))
                        {
                            GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.CaseTeachOver("您已经完成了该案例的学习！");
                        }
                        if (tmpInterpreter.parentLine != -1)
                        {
                            line = tmpInterpreter.parentLine;
                            GSKDATA.CurrentProgramName = tmpInterpreter.parentName;
                            interpreter.destoryChild();//
                            EditInitial();
                            SetCurrentWin(ProgramWin);
                            programJumpToLine(line);
                        }
                        break;
                    case "LAB":
                        break;
                    case "MAIN":
                        break;
                    case "CALL"://子程序
                        childFileName = ipdata[0] + ".prl";
                        tmpInterpreter.initChild(childFileName, line, GSKDATA.CurrentProgramName);
                        //切换显示内容
                        GSKDATA.CurrentProgramName = childFileName;
                        EditInitial();
                        SetCurrentWin(ProgramWin);
                        line = 0;
                        break;
                    default:
                        //Debug.Log("没有找到相关指令：" + GSKDATA.RBP_result);
                        break;
                }
            }
            else
            {
                StartCoroutine(WarningYellow("行：" + (ProgramWin.fline + ProgramWin.line) + "运行未完成"));
            }
        } 
        
    }
    //译码
    public void GrammarTodata()
    {
        //char[] fileName = new char[100];
        //fileName = GSKDATA.CurrentProgramName.ToCharArray();
        //RBP_Set_RUNFileName(fileName);//设置文件名
        ////Debug.Log("当前译码的文件名" + fileName);
        //RBP_CleanParse();//清空译码内存
        //LAnalysis.PointAnalysis();//路径点设置
        //LAnalysis.KeyAnalysis();//关键词分析
        //for (int i = 0; i < 32; i++)
        //{
        //    int ss = RBP_Set_IN_Data(i, GSKDATA.InInfo[i]);
        //    int rr = RBP_Set_OT_Data(i, GSKDATA.OutInfo[i]);
        //}
        //for (int i = 0; i < 100; i++)//PX \R 
        //{
        //    double[] tempdata = new double[6];
        //    for (int j = 0; j < 6; j++)
        //    {
        //        tempdata[j] = GSKDATA.PX_data[i, j];
        //    }
        //    RBP_Set_PX_Data(i, tempdata);
        //    RBP_Set_I_Data(i, GSKDATA.R_data[i]);
        //}
        //RBP_Set_PX_Data(0, new double[6] { 2, 2, 2, 0, 0, 0 });
        //RBP_Set_PX_Data(1, new double[6] { 2, 2, 2, 0, 0, 0 });

        interpreter.run(GSKDATA.CurrentProgramName);

        ///初始化运动数据
        GSKDATA.MOVJ_t = 0;
        GSKDATA.MOVL_t = 0;
        GSKDATA.MOVC_t = 0;
    }
    public void ClearOutAndIn()
    {
        for (int i = 0; i < 32; i++)
        {
            //int ss = RBP_Set_IN_Data(i, false);
            //int rr = RBP_Set_OT_Data(i, false);

            interpreter.setInState(i, 0);
            interpreter.setOutState(i, 0);

        }
    }
    //等待输入接口，t时间后不再等待
    private IEnumerator Waitfors(float t)
    {
        yield return new WaitForSeconds(t);
        GSKDATA.AxisRunning = false;
        GSKDATA.WaitInput = -1;
        GSKDATA.WaitInputState = -1;
        //programJumpToLine();
    }
    //输出接口，t时间后输出接口置为零
    private IEnumerator Waitfor(int t, int num)
    {
        yield return new WaitForSeconds(t);
        GSKDATA.OutInfo[num] = false;
        OutLight();
    }
    
    //远程、示教、再现模图标变化
    private void setModeIcon()
    {
        switch (GSKDATA.RobotMode)
        {
            case 1:
                GameObject.Find("KAIGUAN").GetComponent<UISprite>().spriteName = "U4-1";
                GameObject.Find("Modeshow").GetComponent<UILabel>().text = "再现模式";
                break;
            case 2:
                GameObject.Find("KAIGUAN").GetComponent<UISprite>().spriteName = "U4";
                GameObject.Find("Modeshow").GetComponent<UILabel>().text = "示教模式";
                break;
            case 3:
                GameObject.Find("KAIGUAN").GetComponent<UISprite>().spriteName = "U4-2";
                GameObject.Find("Modeshow").GetComponent<UILabel>().text = "远程模式";
                break;
        }
    }
    //速度图标的变化
    private void setSpeedIcon()
    {
        if (GSKDATA.ManualSpeedR.Level == 0)
        {
            GameObject.Find("Speed").GetComponent<UISprite>().spriteName = "I";
        }
        else if (GSKDATA.ManualSpeedR.Level == 1)
        {
            GameObject.Find("Speed").GetComponent<UISprite>().spriteName = "L";
        }
        else if (GSKDATA.ManualSpeedR.Level == 2)
        {
            GameObject.Find("Speed").GetComponent<UISprite>().spriteName = "M";
        }
        else if (GSKDATA.ManualSpeedR.Level == 3)
        {
            GameObject.Find("Speed").GetComponent<UISprite>().spriteName = "H";
        }
        else
        {
            GameObject.Find("Speed").GetComponent<UISprite>().spriteName = "S";
        }
    }
    //坐标图标的变化
    private void setCoordinateIcon()
    {


        if (GSKDATA.MotionCoordinateSystem == 1)
        {
            GameObject.Find("MotionCoordinate").GetComponent<UISprite>().spriteName = "J";
        }
        else if (GSKDATA.MotionCoordinateSystem == 2)
        {
            GameObject.Find("MotionCoordinate").GetComponent<UISprite>().spriteName = "B";
        }
        else if (GSKDATA.MotionCoordinateSystem == 3)
        {
            GameObject.Find("MotionCoordinate").GetComponent<UISprite>().spriteName = "T";
        }
        else if (GSKDATA.MotionCoordinateSystem == 4)
        {
            GameObject.Find("MotionCoordinate").GetComponent<UISprite>().spriteName = "U";
        }
        else 
        {
            GameObject.Find("MotionCoordinate").GetComponent<UISprite>().spriteName = "eeee";
        }
    }
    //单段连续图标的变化
    private void setContinueIcon()
    {
        if (GSKDATA.ActionCycle)
        {
            GameObject.Find("OneTwo").GetComponent<UISprite>().spriteName = "two";
        }
        else
            GameObject.Find("OneTwo").GetComponent<UISprite>().spriteName = "one";
    }

    private void ChooseResult_1(int result)
    {
        switch (result)
        {
            case ZHUYEMIAN_NO:
                SetCurrentWin(F1window);
                zhuYeMian_InfoSet();
                GSKDATA.CurrentScreenPositon = "主页面>";
                break;
            case XITONGSHEZHI_NO:
                SetCurrentWin(xtSheZhi_win);
                F1window.isChosen();
                GSKDATA.CurrentScreenPositon = "主页面>系统设置>";
                break;
            case CHENGXUGUANLI_NO:
                SetCurrentWin(cxGuanli_win);
                F1window.isChosen();
                GSKDATA.CurrentScreenPositon = "主页面>程序管理>";
                break;
            case CANSHUSHEZHI_NO:
                SetCurrentWin(csSheZhi_win);
                F1window.isChosen();
                GSKDATA.CurrentScreenPositon = "主页面>参数设置>";
                break;
            case YINGYONG_NO:
                SetCurrentWin(YingYong_win);
                F1window.isChosen();
                GSKDATA.CurrentScreenPositon = "主页面>应用>";
                break;
            case BIANLIANG_NO:
                SetCurrentWin(BianLiang_win);
                F1window.isChosen();
                GSKDATA.CurrentScreenPositon = "主页面>变量>";
                break;
            case XITONGXINXI_NO:
                SetCurrentWin(xtXinXi_win);
                F1window.isChosen();
                GSKDATA.CurrentScreenPositon = "主页面>系统信息>";
                break;
            case JIQISHEZHI_NO:
                SetCurrentWin(jqSheZhi_win);
                F1window.isChosen();
                GSKDATA.CurrentScreenPositon = "主页面>机器设置>";
                break;
            case ZAIXIANBANGZHU_NO:
                SetCurrentWin(zxBangZu_win);
                F1window.isChosen();
                GSKDATA.CurrentScreenPositon = "主页面>在线帮助>";
                break;
        }
    }

    //程序界面按钮事件
    private void ChooseResult_2(int result)
    {
        switch (result)
        {
            case CHENGXU_NO://f2键
                SetCurrentWin(ProgramWin);
                GSKDATA.CurrentScreenPositon = "";
                GrammarTodata();
                break;
            case SETCURRENTCHENGXU://从程序列表刀程序界面
                int hanghao = F1part3.firstline + F1part3.partNum;
                GSKDATA.CurrentProgramName = GSKFile.GetFileName(hanghao);
                ProgramWin = new ProgramWinClass("Panel_F3", 1, FILEPATH + GSKDATA.CurrentProgramName);
                ProgramWin.fline = 0;
                ProgramWin.line = 0;
                F1part1.partNum = 1;
                F1part1.isChosen(); 
                SetCurrentWin(ProgramWin);
                GSKDATA.CurrentScreenPositon = "";                
                GrammarTodata();

                ServerCall.EnterProgramFromList_RPC();
                //PAD
                break;
            case DELETE_YES:
                ProgramWin.DeleteDone();
                SetCurrentWin(ProgramWin);
                break;
            case DELETE_NOO:
                SetCurrentWin(ProgramWin);
                break;
            case START_YES:
                if (GSKDATA.IsReady)
                {
                    GSKDATA.ReappearRun = true;
                    detectresult.DetectReappear();
                }
                else
                {
                    StartCoroutine(WarningYellow("请先上开使能"));
                }
                SetCurrentWin(ProgramWin);
                break;
            case START_NO:
                SetCurrentWin(ProgramWin);
                break;
        }
    }

    private void ChooseResult_3(int result)
    {
        //Debug.Log("方向键：" + result);
        switch (result)
        {
            case BIANJI_NO:
                F1part1.partNum = 2;
                F1part1.isChosen();
                SetCurrentWin(ProgramWin);
                ProgramWin.cut = 0;
                ProgramWin.copy = 0;
                GSKDATA.CurrentScreenPositon = "";
                break;
            case POINTREPEAT_NO://不覆盖之前的路径点
                NotCoverPoint();
                SetCurrentWin(ProgramWin);
                break;
            case POINTREPEAT_YES://覆盖之前的路径点
                CoverPoint();
                SetCurrentWin(ProgramWin);
                break;
            case BIANJI_MODIFY:
                SetCurrentWin(ProgramWin);
                OnModify();
                break;
            case ADD:
                AddPart.partNum = 0;
                SetCurrentWin(AddWin);
                ProgramWin.isChosen();
                break;
            case ADD_1:
                AddPart1.partNum = 0;
                SetCurrentWin(AddWin1);
                AddWin.isChosen();
                break;
            case ADD_2:
                AddPart2.partNum = 0;
                SetCurrentWin(AddWin2);
                AddWin.isChosen();
                break;
            case ADD_3:
                AddPart3.partNum = 0;
                SetCurrentWin(AddWin3);
                AddWin.isChosen();
                break;
            case ADD_4:
                AddPart4.partNum = 0;
                SetCurrentWin(AddWin4);
                AddWin.isChosen();
                break;
            case ADD_5:
                AddPart5.partNum = 0;
                SetCurrentWin(AddWin5);
                AddWin.isChosen();
                break;
            case ADD_6:
                AddPart6.partNum = 0;
                SetCurrentWin(AddWin6);
                AddWin.isChosen();
                break;
            case ADD_MOVJ:
                addProgram("MOVJ");
                break;
            case ADD_MOVL:
                addProgram("MOVL");
                break;
            case ADD_MOVC:
                addProgram("MOVC");
                break;
            case ADD_DOUT:
                addProgram("DOUT");
                break;
            case ADD_DIN:
                addProgram("DIN");
                break;
            case ADD_DELAY:
                addProgram("DELAY");
                break;
            case ADD_WAIT:
                addProgram("WAIT");
                break;
            case ADD_PULSE:
                addProgram("PULSE");
                break;
            case ADD_LAB:
                addProgram("LAB");
                break;
            case ADD_JUMP:
                addProgram("JUMP");
                break;
            case ADD_JUMP_R:
                addProgram("JUMP_R");
                break;
            case ADD_JUMP_IN:
                addProgram("JUMP_IN");
                break;
            case ADD_JH:
                addProgram("JH");
                break;
            case ADD_R:
                addProgram("R");
                break;
            case ADD_INC:
                addProgram("INC");
                break;
            case ADD_DEC:
                addProgram("DEC");
                break;
            case ADD_PX:
                addProgram("PX");
                break;
            case ADD_SHIFTON:
                addProgram("SHIFTON");
                break;
            case ADD_SHIFTOFF:
                addProgram("SHIFTOFF");
                break;
            case ADD_MSHIFT:
                addProgram("MSHIFT");
                break;
            case ADD_ARCON:
                addProgram("ARCON");
                break;
            case ADD_ARCOF:
                addProgram("ARCOF");
                break;
            case ADD_CALL:
                addProgram("CALL");
                break;
        }
    }

    private void ChooseResult_4(int result)
    {
        switch (result)
        {
            case XIANSHI_NO:
                SetCurrentWin(F4Window);
                GSKDATA.CurrentScreenPositon = "";
                break;
        }
    }

    private void ChooseResult_5(int result)
    {
        switch (result)
        {
            case GONGJU_NO:
                SetCurrentWin(F5Window);
                GSKDATA.CurrentScreenPositon = "";
                break;
        }
    }

    private void ChooseResult_11(int result)
    {
        switch (result)
        {
            case JUEDUILINGDIAN_FUNCTION_NO:
                break;
            case GONGJUZUOBIAO_FUNCTION_NO:
                SetCurrentWin(setToolF_win);
                toolFunction_InfoSet();
                GSKDATA.CurrentScreenPositon = "主页面>系统设置>工具坐标>";
                break;
            case YONGHUZUOBIAO_FUNCTION_NO:
                SetCurrentWin(setUserF_win);
                userFunction_InfoSet();
                GSKDATA.CurrentScreenPositon = "主页面>系统设置>用户坐标>";
                break;
            case MOSHIQIEHUAN_FUNCTION_NO:
                SetCurrentWin(modeF_win);

                GSKDATA.CurrentScreenPositon = "主页面>系统设置>模式选择>";
                break;
            case XITONGSUDU_FUNCTION_NO:
                SetCurrentWin(speedF_win);
                systemSpeed_InfoSet();
                GSKDATA.CurrentScreenPositon = "主页面>系统设置>系统速度>";
                break;
            case ZHUCHENGXU_FUNCTION_NO:
              
                //GSKDATA.CurrentScreenPositon = "主页面>系统设置>主程序设置>";
                break;
            case GONGJUZUOBIAO_SET://tool功能
                entryToolSetScreen();//entry set tool screen based on different options
                break;
            case TOOL_ZHIJIESHURU:
                setTool_Direct();//直接输入法设置工具坐标
                break;
            case TOOL_SHANDIANFA:
                setTool_Three();
                break;
            case TOOL_WUDIANFA:
                setTool_Five();
                break;
            case YONGHUZUOBIAO_SET://USER功能
                entryUserSetScreen();
                break;
            case USER_ZHIJIESHURU:
                setUser_Direct();//直接输入法设置用户坐标
                break;
            case USER_SHANDIANFA:
                setUser_Three();
                break;
            case MOSHIQIEHUAN_FUNCTION:
                setCurrentSafeMode();//设置当前的安全模式
                break;
            case XITONGSUDU_KAIJI:
                setKaijiSpeed();//设置系统开机默认速度
                break;
            case XITONGSUDU_SHEZHI:
                setSpeed();//设置速度
                break;
            case XITONGSUDU_MOREN:
                setSpeedToDefault();//设置速度为默认值
                break;
        }
    }

    private void ChooseResult_12(int result)
    {
        switch (result)
        {
            case XINJIANCHENGXU_FUNCTION_NO:
                SetCurrentWin(newBuild_win);
                newBuild_InfoSet();
                GSKDATA.CurrentScreenPositon = "主页面>程序管理>新建程序>";
                break;
            case XINJIANCHENGXU:
                createNewProgram();
                break;
            case CHENGXUYILAN_FUNCTION_NO:
                SetCurrentWin(programV_win);
                programV_InfoSet();
                GSKDATA.CurrentScreenPositon = "主页面>程序管理>程序一览>";
                break;
            case COPY_PROGRAM:
                copyProgram();
                break;
            case DELETE_PROGRAM:
                deleteProgram();
                break;
            case SEARCH_PROGRAM:
                searchProgram();
                break;
            case RENAME_PROGRAM:
                renameProgram();
                break;
        }
    }
    //help
    private void ChooseResult_10(int result)
    {
        switch (result)
        {
            case ZHILING_HELP_NO:
                SetCurrentWin(ZL_win);
                zhiLingHelp_InfoSet();
                GSKDATA.CurrentScreenPositon = "主页面>再现帮助>指令>";
                break;
            case CAOZUO_HELP_NO:
                SetCurrentWin(CZ_win);
                GSKDATA.CurrentScreenPositon = "主页面>再现帮助>操作>";
                break;
        }
    }
    //输入软件盘字符
    private void ChooseResult_20(int result)
    {
        int d_value = 0;
        switch (GSKDATA.RuanJianPanNo)
        {
            case 0:
                d_value = 32;
                break;
            case 1:
                d_value = 0;
                break;
        }
        result -= 2000;
        char letter = (char)(result+d_value);
        string number_result = ruanJianPanName.onNumBt(letter.ToString());
        if (number_result != "ok")
            StartCoroutine(WarningYellow(number_result));
    }
    //调出软键盘
    private void ChooseResult_214(int result)
    {
        letterWin.lastWin = result;
        CurrentWindow = letterWin;
        letterWin.isChosen();
        ruanJianPanName.label.text = string.Empty;
        ruanJianPanName.tempstr = string.Empty;
        XiaoXie.isChosen();
        GSKDATA.RuanJianPanNo = 0;
        keyboard_InfoSet();
    }

    //判定进入哪种工具坐标系设定界面
    private void entryToolSetScreen()
    {
        if (toolDirect.mywid.alpha == 1)
        {
            SetCurrentWin(toolD_win);
            directTool_InfoSet();
        }
        else if (toolThree.mywid.alpha == 1)
        {
            SetCurrentWin(toolT_win);
            threeTool_InfoSet();
        }
        else
        {
            SetCurrentWin(toolF_win);
            fiveTool_InfoSet();
        }
    }
    //工具坐标系号加减
    private void currentToolNumPlus(bool plus)
    {
        if (plus)
        {
            if (++GSKDATA.CurrentTool == 10)
            {
                GSKDATA.CurrentTool = 9;
            }
        }
        else
        {
            if (--GSKDATA.CurrentTool == -1)
            {
                GSKDATA.CurrentTool = 0;
            }
        }
        GameObject.Find("F1_1_2_N_text").GetComponent<UILabel>().text = GSKDATA.CurrentTool.ToString();
    }
    //直接法设置工具坐标系
    private void setTool_Direct()
    {
        Debug.Log("set tool direct");
    }
    //三点法设置工具坐标系
    private void setTool_Three()
    {
        Debug.Log("set tool three");
    }
    //五点法设置工具坐标系
    private void setTool_Five()
    {
        Debug.Log("set tool five");
    }

    //判定进入哪种用户坐标系设定界面
    private void entryUserSetScreen()
    {
        if (userDirect.mywid.alpha == 1)
        {
            SetCurrentWin(userD_win);
            directUser_InfoSet();
        }
        else
        {
            SetCurrentWin(userT_win);
            threeUser_InfoSet();
        }
    }
    //用户坐标系号加减
    private void currentUserNumPlus(bool plus)
    {
        if (plus)
        {
            if (++GSKDATA.CurrentUser == 10)
            {
                GSKDATA.CurrentUser = 9;
            }
        }
        else
        {
            if (--GSKDATA.CurrentUser == -1)
            {
                GSKDATA.CurrentUser = 0;
            }
        }
        GameObject.Find("F1_1_3_N_text").GetComponent<UILabel>().text = GSKDATA.CurrentTool.ToString();
    }
    //直接法设置用户坐标系
    private void setUser_Direct()
    {
        Debug.Log("set user direct");
    }
    //三点法设置用户坐标系
    private void setUser_Three()
    {
        Debug.Log("set user three");
    }

    //方向键产生的错误警告
    private void DirectionKeyWarning(int num)
    {
        switch (num)
        {
            case -1:
                StartCoroutine(WarningYellow("[输入]键确认输入"));
                break;
            case -3:
                StartCoroutine(WarningYellow("已到达首行"));
                break;
            case -4:
                StartCoroutine(WarningYellow("已到达尾行"));
                break;
        }
    }

    //设置当前的安全模式
    private void setCurrentSafeMode()
    {
        if (operationMode.mywid.alpha == 1)
        {
            GSKDATA.CurrentSafeMode = 1;
            GameObject.Find("SafeMode").GetComponent<UISprite>().spriteName = "Operation";
        }
        else if (editMode.mywid.alpha == 1)
        {
            GSKDATA.CurrentSafeMode = 2;
            GameObject.Find("SafeMode").GetComponent<UISprite>().spriteName = "Editing";
        }
        else
        {
            GSKDATA.CurrentSafeMode = 3;
            GameObject.Find("SafeMode").GetComponent<UISprite>().spriteName = "Management";
        }
    }
    //设置系统速度
    private void setSpeed()
    {
        Debug.Log(speed_I.label.text);
        GSKDATA.ManualSpeedR.I = Convert.ToInt16(speed_I.label.text) / 100f;
        GSKDATA.ManualSpeedR.L = Convert.ToInt16(speed_L.label.text) / 100f;
        GSKDATA.ManualSpeedR.M = Convert.ToInt16(speed_M.label.text) / 100f;
        GSKDATA.ManualSpeedR.H = Convert.ToInt16(speed_H.label.text) / 100f;
        GSKDATA.ManualSpeedR.S = Convert.ToInt16(speed_S.label.text) / 100f;
    }
    //速度默认值
    private void setSpeedToDefault()
    {
        speed_I.label.text = "5";
        speed_L.label.text = "20";
        speed_M.label.text = "45";
        speed_H.label.text = "70";
        speed_S.label.text = "95";
        GSKDATA.ManualSpeedR = new ManualSpeedRank(0.05f, 0.20f, 0.45f, 0.70f, 0.95f);
    }
    //开机速度
    private void setKaijiSpeed()
    {
        UILabel kaiji_speed = GameObject.Find("F117_MM").GetComponent<UILabel>();
        switch (kaiji_speed.text)
        {
            case "微动(I)":
                kaiji_speed.text = "低速(L)";
                break;
            case "低速(L)":
                kaiji_speed.text = "中速(M)";
                break;
            case "中速(M)":
                kaiji_speed.text = "高速(H)";
                break;
            case "高速(H)":
                kaiji_speed.text = "超高速(S)";
                break;
            case "超高速(S)":
                kaiji_speed.text = "微动(I)";
                break;
        }
        GSKDATA.KaiJiSpeed = kaiji_speed.text;
    }

    //新建程序并跳转到程序编辑界面
    private void createNewProgram()
    {
        //新建程序
        string temp_name = newBuildName.label.text + SUFFIX;
        if(temp_name == "")
        {
            StartCoroutine(WarningYellow("文件名不能为空"));
        }
        else if (GSKFile.CreateFile(temp_name))
        {
            GSKDATA.CurrentProgramName = temp_name;
            //跳转到程序编辑界面
            EditInitial();
            JumpToEdit();
            F1part3.partNum = 0;
            F1part3.doMutexBt();
            updateProgramInfoList(StrNo1, StrName1, StrSize1, StrTime1, 6, F1part3.firstline);
            newBuildName.label.text = "";
            detectresult.DetectNewBuild(temp_name);
            ServerCall.CreateProgram(temp_name);
            //PAD
        }
        else
        {
            StartCoroutine(WarningYellow(temp_name + "已存在"));
            //PAD
        }

    }
    //编辑界面初始化
    public void EditInitial()
    {
        ProgramWin = new ProgramWinClass("Panel_F3", 1, FILEPATH + GSKDATA.CurrentProgramName);
        ProgramWin.fline = 0;
        ProgramWin.line = 0;
    }

    //跳转到程序编辑界面
    private void JumpToEdit()
    {
        F1part1.partNum = 2;
        F1part1.isChosen();
        SetCurrentWin(ProgramWin);
        ProgramWin.cut = 0;
        ProgramWin.copy = 0;
        GSKDATA.CurrentScreenPositon = "";
    }

    //复制程序
    private void copyProgram()
    {
        if (ServerCenter.IsConnected)
        {
            return;
        }
        int hanghao = programVPart_1.partNum + programVPart_1.firstline;
        string originalName = GSKFile.GetFileName(hanghao);
        string copyName = newName.label.text + SUFFIX;
        StartCoroutine(GSKFile.CopyProgram(originalName, copyName));
        newName.label.text = "";
        updateProgramInfoList(StrNo4, StrName4, StrSize4, StrTime4, 7, programVPart_1.firstline);
    }

    //删除程序
    private void deleteProgram()
    {
        if (ServerCenter.IsConnected)
        {
            return;
        }
        int hanghao = programVPart_1.partNum + programVPart_1.firstline;
        string originalName = GSKFile.GetFileName(hanghao);
        StartCoroutine(GSKFile.DeleteProgram(originalName));
        newName.label.text = "";
        updateProgramInfoList(StrNo4, StrName4, StrSize4, StrTime4, 7, programVPart_1.firstline);
    }

    //添加程序
    private void addProgram(string keyword)
    {
        if (ProgramWin.fline * 10 + ProgramWin.line == ProgramWin.fileContents.Count - 1)
        {
            StartCoroutine(WarningYellow("当前位置不能添加程序"));
        }
        else
        {
            ProgramWin.AddContent(keyword);
            ProgramWin.ContentsSplit();
        }
        SetCurrentWin(ProgramWin);
        AddWin.noChosen();
        OnModify();
    }

    //搜索程序
    private void searchProgram()
    {
        if (ServerCenter.IsConnected)
        {
            return;
        }
        int hangshu = 7;
        string xinName = newName.label.text + SUFFIX;
        StartCoroutine(GSKFile.SearchProgram(ref programVPart_1.partNum, ref programVPart_1.firstline, hangshu, ref xinName));
        newName.label.text = "";
        programV_win.winNum = 0;
        programV_win.isChosen();
        updateProgramInfoList(StrNo4, StrName4, StrSize4, StrTime4, 7, programVPart_1.firstline);
    }

    //重命名程序
    private void renameProgram()
    {
        if (ServerCenter.IsConnected)
        {
            return;
        }
        int hanghao = programVPart_1.partNum + programVPart_1.firstline;
        string originalName = GSKFile.GetFileName(hanghao);
        string copyName = newName.label.text + SUFFIX;
        StartCoroutine(GSKFile.RenameProgram(originalName,copyName));
        newName.label.text = "";
        updateProgramInfoList(StrNo4, StrName4, StrSize4, StrTime4, 7, programVPart_1.firstline);
    }

    //指令帮助的显示
    private void zhiLingHelp()
    {
        string zhiLing = string.Empty;
        string zhiLing_illumination = string.Empty;
        switch (ZL_Part.partNum+1)
        {
            case 1:
                zhiLing = "MOVJ";
                zhiLing_illumination = "类型：运动指令\n功能：以点到点（PTP）方式移动到指定位姿.\n格式：MOVJ  P,  V,  Z ;";
                break;
            case 2:
                zhiLing = "MOVL";
                zhiLing_illumination = "类型：运动指令\n功能：以直线插补方式移动到指定位姿.\n格式：MOVL  P,  V,  Z ;";
                break;
            case 3:
                zhiLing = "MOVC";
                zhiLing_illumination = "类型：运动指令\n功能：以圆弧插补方式移动到指定位姿.\n格式：MOVC  P,  V,  Z ;";
                break;
            case 4:
                zhiLing = "DOUT";
                zhiLing_illumination = "类型：信号处理指令\n功能：数字信号输出I/O置位指令.\n格式：DOUT  OT,  ON/OStaticVariables.MOVJ_A1 ;";
                break;
            case 5:
                zhiLing = "DIN";
                zhiLing_illumination = "类型：信号处理指令\n功能：把输入信号读入到变量中.\n格式：DIN  R,  IN ;";
                break;
            case 6:
                zhiLing = "DELAY";
                zhiLing_illumination = "类型：信号处理指令\n功能：使机器人延时运行指定时间.\n格式：DELAY  T ;";
                break;
            case 7:
                zhiLing = "WAIT";
                zhiLing_illumination = "类型：信号处理指令\n功能：等待直到外部输入信号的状态符合指定的值.\n格式：WAIT  IN,  ON/OStaticVariables.MOVJ_A1,  T ;";
                break;
            case 8:
                zhiLing = "LAB";
                zhiLing_illumination = "类型：流程控制指令\n功能：标明要跳转到的语句.\n格式：LAB ;";
                break;
            case 9:
                zhiLing = "JUMP";
                zhiLing_illumination = "类型：流程控制指令\n功能：跳转到指定标签.\n格式：JUMP  LAB ;";
                break;
            case 10:
                zhiLing = "END";
                zhiLing_illumination = "类型：流程控制指令\n功能：程序结束.\n格式：END ;";
                break;
        }
        GameObject.Find("ZHILING").GetComponent<UILabel>().text = zhiLing;
        GameObject.Find("Ordercontents").GetComponent<UILabel>().text = zhiLing_illumination;
    }

    //更新程序信息列表
    private void updateProgramInfoList(string[] strNo, string[] strName, string[] strSize, string[] strTime, int biaohangshu, int firstline)
    {
        List<FileInfo> listfileinfo = GSKFile.GetFileDirectory();
        GSKDATA.ChengXuCount = listfileinfo.Count;//更新程序列表
        if (GSKDATA.ChengXuCount >= firstline + biaohangshu)
        {
            for (int i = firstline; i < firstline + biaohangshu; i++)
            {
                GameObject.Find(strName[i - firstline]).GetComponent<UILabel>().text = Fname(listfileinfo[i].Name);
                GameObject.Find(strNo[i - firstline]).GetComponent<UILabel>().text = (1 + i).ToString("000");
                GameObject.Find(strSize[i - firstline]).GetComponent<UILabel>().text = listfileinfo[i].Length.ToString() + "b";
                GameObject.Find(strTime[i - firstline]).GetComponent<UILabel>().text = listfileinfo[i].CreationTime.ToString("yyyy-MM-dd");
            }
        }
        else
        {
            for (int i = 0; i < listfileinfo.Count; i++)
            {
                GameObject.Find(strName[i]).GetComponent<UILabel>().text = Fname(listfileinfo[i].Name);
                GameObject.Find(strNo[i]).GetComponent<UILabel>().text = (1 + i).ToString("000");
                GameObject.Find(strSize[i]).GetComponent<UILabel>().text = listfileinfo[i].Length.ToString() + "b";
                GameObject.Find(strTime[i]).GetComponent<UILabel>().text = listfileinfo[i].CreationTime.ToString("yyyy-MM-dd");
            }
            for (int j = listfileinfo.Count; j < biaohangshu; j++)
            {
                GameObject.Find(strName[j]).GetComponent<UILabel>().text = "";
                GameObject.Find(strNo[j]).GetComponent<UILabel>().text = "";
                GameObject.Find(strSize[j]).GetComponent<UILabel>().text = "";
                GameObject.Find(strTime[j]).GetComponent<UILabel>().text = "";
            }
        }
    }

    //通过下上键更新列表
    private void updateList_UP_DOWN()
    {
        if (CurrentWindow == F1window)
        {
            updateProgramInfoList(StrNo1, StrName1, StrSize1, StrTime1, 6, F1part3.firstline);
        }
        else if (CurrentWindow == newBuild_win)
        {
            updateProgramInfoList(StrNo3, StrName3, StrSize3, StrTime3, 5, newBuildPart_1.firstline);
        }
        else if (CurrentWindow == programV_win)
        {
            updateProgramInfoList(StrNo4, StrName4, StrSize4, StrTime4, 7, programVPart_1.firstline);
        }
    }

    //去除后缀名
    private string Fname(string NN)
    {
        int index = NN.IndexOf(".");
        //type = name.Substring(index + 1);
        string nn = NN.Remove(index);
        return nn;
    }


    #region -----------进入界面之前，界面Label信息的初始化---------

    //进入主页面的初始化
    private void zhuYeMian_InfoSet()
    {
        updateProgramInfoList(StrNo1, StrName1, StrSize1, StrTime1, 6, F1part3.firstline);
        GSKDATA.ChengXuCount = GSKFile.GetFileDirectory().Count;
        F1part3.SetlistCount = GSKDATA.ChengXuCount;

    }

    //工具坐标系设置方法选择界面
    private void toolFunction_InfoSet()
    {
        GameObject.Find("F1_1_2_N_text").GetComponent<UILabel>().text = GSKDATA.CurrentTool.ToString();
    }

    //直接输入法设置工具坐标界面的初始化
    private void directTool_InfoSet()
    {
        GameObject.Find("ZBNumber").GetComponent<UILabel>().text = GSKDATA.CurrentTool.ToString();
        //读入相应的坐标值
    }

    //三点输入法设置工具坐标界面的初始化
    private void threeTool_InfoSet()
    {
        GameObject.Find("F1_1_2_2_ZBNumber").GetComponent<UILabel>().text = GSKDATA.CurrentTool.ToString();
        //读入相应的坐标值
    }

    //五点输入法设置工具坐标界面的初始化
    private void fiveTool_InfoSet()
    {
        GameObject.Find("F1_1_2_3_ZBNumber").GetComponent<UILabel>().text = GSKDATA.CurrentTool.ToString();
        //读入相应的坐标值
    }

    //用户坐标系设置方法选择界面的初始化
    private void userFunction_InfoSet()
    {
        GameObject.Find("F1_1_3_N_text").GetComponent<UILabel>().text = GSKDATA.CurrentUser.ToString();
    }

    //直接输入法设置用户坐标界面的初始化
    private void directUser_InfoSet()
    {
        GameObject.Find("UserZBNumber").GetComponent<UILabel>().text = GSKDATA.CurrentUser.ToString();
        //读入相应的坐标值
    }

    //三点输入法设置用户坐标界面的初始化
    private void threeUser_InfoSet()
    {
        GameObject.Find("F1_1_3_2_ZBNumber").GetComponent<UILabel>().text = GSKDATA.CurrentUser.ToString();
        //读入相应的坐标值
    }

    //系统速度界面信息初始化
    private void systemSpeed_InfoSet()
    {
        Debug.Log(GSKDATA.ManualSpeedR.I);
        GameObject.Find("F117_I").GetComponent<UILabel>().text = (GSKDATA.ManualSpeedR.I * 100).ToString();
        GameObject.Find("F117_L").GetComponent<UILabel>().text = (GSKDATA.ManualSpeedR.L * 100).ToString();
        GameObject.Find("F117_M").GetComponent<UILabel>().text = (GSKDATA.ManualSpeedR.M * 100).ToString();
        GameObject.Find("F117_H").GetComponent<UILabel>().text = (GSKDATA.ManualSpeedR.H * 100).ToString();
        GameObject.Find("F117_S").GetComponent<UILabel>().text = (GSKDATA.ManualSpeedR.S * 100).ToString();
        GameObject.Find("F117_MM").GetComponent<UILabel>().text = GSKDATA.KaiJiSpeed;
    }

    //新建程序界面信息初始化
    private void newBuild_InfoSet()
    {
        updateProgramInfoList(StrNo3, StrName3, StrSize3, StrTime3, 5, newBuildPart_1.firstline);
        GSKDATA.ChengXuCount = GSKFile.GetFileDirectory().Count;
        newBuildPart_1.SetlistCount = GSKDATA.ChengXuCount;

        GameObject.Find("F121_label1").GetComponent<UILabel>().text = GSKDATA.ChengXuCount.ToString();
        GameObject.Find("F121_label3").GetComponent<UILabel>().text = GSKFile.GetMemotySize(GSKFile.GetFileDirectory()).ToString();
    }

    //程序一览界面信息初始化
    private void programV_InfoSet()
    {
        updateProgramInfoList(StrNo4, StrName4, StrSize4, StrTime4, 7, programVPart_1.firstline);
        GSKDATA.ChengXuCount = GSKFile.GetFileDirectory().Count;
        programVPart_1.SetlistCount = GSKDATA.ChengXuCount;

        GameObject.Find("F122_label1").GetComponent<UILabel>().text = GSKDATA.ChengXuCount.ToString();
        GameObject.Find("F122_label3").GetComponent<UILabel>().text = GSKFile.GetMemotySize(GSKFile.GetFileDirectory()).ToString();
    }

    //软键盘信息初始化
    private void keyboard_InfoSet()
    {
        GameObject.Find("keyInput").GetComponent<UILabel>().text = string.Empty;
        //xiaoxie
        keyboard_xx();
    }

    //指令帮助
    private void zhiLingHelp_InfoSet()
    {
        zhiLingHelp();
    }

    #endregion---------------------------

    //小写——软键盘
    private void keyboard_xx()
    {
        XiaoXie.isChosen();
        DaXie.noChosen();
        FuHao.noChosen();
        letterA.buttonLabel.text = "a"; letterP.buttonLabel.text = "p";
        letterB.buttonLabel.text = "b"; letterQ.buttonLabel.text = "q";
        letterC.buttonLabel.text = "c"; letterR.buttonLabel.text = "r";
        letterD.buttonLabel.text = "d"; letterS.buttonLabel.text = "s";
        letterE.buttonLabel.text = "e"; letterT.buttonLabel.text = "t";
        letterF.buttonLabel.text = "f"; letterU.buttonLabel.text = "u";
        letterG.buttonLabel.text = "d"; letterV.buttonLabel.text = "v";
        letterH.buttonLabel.text = "h"; letterW.buttonLabel.text = "w";
        letterI.buttonLabel.text = "i"; letterX.buttonLabel.text = "x";
        letterJ.buttonLabel.text = "j"; letterY.buttonLabel.text = "y";
        letterK.buttonLabel.text = "k"; letterZ.buttonLabel.text = "z";
        letterL.buttonLabel.text = "l"; letter27.buttonLabel.text = "";
        letterM.buttonLabel.text = "m"; letter28.buttonLabel.text = "";
        letterN.buttonLabel.text = "n"; letter29.buttonLabel.text = "";
        letterO.buttonLabel.text = "o"; letter30.buttonLabel.text = "";
    }

    //大写——软键盘
    private void keyboard_dx()
    {
        XiaoXie.noChosen();
        DaXie.isChosen();
        FuHao.noChosen();
        letterA.buttonLabel.text = "A"; letterP.buttonLabel.text = "P";
        letterB.buttonLabel.text = "B"; letterQ.buttonLabel.text = "Q";
        letterC.buttonLabel.text = "C"; letterR.buttonLabel.text = "R";
        letterD.buttonLabel.text = "D"; letterS.buttonLabel.text = "S";
        letterE.buttonLabel.text = "E"; letterT.buttonLabel.text = "T";
        letterF.buttonLabel.text = "F"; letterU.buttonLabel.text = "U";
        letterG.buttonLabel.text = "G"; letterV.buttonLabel.text = "V";
        letterH.buttonLabel.text = "H"; letterW.buttonLabel.text = "W";
        letterI.buttonLabel.text = "I"; letterX.buttonLabel.text = "X";
        letterJ.buttonLabel.text = "J"; letterY.buttonLabel.text = "Y";
        letterK.buttonLabel.text = "K"; letterZ.buttonLabel.text = "Z";
        letterL.buttonLabel.text = "L"; letter27.buttonLabel.text = "";
        letterM.buttonLabel.text = "M"; letter28.buttonLabel.text = "";
        letterN.buttonLabel.text = "N"; letter29.buttonLabel.text = "";
        letterO.buttonLabel.text = "O"; letter30.buttonLabel.text = "";
    }

    //符号——软键盘
    private void keyboard_fh()
    {
        XiaoXie.noChosen();
        DaXie.noChosen();
        FuHao.isChosen();
        letterA.buttonLabel.text = ","; letterP.buttonLabel.text = "!";
        letterB.buttonLabel.text = "\\"; letterQ.buttonLabel.text = "@";
        letterC.buttonLabel.text = "."; letterR.buttonLabel.text = "#";
        letterD.buttonLabel.text = ":"; letterS.buttonLabel.text = "$";
        letterE.buttonLabel.text = ";"; letterT.buttonLabel.text = "%";
        letterF.buttonLabel.text = "["; letterU.buttonLabel.text = "^";
        letterG.buttonLabel.text = "]"; letterV.buttonLabel.text = "&";
        letterH.buttonLabel.text = "'"; letterW.buttonLabel.text = "*";
        letterI.buttonLabel.text = "\""; letterX.buttonLabel.text = "(";
        letterJ.buttonLabel.text = "{"; letterY.buttonLabel.text = ")";
        letterK.buttonLabel.text = "}"; letterZ.buttonLabel.text = "_";
        letterL.buttonLabel.text = "|"; letter27.buttonLabel.text = "~";
        letterM.buttonLabel.text = "?"; letter28.buttonLabel.text = "`";
        letterN.buttonLabel.text = "<"; letter29.buttonLabel.text = "-";
        letterO.buttonLabel.text = ">"; letter30.buttonLabel.text = "+";
    }

    //更新示教点信息
    private void updatePointInfo(int num)
    {
        //获取当前机器人各个转动臂的信息
        RobotMotion Motionscript = GameObject.Find("MyMotion").GetComponent<RobotMotion>();
        float[] angle8 = Motionscript.CurrentAngle_All();
        string point = string.Empty;//
        string Pstr = string.Empty;
        for (int i = 0; i < 8; i++)
        {
            point += angle8[i].ToString();
            if (i != 7)
            {
                point += ",";
            }
        }
        Pstr = "P" + Convert.ToInt16(num).ToString("000") + "=" + point + ";";

        List<string> point_Contents = GSKFile.GetPointContents(GSKDATA.CurrentProgramName);

        for (int i = 0; i < point_Contents.Count; i++)
        {
            if (point_Contents[i].Substring(1, 3) == Convert.ToInt16(num).ToString("000"))//比较示教点序号是否相等
            {
                point_Contents[i] = Pstr;
            }
        }

        //写入
        List<string> file_Contents = GSKFile.GetFileContents(GSKDATA.CurrentProgramName);
        //加上point信息再写入文件
        file_Contents.InsertRange(0, point_Contents);
        file_Contents.Insert(0, "");
        GSKFile.writeContents(file_Contents, GSKDATA.CurrentProgramName);
    }

    //覆盖之前的示教点
    private void CoverPoint()
    {
        ProgramWin.CoverPoint();
    }

    //不覆盖之前的示教点
    private void NotCoverPoint()
    {
        ProgramWin.NotCoverPoint();
    }


    public void setInData(int num, int state)
    {
        interpreter.setInState(num, state);
    }

    public void setOutData(int num, int state)
    {
        interpreter.setOutState(num, state);
    }

    //info of axis7
    private void outShaftInfo(List<string> paraData)
    {
        if(paraData.Count > 3)
        {
            GSKDATA.AXIS7_SPEED = int.Parse(paraData[paraData.Count - 1]);
        }
        else
        {
            GSKDATA.AXIS7_SPEED = 0;
        }
    }
    
}
