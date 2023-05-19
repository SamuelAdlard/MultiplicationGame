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

    //Records whether the enemy is alive or not
    public bool living = true;

    //The detailed model of the enemy
    public GameObject model;

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
        //plays the character animations and sets the states of the enemy
        SetStates();
    }

    private void SetStates()
    {
       


        if (!living)
        {
            //plays death animation
            animator.SetBool("dead", true);
            //Turns off the text and the clear shell around the enemy
            text.enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            //stops the enemy from moving
            agent.speed = 0;
            //turns the agent off
            agent.enabled = false;
            //turns of collider
            gameObject.GetComponent<Collider>().enabled = false;
            //sets the model to the correct height
            model.transform.localPosition = new Vector3 (0, -1.7f, 0);
            
        }
        else if (!manager.alive)
        {
            //sets the animation to celebrate
            animator.SetBool("Celebrate", true);
            //Turns off the text and the clear shell around the enemy
            text.enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            //stops the enemy from moving
            agent.speed = 0;
        }
        else if (walking)
        {
            //sets the animation state to walk
            animator.SetBool("Walk", true);
            animator.SetBool("Idle", false);
            
        }
        else
        {
            
            //sets the animation state to idle
            animator.SetBool("Idle", true);
            animator.SetBool("Walk", false);
           
        }

        //Makes the enemy walk towards the player
        if (living)
        {
            agent.SetDestination(player.position);
        }
        
    }


    //freezes the enemy
    public IEnumerator Freeze()
    {
        //Sets the enemy state to not walking
        walking = false;
        
        
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
