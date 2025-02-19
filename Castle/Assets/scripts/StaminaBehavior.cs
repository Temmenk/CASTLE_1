using UnityEngine;

public class StaminaBehavior : MonoBehaviour
 {
     public GameBehavior gameManager;
 
     void Start()
     {  
           gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();
     }
     void OnCollisionEnter(Collision collision)
     
     {
         if(collision.gameObject.name == "Player")
         {
             Destroy(this.transform.parent.gameObject);

             Debug.Log("Stamina Restored!");
             
             gameManager.Items += 1;
         }
     }
 } 
