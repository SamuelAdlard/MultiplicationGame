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

    //player transform variable
    Transform player;

    //records the original speed
    float moveSpeed;

    

    // Start is called before the first frame update
    private void Start()
    {
        //gets player object
        player = GameObject.Find("Player").transform;

        //records speed
        moveSpeed = agent.speed;
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
        text.transform.rotation = Quaternion.Euler(90, 0, 0);
        agent.SetDestination(player.position);
    }

    //freezes the enemy
    public IEnumerator Freeze()
    {
        
        
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
    }

    public void OnDestroy()
    {
        
        
    }

}
