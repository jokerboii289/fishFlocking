using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour
{  
    float timer,time;
    public Vector3 goalPos; // for changing direction

    public GameObject[] allfish;

    public float space;
    private void Start()
    {
        goalPos = transform.position;
        timer = 0;
        time = Random.Range(2,5);
    }
    // Update is called once per frame
    void Update()
    {
        timer += 1 * Time.deltaTime;
      
        if(timer>time)
        {
            time = Random.Range(2,5);
            ChangePosition();
            timer = 0;
        }
    }

    void ChangePosition() { goalPos = new Vector3(transform.position.x + Rand(), transform.position.y, transform.position.z + Rand()); }
 
    float Rand() { return Random.Range(-space, space); }  
}
