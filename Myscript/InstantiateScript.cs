using UnityEngine;
using System.Collections;

public class InstantiateScript : MonoBehaviour {
    DeviceBelong big_box;
	// Use this for initialization
	void Start () {
        big_box = new DeviceBelong(gameObject.name, 1, 6);
        big_box.range_detect.range_h = 0.1f;
        big_box.range_detect.range_r = 0.1f;
        big_box.SensorSignal = 1;
        big_box.orignalScale = new Vector3(1, 1, 1); big_box.newScale = new Vector3(10, 10, 10);
	}
	
	// Update is called once per frame
	void Update () {
        if (GSKDATA.Scene_NO == 6)
        {
            big_box.DMove();
            big_box.CheckIn();
        }
	}
}
