using UnityEngine;
using System.Collections;

public class CharactorMove : MonoBehaviour 
{
	//移动速度
    public float MoveSpeed = 4.0F;
	//左右旋转速度
	public float RightLeftSpeed = 10.0f;
	//上下旋转速度
	public float UpDownSpeed = 10.0f;
    public float jumpSpeed = 15.0f;
    private float gravity = 40.0F;
    private Vector3 moveDirection = Vector3.zero;
    private Transform childTrans = null;
	private CharacterController controller;

	void Start () 
	{
		controller = GetComponent<CharacterController>();
        if (transform.childCount > 0)
        {
            childTrans = transform.GetChild(0);
        }
	}
	

    void Update() 
	{
        if (GameObject.Find("Main Camera").GetComponent<Camera>().enabled)
        {
            if (controller.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= MoveSpeed;
                if (Input.GetButton("Jump"))
                    moveDirection.y = jumpSpeed;
            }
            if (Time.timeScale != 0)
            {
                //平移
                moveDirection.y -= gravity * Time.deltaTime / Time.timeScale;
                controller.Move(moveDirection * Time.deltaTime / Time.timeScale);


                //左右旋转
                if (Input.GetKey(KeyCode.E))
                {
                    transform.Rotate(Vector3.up * 0.08f * RightLeftSpeed, Space.Self);
                }

                if (Input.GetKey(KeyCode.Q))
                {
                    transform.Rotate(Vector3.up * -0.08f * RightLeftSpeed, Space.Self);
                }

                //上下旋转
                if (childTrans != null)
                {
                    if (Input.GetKey(KeyCode.Z))
                    {
                        if ((childTrans.eulerAngles.x < 90 && childTrans.eulerAngles.x >= -1) || (childTrans.eulerAngles.x > 275))
                            childTrans.Rotate(Vector3.left * 0.08f * UpDownSpeed);
                    }

                    if (Input.GetKey(KeyCode.X))
                    {
                        if ((childTrans.eulerAngles.x < 85 && childTrans.eulerAngles.x >= -1) || (childTrans.eulerAngles.x > 270))
                            childTrans.Rotate(Vector3.left * (-0.08f) * UpDownSpeed);
                    }
                }

            }
            else
            {
                moveDirection.y = 0;
                if (Input.GetKey(KeyCode.S))
                {
                    moveDirection.z = -1.0F;
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    moveDirection.z = 1 * 1.0F;
                }
                else
                {
                    moveDirection.z = 0.0F;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    moveDirection.x = 1 * 1.0F;
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    moveDirection.x = -1 * 1.0F;
                }
                else
                {
                    moveDirection.x = 0.0F;
                }

                moveDirection = transform.TransformDirection(moveDirection);

                moveDirection *= MoveSpeed;
                moveDirection.y -= gravity * 0.016F;
                controller.Move(moveDirection * 0.016F);
            }
        }
        
    }
}
