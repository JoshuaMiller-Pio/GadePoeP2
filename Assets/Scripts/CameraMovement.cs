using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float Movespeed,RotateSpeed, moveTime,rotationSpeed,Rtime, Zsens, camDist;
    public Vector3 newPos;
    public Quaternion newRot;
    public CinemachineVirtualCamera cam;

    public CinemachineComponentBase ComponentBase;
    
    // Start is called before the first frame update
    void Start()
    {
        Rtime = 0.25f;
        newPos = transform.position;
        newRot = transform.rotation;
        ComponentBase = cam.GetCinemachineComponent(CinemachineCore.Stage.Body);
    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();
        zoom();
    }

    
    public void MovementInput()
    {
       
        if (Input.GetKey(KeyCode.W) ||Input.GetKey(KeyCode.UpArrow))
        {
            newPos += (transform.forward * Movespeed);
        }
        if (Input.GetKey(KeyCode.D) ||Input.GetKey(KeyCode.RightArrow))
        {
            newPos += (transform.right * Movespeed);
        }
        if (Input.GetKey(KeyCode.A) ||Input.GetKey(KeyCode.LeftArrow))
        {
            newPos += (-transform.right * Movespeed);
        }
        if (Input.GetKey(KeyCode.S) ||Input.GetKey(KeyCode.DownArrow))
        {
            newPos += (-transform.forward * Movespeed);
        }
        if (Input.GetKey(KeyCode.Q) )
        {
            newRot *= Quaternion.Euler(Vector3.up * -rotationSpeed);
        }
        if (Input.GetKey(KeyCode.E) )
        {
            newRot *= Quaternion.Euler(Vector3.up * rotationSpeed);
        }

      
        if ( (newPos.z < 150.3f && newPos.x < 160.8 && newPos.y < 140.8f ) && (newPos.z > -9.1f && newPos.x > -20.1f && newPos.y > 45.8f ) )
        {
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime*moveTime);
        }
        else
        {
            newPos = transform.position;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot,Time.deltaTime* RotateSpeed);
        StartCoroutine(rotate());

    }

    private void zoom()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            camDist = Input.mouseScrollDelta.y * Zsens;
            if (ComponentBase is CinemachineFramingTransposer &&
                (ComponentBase as CinemachineFramingTransposer).m_CameraDistance <= 100 && (ComponentBase as CinemachineFramingTransposer).m_CameraDistance >= 0) 
            {
                (ComponentBase as CinemachineFramingTransposer).m_CameraDistance -= camDist;
            }
            else
            {
                if ((ComponentBase as CinemachineFramingTransposer).m_CameraDistance > 100)
                {
                    (ComponentBase as CinemachineFramingTransposer).m_CameraDistance = 100;
                }
                else if ((ComponentBase as CinemachineFramingTransposer).m_CameraDistance < 0)
                    
                {
                    (ComponentBase as CinemachineFramingTransposer).m_CameraDistance = 0;
                }
            }
        }
        
      
    }
    IEnumerator rotate()
    {
        while (Rtime > 0) 
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, newRot,Time.deltaTime* RotateSpeed);
            Rtime -= Time.deltaTime;
            yield return new WaitForSeconds(0.001f);
        }

        Rtime = 0.25f;
        transform.rotation = transform.rotation;
        yield return null;
    }
}
