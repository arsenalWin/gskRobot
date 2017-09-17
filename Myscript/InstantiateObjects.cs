using UnityEngine;
using System.Collections;

public class InstantiateObjects : MonoBehaviour {

    public Transform Box;
	// Use this for initialization
    Vector3 objectPosition = new Vector3(-1.349186f,-1.625375f,-2.614297f);

    GameObject originalObj;

    bool doing = false;
    
    int num = 0;
	void Start () {
        objectPosition = new Vector3(-1.599212f, -1.462057f, -1.845448f);
        num = 0;
        originalObj = GameObject.Find("passbox11");
	}
	
	// Update is called once per frame
	void Update () {
        if (originalObj.transform.position.x < -2.0f && !doing)
        {
            InstantiateBox();
        }
	}

    public string InstantiateBox()
    {
        doing = true;
        Object obj;
        obj = Instantiate(Box, objectPosition, Quaternion.identity);
        obj.name = "MyObject_" + num;
        originalObj = GameObject.Find(obj.name);
        num++;
        doing = false;
        originalObj.AddComponent<InstantiateScript>();
        return obj.name;
    }

    public void DestoryBox()
    {
        //Debug.Log("ready to destory");
        for (int i = 0; i <= num; i++)
        {
            //Debug.Log("destory the box");
            Destroy(GameObject.Find("MyObject_" + i));
        }
        originalObj = GameObject.Find("passbox11");
        num = 0;
    }
}
