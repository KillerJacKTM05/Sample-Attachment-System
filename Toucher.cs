using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class Toucher : MonoBehaviour
{
    public LeanFinger fingerTouch;
    [HideInInspector] public bool isAnyFingerTouching;

    private GameObject beforeUI;
    void Start()
    {
        beforeUI = UIManager.Instance.getBeforeUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (beforeUI.activeInHierarchy)
        {
            ProcessInput();
        }
    }

    private void ProcessInput()
    {
        var fingers = LeanTouch.Fingers;
        isAnyFingerTouching = fingers.Count > 0;
        if (isAnyFingerTouching)
        {
            fingerTouch = fingers[0];
            Ray Casted = Camera.main.ScreenPointToRay(fingers[0].ScreenPosition);
            RaycastHit hitted;

            if (fingers[0].Down)
            {
                //Debug.Log("Touch Started");
            }
            else if (fingers[0].Set)
            {
                //Debug.Log("Girdi");
                if(AttachmentManager.I.createdObj != null)
                {
                    if (Physics.Raycast(Casted, out hitted, 30, 1 << 9))
                    {
                        //Debug.Log("Hit point on character:" + hitted.point);
                        AttachmentManager.I.SetPos(hitted.point);
                    }
                    else if (Physics.Raycast(Casted, out hitted, 30, 1 << 10))
                    {
                        //Debug.Log("Hit point on word:" + hitted.point);
                        AttachmentManager.I.SetPos(hitted.point);
                    }
                   
                }
                else
                {

                    if (Physics.Raycast(Casted, out hitted, 30, 1 << 11))
                    {
                        //Debug.Log("Hit point on attachment:" + hitted.point);
                        hitted.rigidbody.gameObject.transform.parent = null;
                        AttachmentManager.I.objCreated = true;
                        AttachmentManager.I.createdObj = hitted.rigidbody.gameObject;
                        AttachmentManager.I.SetPos(hitted.point);
                    }

                }

            }
            else if (fingers[0].Up)
            {
                if(AttachmentManager.I.createdObj!=null)
                {
                    AttachmentManager.I.objCreated = false;
                    if (Physics.Raycast(Casted, out hitted, 30, 1 << 9))
                    {
                        AttachmentManager.I.createdObj.transform.SetParent(hitted.rigidbody.gameObject.transform);
                        var temp = AttachmentManager.I.createdObj.GetComponent<Balloon>();
                        if(temp != null)
                        {
                            temp.otherRb = hitted.rigidbody;
                        }
                        AttachmentManager.I.createdObj = null;
                    }
                    else
                    {
                        Destroy(AttachmentManager.I.createdObj);
                    }
                }
                
            }
        }
    }
}
