//<summary>
//DeviceMove#FILEEXTENSION#
//#PROJECTNAME#
//Created by #SMARTDEVELOPERS# on #CREATIONDATE#.
//Company: Sunnytech
//Function:
//
//
//<summary>
using UnityEngine;
using System.Collections;

public class DeviceMove : MonoBehaviour {
    //scene2
    DeviceBelong xq_1;



    //scene3
    DeviceLine men_z;
    DeviceLine men_y;
    DeviceLine men_z1;
    DeviceLine men_y1;
    DeviceLine ka1;
    DeviceLine ka2;
    DeviceLine ka3;
    DeviceLine ka1_;
    DeviceLine ka2_;
    DeviceLine ka3_;
    DistanceSeneor men_sensor_z;
    const float menSpeed = 0.01f;
    const float zhualineSpeed = 0.0005f;
    const float zhuaSpeed =0.5f;
    const float mdSpeed = 0.001f;
    DeviceBelong gj_1;
    //DeviceBelong gj_2;
    DeviceRotate zhua_z;
    DeviceRotate zhua_z1;
    DeviceRotate zhua_y;
    DeviceRotate zhua_y1;

    //
    DeviceLine big_63_2_1;
    DeviceLine big_63_2_2;
    DeviceLine big_63_2_1_false;
    DeviceLine big_63_2_2_false;
    DeviceBelong big_box;


	// Use this for initialization
	void Awake () {
        //1
        xq_1 = new DeviceBelong("qt3381", 1, 1);
        xq_1.SensorSignal = 10;


        //3
        men_z = new DeviceLine("VIFS9511", 1, new Vector3(1, 0, 0), -menSpeed, 1.606276f, 0.8332105f);
        men_y = new DeviceLine("VIFS84711", 1, new Vector3(1, 0, 0), menSpeed, 0.09332418f, -0.2446632f);
        men_z1 = new DeviceLine("VIFS9511", 31, new Vector3(1, 0, 0), menSpeed, 1.606276f, 0.8332105f);
        men_y1 = new DeviceLine("VIFS84711", 31, new Vector3(1, 0, 0), -menSpeed, 0.09332418f, -0.2446632f);
        men_sensor_z = new DistanceSeneor(1, "VIFS9511", new Vector3(1.606276f,0.24947f,-0.25305f));

        ka1 = new DeviceLine("zhua1", 3, new Vector3(0, 0, 1), -zhualineSpeed, -0.3012792f, -0.338398f);
        ka2 = new DeviceLine("zhua2", 3, new Vector3(0, 0, 1), zhualineSpeed, -0.4463772f, -0.4885937f);
        ka3 = new DeviceLine("zhua3", 3, new Vector3(0, 0, 1), -zhualineSpeed, -0.380332f, -0.3843685f);
        ka1_ = new DeviceLine("zhua1", 3, new Vector3(0, 0, 1), zhualineSpeed, -0.3012792f, -0.338398f,true);
        ka2_ = new DeviceLine("zhua2", 3, new Vector3(0, 0, 1), -zhualineSpeed, -0.4463772f, -0.4885937f, true);
        ka3_ = new DeviceLine("zhua3", 3, new Vector3(0, 0, 1), zhualineSpeed, -0.380332f, -0.3843685f, true);
        ka1.SensorSignal = 1; ka2.SensorSignal = 1; ka3.SensorSignal = 1;

        gj_1 = new DeviceBelong("scx0271", 2, 3);
        gj_1.detect_h_range = 0.001f;
        gj_1.detect_r_range = 0.001f;
        gj_1.SensorSignal = 2;
        gj_1.orignalScale = new Vector3(0.09999572f, 0.09999572f, 0.09999572f); gj_1.newScale = new Vector3(1.247476f, 1.247476f, 1.247476f);

        zhua_z = new DeviceRotate("RB50301", 2, new Vector3(1, 0, 0),zhuaSpeed, 20, 1);
        zhua_z1 = new DeviceRotate("RB50301", 2, new Vector3(1, 0, 0), -zhuaSpeed, 19, 0.5f, true);
        zhua_y = new DeviceRotate("RB50291", 2, new Vector3(1, 0, 0), zhuaSpeed, -1, -20);
        zhua_y1 = new DeviceRotate("RB50291", 2, new Vector3(1, 0, 0), -zhuaSpeed, -0.5f, -19, true);
        zhua_z.y = -0.381012f; zhua_z1.y = -0.381012f; zhua_z.z = -179.9078f; zhua_z1.z = -179.9078f;
        zhua_y.y = -0.3598938f; zhua_y1.y = -0.3598938f; zhua_y.z = 2.005913f; zhua_y1.z = 2.005913f;
        zhua_z.SensorSignal = 2; zhua_y.SensorSignal = 2;

        //6
        big_63_2_1 = new DeviceLine("big_63_2_1", 1, new Vector3(0, 0, 1), mdSpeed, -1.5f, -2.9f);
        big_63_2_2 = new DeviceLine("big_63_2_2", 1, new Vector3(0, 0, 1), -mdSpeed, 2.7f, 1.3f);

        big_63_2_1_false = new DeviceLine("big_63_2_1", 1, new Vector3(0, 0, 1), -mdSpeed, -1.5f, -2.9f, true);
        big_63_2_2_false = new DeviceLine("big_63_2_2", 1, new Vector3(0, 0, 1), mdSpeed, 2.7f, 1.3f, true);
        big_63_2_1.SensorSignal = 1;
        big_63_2_2.SensorSignal = 1;

        big_box = new DeviceBelong("passbox11", 1, 6);
        big_box.range_detect.range_h = 0.1f;
        big_box.range_detect.range_r = 0.1f;
        big_box.SensorSignal = 1;
        big_box.orignalScale = new Vector3(1, 1, 1); big_box.newScale = new Vector3(10, 10, 10);
	}
	
	// Update is called once per frame
	void Update () {
       
        if (GameObject.Find("CameraFree").GetComponent<Camera>().enabled)
        {
            if (GSKDATA.Scene_NO == 1)
            {
                xq_1.DMove();
                xq_1.CheckIn();
            }
            else if (GSKDATA.Scene_NO == 6)
            {
                big_63_2_1.DMove();
                big_63_2_1.CheckIn();
                big_63_2_2.DMove();
                big_63_2_2.CheckIn();

                big_63_2_1_false.DMove();
                big_63_2_1_false.CheckIn();
                big_63_2_2_false.DMove();
                big_63_2_2_false.CheckIn();

                big_box.DMove();
                big_box.CheckIn();
            }
            else if (GSKDATA.Scene_NO == 3)
            {
                men_z.DMove();
                men_z.CheckIn();
                men_z1.DMove();
                men_z1.CheckIn();
                men_y.DMove();
                men_y.CheckIn();
                men_y1.DMove();
                men_y1.CheckIn();
                men_sensor_z.CheckIn();

                gj_1.DMove();
                gj_1.CheckIn();

                //gj_2.DMove();
                //gj_2.CheckIn();

                zhua_z.DMove();
                zhua_z.CheckIn();
                zhua_z1.DMove();
                zhua_z1.CheckIn();
                zhua_y.DMove();
                zhua_y.CheckIn();
                zhua_y1.DMove();
                zhua_y1.CheckIn();

                ka1.DMove();
                ka1.CheckIn();
                ka2.DMove();
                ka2.CheckIn();
                ka3.DMove();
                ka3.CheckIn();
                ka1_.DMove();
                ka1_.CheckIn();
                ka2_.DMove();
                ka2_.CheckIn();
                ka3_.DMove();
                ka3_.CheckIn();
            }
        }
	}

    
}


