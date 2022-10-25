using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject dummyfishNet, originalfishNet; //include when to throw fish net
   
    // Start is called before the first frame update
    void Start()
    {
        Throw.stopBoat += PullNet;
        dummyfishNet.SetActive(true);
        originalfishNet.SetActive(false);
      //  StartCoroutine(DelayNetThrow());
    }

    public void DelayNetThrow()
    {      
        dummyfishNet.SetActive(false);
        originalfishNet.SetActive(true);
    }

    void PullNet()
    {
        GetComponent<Animator>().SetTrigger("pull");
    }
}
