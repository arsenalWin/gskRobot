using UnityEngine;
using System.Collections;

public class ResultDetect : MonoBehaviour {
    float[] posDeviation = new float[6]{0,0,0,0,0,0};

    void OnGUI()
    {
        if (GSKDATA.SoftCurrentMode == "Exam" || GSKDATA.SoftCurrentMode == "Exercise")
        {
            GUI.skin.label = FuncPara.defaultSkin.label;
            GUI.skin.label.font = FuncPara.defaultFont;
            GUI.skin.label.fontSize = 17;
            GUI.skin.label.normal.textColor = Color.white;
            GUILayout.Label("各个关节距离目标点的转角差");
            GUI.skin.label.normal.textColor = Color.red;
            GUILayout.BeginArea(new Rect(0, 27, 100, 100));
            GUILayout.BeginVertical("BOX");
            GUILayout.Label("S : " + (int)posDeviation[0]);
            GUILayout.Label("L : " + (int)posDeviation[1]);
            GUILayout.Label("U : " + (int)posDeviation[2]);
            GUILayout.EndVertical();
            GUILayout.EndArea();
            GUILayout.BeginArea(new Rect(125, 27, 100, 100));
            GUILayout.BeginVertical("BOX");
            GUILayout.Label("R : " + (int)posDeviation[3]);          
            GUILayout.Label("B : " + (int)posDeviation[4]);
            GUILayout.Label("T : " + (int)posDeviation[5]);
            GUILayout.EndVertical();
            GUILayout.EndArea();
            GUI.skin.label = null;
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        if (GSKDATA.SoftCurrentMode == "Exam" || GSKDATA.SoftCurrentMode == "Exercise")
        {
            switch (GSKDATA.CaseName)
            {
                case "Case1":
                    switch (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.DetectNo)
                    {
                        case 1://新建程序
                            break;
                        case 11://还原场景
                            break;
                        case 12://再现
                            break;
                        case 2:case 3:case 4:case 5:case 6:case 7:case 8:case 9:case 10:
                            posDeviation = GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.PosDeviation();
                            if (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.CodeDetect() && GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.PositionDetect())
                            {
                                Debug.Log("right");
                                FuncPara.loopControl = 14;
                                GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.RecordScore();
                            }
                            //else
                            //{
                            //    Debug.Log(GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.CodeDetect());
                            //    Debug.Log(GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.PositionDetect());
                            //}
                            break;
                    }
                    break;
                case "Case2":
                    switch (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.DetectNo)
                    {
                        case 1://新建程序
                            break;
                        case 7://qingkong
                            break;
                        case 8://再现
                            break;
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                            posDeviation = GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.PosDeviation();
                            if (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.CodeDetect() && GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.PositionDetect())
                            {
                                FuncPara.loopControl = 14;
                                GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.RecordScore();
                            }
                            break;
                    }
                    break;
                case "Case3":
                    switch (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.DetectNo)
                    {
                        case 1://新建程序
                            break;
                        case 32://还原场景
                            break;
                        case 33://再现
                            break;
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                        case 26:
                        case 27:
                        case 28:
                        case 29:
                        case 30:
                        case 31:
                            posDeviation = GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.PosDeviation();
                            if (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.CodeDetect() && GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.PositionDetect())
                            {
                                FuncPara.loopControl = 14;
                                GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.RecordScore();
                            }
                            break;
                    }
                    break;
                case "Case4":
                    switch (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.DetectNo)
                    {
                        case 1://新建程序
                            break;
                        case 10://再现运行
                            break;
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            posDeviation = GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.PosDeviation();
                            if (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.CodeDetect() && GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.PositionDetect())
                            {
                                FuncPara.loopControl = 14;
                                GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.RecordScore();
                            }
                            break;
                    }
                    break;
                case "Case6":
                    switch (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.DetectNo)
                    {
                        case 1://新建程序
                            break;
                        case 13://再现运行
                            break;
                        case 14://再现运行
                            break;
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                            posDeviation = GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.PosDeviation();
                            if (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.CodeDetect() && GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.PositionDetect())
                            {
                                FuncPara.loopControl = 14;
                                GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.RecordScore();
                            }
                            break;
                    }
                    break;
            }
        }
	}

    #region --------------------教练考的检测--------------------------
    //新建程序
    public void DetectNewBuild(string name)
    {
        if (GSKDATA.CaseName != "")
        {
            Debug.Log(GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.DetectNo);
            if (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.DetectNo == 1)
            {
                GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.FileName = name;
                ResultRight();
            }
        }
    }
    //再现运行
    public void DetectReappear()
    {
        if (GSKDATA.CaseName != "")
        {
            switch (GSKDATA.CaseName)
            {
                case "Case1":
                    if (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.DetectNo == 12)
                    {
                        ResultRight();
                    }
                    break;
                case "Case2":
                    if (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.DetectNo == 8)
                    {
                        ResultRight();
                    }
                    break;
                case "Case3":
                    if (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.DetectNo == 33)
                    {
                        ResultRight();
                    }
                    break;
                case "Case4":
                    if (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.DetectNo == 10)
                    {
                        ResultRight();
                    }
                    break;
                case "Case6":
                    if (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.DetectNo == 14)
                    {
                        ResultRight();
                    }
                    break;
            }
        }
    }
    //还原场景
    public void DetectResetScene()
    {
        if (GSKDATA.CaseName != "")
        {
            switch (GSKDATA.CaseName)
            {
                case "Case1":
                    if (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.DetectNo == 11)
                    {
                        ResultRight();
                    }

                    break;
                case "Case3":
                    if (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.DetectNo == 32)
                    {
                        ResultRight();
                    }

                    break;
                case "Case6":
                    if (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.DetectNo == 13)
                    {
                        ResultRight();
                    }

                    break;
            }
        }
    }

    //还原场景
    public void ClearPanel()
    {
        if (GSKDATA.CaseName != "")
        {
            switch (GSKDATA.CaseName)
            {
                case "Case2":
                    if (GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.DetectNo == 7)
                    {
                        ResultRight();
                    }
                    break;
            }
        }
    }
    //结果正确
    public void ResultRight()
    {
        GameObject.Find("MainScript").GetComponent<TeachWinow>().MyCase.RecordScore();
        FuncPara.loopControl = 14;
    }
    #endregion
}
