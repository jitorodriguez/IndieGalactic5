using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroG_Controller : MonoBehaviour
{
    public float torque;
    public float force;
    float oldX;
    float oldY;
    Rigidbody rb;

    public GameObject catMaker;
    Cat_Maker catMakerScript;
    string[] CAT_TYPE = new string[4] { "Tomcat", "Bullet Cat", "Spaz", "MiniMeow" };
    private int catType = 0;
    
    int timerCount = 0;
    int timeBtwnShots = 90;

    int[] catQuota = new int[3] { 999, 999, 999 };

    private void Start()
    {
        catMakerScript = catMaker.GetComponent<Cat_Maker>();
        catMakerScript.SetCat(0);

        Vector3 mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        oldX = mousePosition.x;
        oldY = mousePosition.y;
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        timerCount++;
        Vector3 mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        print(mousePosition.x + ", " + oldX);

        //Num Keys 1-4
        if (Input.GetKey(KeyCode.Alpha1))
        {
            catType = 0;
            catMakerScript.DestroyCat();
            if (catQuota[catType] > 0)
            {
                catMakerScript.SetCat(0);
            }
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            catType = 1;
            catMakerScript.DestroyCat();
            if (catQuota[catType] > 0)
            {
                catMakerScript.SetCat(1);
            }
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            catType = 2;
            catMakerScript.DestroyCat();
            if (catQuota[catType] > 0)
            {
                catMakerScript.SetCat(2);
            }
        }

        oldX = mousePosition.x;
        oldY = mousePosition.y;

        Vector3 cameraAngel = Camera.main.transform.localEulerAngles;
        Camera.main.transform.localEulerAngles = new Vector3(-(50 * mousePosition.y - 25), 50 * mousePosition.x - 25, cameraAngel.z);

        //Left Mouse
        if (Input.GetMouseButtonDown(0))
        {
            if (timerCount <= timeBtwnShots)
            {
                return;
            } else
            {
                timerCount = 0;
            }
            Vector3 controlDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 1);
            Vector3 actualDirection = Camera.main.transform.TransformDirection(controlDirection);
            if (catType == 0)
            {
                rb.AddForce(-actualDirection * 2, ForceMode.Impulse);
                catMakerScript.LaunchCat(actualDirection);
                catQuota[catType]--;
                if (catQuota[catType] > 0)
                {
                    catMakerScript.SetCat(0);
                }
            }
            else if (catType == 1)
            {
                rb.AddForce(-actualDirection * 4, ForceMode.Impulse);
                catMakerScript.LaunchCat(actualDirection);
                catQuota[catType]--;
                if (catQuota[catType] > 0)
                {
                    catMakerScript.SetCat(1);
                }
            }
            else if (catType == 2)
            {
                if (oldX > 0 || oldX < 0)
                {
                    rb.AddTorque(transform.up * torque * oldX);
                }
                if (oldY > 0 || oldY < 0)
                {
                    rb.AddTorque(-transform.right * torque * oldY);
                }
                if ((oldX > 0 || oldX < 0) && !(oldY > 0 || oldY < 0))
                {
                    catMakerScript.FlingCat(actualDirection * 1, - transform.up * torque * oldX);
                } else if (!(oldX > 0 || oldX < 0) && (oldY > 0 || oldY < 0))
                {
                    catMakerScript.FlingCat(actualDirection * 1, transform.right * torque * oldY);
                } else if ((oldX > 0 || oldX < 0) && (oldY > 0 || oldY < 0))
                {
                    catMakerScript.FlingCat(actualDirection * 1, -transform.up * torque * oldX, transform.right * torque * oldY);
                }
                catQuota[catType]--;
                if (catQuota[catType] > 0)
                {
                    catMakerScript.SetCat(2);
                }
            }
        }
    }
}
