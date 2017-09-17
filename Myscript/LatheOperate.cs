using UnityEngine;
using System.Collections;

public class LatheOperate : MonoBehaviour {
    GameObject origianlWorkpiece;
    ButtonRespond button;

	// Use this for initialization
	void Start () {
        origianlWorkpiece = GameObject.Find("scx0261");
        button = GameObject.Find("MyButton").GetComponent<ButtonRespond>();
	}
	
	// Update is called once per frame
	void Update () {
        if (GSKDATA.Scene_NO == 3)
        {
            if (GSKDATA.ReappearRun)
            {
                if (GSKDATA.OutInfo[1])
                {
                    StartCoroutine(BeginWork());
                }
                if (GSKDATA.InInfo[1])
                {
                    button.SetOutfo(31, false);
                }
            }
        }
        
	    
	}

    IEnumerator BeginWork()
    {
        //Debug.Log("kaimen");
        yield return new WaitForSeconds(7.0f);
        if (ConditionCheck())
        {
            origianlWorkpiece.SetActive(false);
        }
        //开门
        button.SetOutfo(1, false);
        button.SetOutfo(31, true);
    }

    bool ConditionCheck()
    {
        if (GSKDATA.OutInfo[3])
        {
            if (GSKDATA.SensorSignal[1])
            {
                if (!GSKDATA.OutInfo[2])
                {
                    return true;
                }
            }
        }
        return false;
    }
}
