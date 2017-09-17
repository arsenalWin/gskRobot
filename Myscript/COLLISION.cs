using UnityEngine;
using System.Collections;

public class COLLISION : MonoBehaviour {
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        GSKDATA.IsCollide = true;
    }
    void OnCollisionStay(Collision collisionInfo)
    {
        GSKDATA.IsCollide = true;
    }
    void OnCollisionExit(Collision collisionInfo)
    {
        GSKDATA.IsCollide = false;
    }
}
