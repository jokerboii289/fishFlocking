using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateFish : MonoBehaviour
{
    [SerializeField]private FlockPoint[] flockPoints;
    void Start()
    {
        foreach (FlockPoint x in flockPoints)
            GenerateFish(x.flockpoint,x.fish,x.noOfFish,x.swimmingSpace);        
    }

   void GenerateFish(Transform pivot,GameObject fish,int noOfFish,Vector2 swimmingSpace)
    {
        pivot.GetComponent<Pivot>().allfish= new GameObject[noOfFish];//intialize the allfish array in pivot
        for (int i=0;i<noOfFish;i++)
        {
            //calculate position
            var pos = new Vector3(pivot.position.x + Rand(swimmingSpace),pivot.position.y,pivot.position.z+Rand(swimmingSpace));
            var fishObj=Instantiate(fish, pos, Quaternion.identity);
            var obj = fishObj.GetComponent<FishFlocking>();
            obj.pivot = pivot;  // assiging pivot to the each fish
            obj.space = swimmingSpace.y;
            var currentPivot = pivot.GetComponent<Pivot>();
            currentPivot.allfish[i] = fishObj;   //assigning values in array 
            currentPivot.space = swimmingSpace.y;
        }
    }
    
    float Rand(Vector2 space) { return Random.Range(space.x, space.y); }  
}

[System.Serializable]
public class FlockPoint
{
    public Transform flockpoint;
    public int noOfFish;
    public GameObject fish;
    public Vector2 swimmingSpace;
}
