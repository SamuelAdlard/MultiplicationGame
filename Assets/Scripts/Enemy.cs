using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    //first number in the multiplication problem
    public int number1;
    //second number
    public int number2;
    //total value
    public int answer;
    //Dislay text for the answer
    public TextMeshPro text;
    //Navmesh agent 
    public NavMeshAgent agent;

    //enemy freeze materials
    public Material normalEnemy;
    public Material frozenEnemy;

    //references the manager script
    public Manager manager;

    //player transform variable
    Transform player;

    //records the original speed
    float moveSpeed;

    //animation component
    public Animator animator;

    //records the state of the enemy to see if it is walking or not
    bool walking = true;

    

    // Start is called before the first frame update
    private void Start()
    {
        //Gets the manager script
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        
        //gets player object
        player = GameObject.Find("Player").transform;

        //records speed
        moveSpeed = agent.speed;

        //Gives each enemy a random animation speed
        animator.speed = Random.Range(0.5f, 1);
    }



    //called just after the enemy is instantiated and all the values have been set
    public void Spawn()
    {
        //finds answer
        answer = number1 * number2;
        //sets the text to the answer
        text.text = $"{answer}";

    }

    // Update is called once per frames
    void Update()
    {
        //sets the rotation to always align with the camera
        text.transform.rotation = Quaternion.Euler(90, 0, 0);
        //plays the character animations
        if (!manager.alive)
        {
            animator.SetBool("Celebrate", true);
        }
        else if(walking)
        {
            animator.SetBool("Walk", true);
            
        }
        else
        {
            print("idle");
            animator.SetBool("Idle", true);
        }
        
        //Makes the enemy walk towards the player
        agent.SetDestination(player.position);
    }

    //freezes the enemy
    public IEnumerator Freeze()
    {
        //Sets the enemy state to not walking
        walking = false;
        
        print(moveSpeed);
        //stops the agent from moving
        agent.speed = 0;
        //sets the frozen material
        gameObject.GetComponent<Renderer>().material = frozenEnemy;
        //Turns off collider
        gameObject.GetComponent<Collider>().enabled = false;
        //waits for 2 seconds
        yield return new WaitForSeconds(2);
        //resets speed
        agent.speed = moveSpeed;
        //resets the material
        gameObject.GetComponent<Renderer>().material = normalEnemy;
        //Turns on collider
        gameObject.GetComponent<Collider>().enabled = true;

        //Sets the enemy state to walking
        walking = true;
    }

    public void OnDestroy()
    {
        
        
    }

}
