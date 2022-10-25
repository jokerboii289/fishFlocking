using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Obi;
using System;


public class Throw : MonoBehaviour
{
    public ObiParticleAttachment[] obj; //edge points
    public GameObject[] edgePoints;// points

    public ObiParticleAttachment midPoint;
    
    bool stop;
    [Header("Throwing Net")]
    [SerializeField]private float offsetZ,offsetY,timer,rotationSpeed;
    [SerializeField]private Transform edgeHolder;
    [SerializeField] private GameObject netDropParticle;

    public static Action boatStart;

    [Header("FishNetMovement")]
    [SerializeField]private float xSpeed,speed;
    [SerializeField] private Transform netMidPoint;

    [SerializeField] private ObiParticleAttachment p;//pointro release up on boat movee
    [SerializeField] private GameObject point;

    [Header("Rope")]
    [SerializeField]private ObiRope rope;

    private Transform fishCollection;

    [SerializeField] private Transform fishCollectionPoint;
    public static Action stopBoat;

    [SerializeField]private GameObject initialCamera;

    [Header("BoatMovement")]
    [SerializeField] private bool boatMovement;
    [SerializeField] private Transform boat;


    // Start is called before the first frame update
    void Start()
    {        
        stopBoat += FishNetPull;
        fishCollection= GameObject.FindGameObjectWithTag("fishcollection").transform;

        speed = BoatMove.instance.speed;
        midPoint.enabled = true;
        if(obj!=null)
            foreach (ObiParticleAttachment x in obj)
                x.enabled = true;
        stop = true;
        NetThrow();
    }

    void NetThrow()
    {
        rope.stretchingScale = 3;
        stop = false;
        transform.DOJump(new Vector3(transform.position.x,transform.position.y-offsetY,transform.position.z-offsetZ),10,1,timer).OnComplete(()=> {
            Instantiate(netDropParticle, transform.position, Quaternion.identity);//particle effect
            transform.DOMove(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), .4f).OnComplete(()=> { boatStart(); stop = !stop;
               //make the points closer and bulkier net
               //netMidPoint.DOMoveY(netMidPoint.position.y+1f,.5f);

                p.enabled = false;
                point.SetActive(false);
                initialCamera.SetActive(false);
            });
            });
    }

    private void Update()
    {
        fishCollection.position = transform.position;
        if(!stop)
            edgeHolder.eulerAngles += new Vector3(0,0,200*rotationSpeed*Time.deltaTime);

        MovementOfFishingNet();     
    }
  

    void MovementOfFishingNet()//after deployed
    {
        if (!boatMovement)
        {
            var xInput = Input.GetMouseButton(0) ? Input.GetAxis("Mouse X") * xSpeed * Mathf.Deg2Rad : 0;
            transform.position += Vector3.right * xInput;
        }
        else
        {
            //transform.position = Vector3.Lerp(transform.position, new Vector3(boat.position.x, transform.position.y, transform.position.z), 1.5f * Time.deltaTime);
        }

        //moveForward
        if (stop)
            transform.position += -transform.up * speed *Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("gate"))
        {
            if(other.GetComponent<Gate>().typeOfGate.Equals("add"))
                transform.localScale += Vector3.one * .5f;
            else if (other.GetComponent<Gate>().typeOfGate.Equals("minus"))
                transform.localScale -= Vector3.one * .5f;
        }
        
        if (other.gameObject.CompareTag("stoppoint"))
            stopBoat();
    }


    void FishNetPull()
    {
        if (obj != null)
        {
            foreach (ObiParticleAttachment x in obj)
                x.enabled = false;
            foreach (GameObject obj in edgePoints)
                obj.SetActive(false);
        }
        transform.DOMove(fishCollectionPoint.position,10f);
        rope.stretchingScale = 1;
        speed = 0;
    }

    private void OnDisable() { stopBoat -= FishNetPull; }
   
}
