using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Grabbing1 : MonoBehaviour
{
    // This script should be attached to both controller objects in the scene
    // Make sure to define the input in the editor (LeftHand/Grip and RightHand/Grip recommended respectively)
    Grabbing1 otherHand = null;
    public List<Transform> nearObjects = new List<Transform>();
    public Transform grabbedObject = null;


  //  public GameObject grabbedObject = null;
    public InputActionReference grabAction;
    public InputActionReference doubleRotation;
    private bool grabbing = false;
    bool doubleRot = false;

    private Vector3 worldPos;
    private Quaternion worldrot;


    private void Start()
    {
        grabAction.action.Enable();
        doubleRotation.action.Enable();


        // Find the other hand
        foreach(Grabbing1 c in transform.parent.GetComponentsInChildren<Grabbing1>())
        {
            if (c != this)
                otherHand = c;
        }
    }

    public bool getGrabbing(){
        return grabbing;
    }
    void beginGrab(){
        worldPos = transform.position;
        worldrot = transform.rotation;
        Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
        if (rb) {
            rb.isKinematic = true;
        }
        grabbing = true;
    }
    void endGrab(){
        
        
        if(otherHand.getGrabbing() == false){
        Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
        if (rb) {
            rb.isKinematic = false;
        }}
        grabbing = false;
    }


    void Update()
    {
        grabbing = grabAction.action.IsPressed();
        doubleRot = doubleRotation.action.IsPressed();
        if (grabbing)
        {
            // Grab nearby object or the object in the other hand
            if (!grabbedObject) {
                grabbedObject = nearObjects.Count > 0 ? nearObjects[0] : otherHand.grabbedObject;
                if(grabbedObject)
                    beginGrab();
            }

            if (grabbedObject)
            {
                // Change these to add the delta position and rotation instead
                // Save the position and rotation at the end of Update function, so you can compare previous pos/rot to current here

                Vector3 deltaPOSITION = transform.position - worldPos;
                Quaternion deltaROTATION = transform.rotation * Quaternion.Inverse(worldrot);


                Vector3 distanceHandToObject = grabbedObject.transform.position-transform.position;
                


                if(doubleRot == true){
                    Vector3 rotAroundHand_ = deltaROTATION*deltaROTATION*distanceHandToObject - distanceHandToObject;
                    grabbedObject.transform.root.position = grabbedObject.transform.root.position+deltaPOSITION+ rotAroundHand_;  
                    grabbedObject.transform.root.rotation =  deltaROTATION*deltaROTATION*grabbedObject.transform.root.rotation;
  
                }else{
                Vector3 rotAroundHand = deltaROTATION*distanceHandToObject - distanceHandToObject;
                grabbedObject.transform.root.position = grabbedObject.transform.root.position+deltaPOSITION+rotAroundHand;  
                grabbedObject.transform.root.rotation =  deltaROTATION*grabbedObject.transform.root.rotation;
                }
             
            }
        }
        // If let go of button, release object
        else if (grabbedObject){
            endGrab();
            grabbedObject = null;
        }
        // Should save the current position and rotation here
        if (grabbedObject){
            worldPos = transform.position;
            worldrot = transform.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Make sure to tag grabbable objects with the "grabbable" tag
        // You also need to make sure to have colliders for the grabbable objects and the controllers
        // Make sure to set the controller colliders as triggers or they will get misplaced
        // You also need to add Rigidbody to the controllers for these functions to be triggered
        // Make sure gravity is disabled though, or your controllers will (virtually) fall to the ground

        Transform t = other.transform;
        if(t && t.tag.ToLower()=="grabbable")
            nearObjects.Add(t);
    }

    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;
        if( t && t.tag.ToLower()=="grabbable")
            nearObjects.Remove(t);
    }
}