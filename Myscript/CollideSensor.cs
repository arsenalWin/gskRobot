using UnityEngine;
using System.Collections;

public class CollideSensor : MonoBehaviour {
    ButtonRespond button;
    public int SensorSignal = 0;
    string SensorTag = "sensor";
    public bool feedback = false;//是否有反馈信息
	// Use this for initialization
	void Start () {
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        SensorTag = "sensor";
        button = GameObject.Find("MyButton").GetComponent<ButtonRespond>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.transform.tag == SensorTag)
        {
            GSKDATA.SensorSignal[SensorSignal] = true;
            //Debug.Log("SensorSignal:"+SensorSignal);
            if (feedback)
            {
                button.SetInfo(SensorSignal,true);
            }
        }
        else
        {
            GSKDATA.IsCollide = true;
        }
    }
    void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.transform.tag == SensorTag)
        {
            GSKDATA.SensorSignal[SensorSignal] = true;
            if (feedback)
            {
                button.SetInfo(SensorSignal, true);
            }
        }
        else
        {
            GSKDATA.IsCollide = true;
        }
    }
    void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.transform.tag == SensorTag)
        {
            GSKDATA.SensorSignal[SensorSignal] = false;
            if (feedback)
            {
                button.SetInfo(SensorSignal, false);
            }
        }
        else
        {
            GSKDATA.IsCollide = false;
        }
    }
    
}
