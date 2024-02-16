using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


public class TESTIGRAB : MonoBehaviour
{
    // This script should be attached to both controller objects in the scene
    // Make sure to define the input in the editor (LeftHand/Grip and RightHand/Grip recommended respectively)
    TESTIGRAB otherHand = null;
    public GameObject grabbedObject = null;
    public GameObject grabTarget = null;
    public InputActionReference grabAction;
    bool grabbing = false;
    private Vector3 worldPos;
    private Quaternion worldrot;

    GameObject getGrabbedObject(){
        return grabbedObject;
    }

    private void Start()
    {
        Debug.Log("Start");
        grabAction.action.Enable();
       
        // Find the other hand
        foreach(TESTIGRAB c in transform.parent.GetComponentsInChildren<TESTIGRAB>())
        {
            Debug.Log("Find other hand");
            if (c != this)
                otherHand = c;
        }
    }

    void beginGrab(){
            if(grabbedObject.GetComponent<Rigidbody>() != null){
                Debug.Log("Begin grab"+grabbedObject);
                worldPos  = transform.position;
                worldrot  = transform.rotation;
                Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                grabbing = true;
            }
    }
    void endGrab(){
        Debug.Log("End grab");
        grabbedObject.transform.position = worldPos;
        grabbedObject.transform.rotation = worldrot;
        Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        grabbing = false;
    }

    void Update()
    {
        grabbing = grabAction.action.IsPressed();
        if (grabbing)
        {
            // Grab nearby object or the object in the other hand
            if (!grabbedObject){
                grabbedObject = otherHand.getGrabbedObject();
                    if(grabbedObject)
                        beginGrab();
            }
            if (grabbedObject)
            {
                // Change these to add the delta position and rotation instead
                // Save the position and rotation at the end of Update function, so you can compare previous pos/rot to current here

                Vector3 deltaPOSITION = transform.position-worldPos;
                Quaternion deltaROTATION = transform.rotation * Quaternion.Inverse(worldrot);
                
           

                grabbedObject.transform.root.position += deltaPOSITION;
                grabbedObject.transform.root.rotation =  deltaROTATION*grabbedObject.transform.root.rotation   ;
            }
        }
        // If let go of button, release object
        else if (grabbedObject){
            endGrab();
            grabbedObject = null;
        }
        // Should save the current position and rotation here
    if (grabbedObject){
    worldPos = transform.root.position;
    worldrot = transform.rotation;
    }
    }

  private void OnTriggerEnter(Collider other)
    {      
        Debug.Log("trigger enter");
        if(other.gameObject != null && other.gameObject.tag.ToLower()=="grabbable"){
            grabbedObject = other.gameObject;
 
             Debug.Log("assigned grabbedobject"+grabbedObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("trigger exit");
        if(!grabbing){
        grabbedObject=null;
        Debug.Log("de-assigned grabbedobject");
        }

    }
}
