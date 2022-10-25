using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMove : MonoBehaviour
{
    public static BoatMove instance;  
    public float speed;
    bool startboat;

    private void Awake()
    {
        instance = this; 
    }
    private void Start()
    {
        Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 0;
        Throw.boatStart += Startboat;
        startboat = false;
        Throw.stopBoat += StopBoat;
    }
    void Update()
    {        
        if(startboat)
            transform.position +=transform.forward * speed * Time.deltaTime;

        //var xInput = Input.GetMouseButton(0) ? Input.GetAxis("Mouse X") * speed * Mathf.Deg2Rad : 0; // sideways movement
        //transform.eulerAngles += Vector3.up * xInput * 2;
    }
    void Startboat()
    {
        startboat = !startboat;
    }

    void StopBoat() { speed = 0; }

    private void OnDisable()
    {
        Throw.boatStart -= Startboat;
    }
}
