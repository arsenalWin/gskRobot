using UnityEngine;
using System.Collections;

public class doLableData{

	public Vector3  vPosStart;
	public Vector3  vPosEnd;
	public Rect	    rGUIRect;
	//public Vector3	vPosWordToScreen;
	public string  strPartName;
	public doLableData()
	{
		vPosStart 	= new Vector3(0,0,0);
		vPosEnd 	= new Vector3(0,0,0);
		rGUIRect	= new Rect(0,0,200,28);
		//vPosWordToScreen = new Vector3(0,0,0);
		strPartName = "";
	}
	
	public doLableData(doLableData other)
	{
		vPosStart = other.vPosStart;
		vPosEnd   = other.vPosEnd;
		rGUIRect  = other.rGUIRect;
		strPartName = other.strPartName;
	}
	
	public void setData(doLableData other)
	{
		vPosStart = other.vPosStart;
		vPosEnd   = other.vPosEnd;
		rGUIRect  = other.rGUIRect;
		strPartName = other.strPartName;
	}
}
