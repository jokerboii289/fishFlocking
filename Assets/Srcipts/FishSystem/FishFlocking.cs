using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FishFlocking : MonoBehaviour
{
    public Transform pivot;
    public float speed;
    public float space;

    private float _rotationSpeed=5f;
    private Vector3 _averageHeading,_averagePosition;
    private float neighbourDistance = 3f;

    bool turning;

    Animator animator;
    private Transform boat;
    private Collider fishCollider;
  
    bool stopMoving;

    // Start is called before the first frame update
    void Start()
    {
        stopMoving = false;
        
        boat = GameObject.FindGameObjectWithTag("boat").transform;
        fishCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        speed = Random.Range(1,8);
        StartCoroutine(DelayAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopMoving)
        {
            if (Vector3.Distance(transform.position, pivot.position) >= space)
                turning = true;
            else
                turning = false;

            if (turning)
            {
                var direction = pivot.position - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _rotationSpeed * Time.deltaTime);
                speed = Random.Range(1, 8);
            }
            else
            {
                if (Random.Range(0, 6) < 1)// determines how often the rules are applied
                    ApplyRules();
            }
            transform.Translate(0, 0, speed * Time.deltaTime);

            //for collider deactivate for performance
            if (Vector3.Distance(boat.position, transform.position) < 40)
                fishCollider.enabled = true;
            else
                fishCollider.enabled = false;
        }
    }

    void ApplyRules()
    {
        GameObject[] gos = pivot.GetComponent<Pivot>().allfish;

        Vector3 vCentre = Vector3.zero;
        Vector3 avoid = Vector3.zero;
        float gSpeed = .1f;

        Vector3 goalPos = pivot.GetComponent<Pivot>().goalPos;

        float dist;

        int groupSize = 0;

        foreach(GameObject fish in gos)
        {
            if(fish!=this.gameObject)
            {
                dist = Vector3.Distance(fish.transform.position,this.transform.position);
                if(dist<=neighbourDistance)//checking no of fish within grouping range
                {
                    vCentre += fish.transform.position;
                    groupSize++;

                    if (dist < 1) // for avoidance
                        avoid += (this.transform.position - fish.transform.position);

                    var anotherFish = fish.GetComponent<FishFlocking>();
                    gSpeed += anotherFish.speed;
                }
            }
        }

        if(groupSize>0)
        {
            vCentre = vCentre / groupSize + (goalPos-this.transform.position);
            speed = gSpeed / groupSize;
           
            var direction = (vCentre + avoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(direction),_rotationSpeed*Time.deltaTime);
        }
    }

    IEnumerator DelayAnimation()
    {
        yield return new WaitForSeconds(Random.Range(.1f,2.5f));
        animator.SetTrigger("swim");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("fishnet"))
        {
            var fishCollection = GameObject.FindGameObjectWithTag("fishcollection").transform;
            print("hit net");
            stopMoving = true;                                                                          //offset make 5 when boat control is off  
            transform.DOMove(new Vector3(fishCollection.position.x, transform.position.y, fishCollection.position.z+ 10),.5f).OnComplete(()=> { transform.SetParent(fishCollection);
                fishCollider.enabled = false;
            });
        }
    }
} 
