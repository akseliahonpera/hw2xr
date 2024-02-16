using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;


public class MagClassAngledView : MonoBehaviour
{
    public Transform vrCameraTransform;
    public Transform magGlassTransform;
    public Transform magGlassCameraTransform;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
           

           // magGlassCameraTransform.rotation = vrCameraTransform.rotation;

           // magGlassCameraTransform.rotation = vrCameraTransform.rotation;


            magGlassCameraTransform.LookAt(vrCameraTransform.position, magGlassTransform.rotation * Vector3.up  );
            magGlassCameraTransform.Rotate(0.0f, 180.0f, 0.0f);
            
    }
}
