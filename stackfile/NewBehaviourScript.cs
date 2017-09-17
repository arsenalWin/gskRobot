using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class NewBehaviourScript : MonoBehaviour {
    PXClass px0;
    PXClass px1;
    PXClass px2;
    PXClass px3;
    PXClass px4;
    PXClass px5;

    PXList pxList;

    // Use this for initialization
    void Start () {
        px0 = new PXClass
        {
            X = 0,
            Y = 0,
            Z = 20,
            W = 0,
            P = 0,
            R = 0
        };
        px1 = new PXClass
        {
            X = 0,
            Y = 0,
            Z = 20,
            W = 0,
            P = 0,
            R = 0
        };
        px2 = new PXClass
        {
            X = 0,
            Y = 0,
            Z = 20,
            W = 0,
            P = 0,
            R = 0
        };
        px3 = new PXClass
        {
            X = 0,
            Y = 0,
            Z = 20,
            W = 0,
            P = 0,
            R = 0
        };
        px4 = new PXClass
        {
            X = 0,
            Y = 0,
            Z = 20,
            W = 0,
            P = 0,
            R = 0
        };
        px5 = new PXClass
        {
            X = 0,
            Y = 0,
            Z = 20,
            W = 0,
            P = 0,
            R = 0
        };

        pxList = new PXList
        {
            PX0 = px0,
            PX1 = px1,
            PX2 = px2,
            PX3 = px3,
            PX4 = px4,
            PX5 = px5
        };

        string jsonString = JsonConvert.SerializeObject(pxList);
        Debug.Log(jsonString);
        string fileName = "/pxList";
        File.WriteAllText(Application.dataPath + "/StreamingAssets/PX" + fileName, jsonString);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
