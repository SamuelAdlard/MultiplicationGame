using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Manager : MonoBehaviour
{
    
    //reference to the player's power
    public int playerPower = 10;

    //player display text
    public TextMeshPro playerText; 

    //player's health
    public int health = 3;

    //Number of kills the player has
    public int kills = 0;

    //Number of mistakes the player has made
    public int mistakes = 0;

    //Current wave level
    public int currentWave = 1;

    //Enemy prefab
    public GameObject enemyPrefab;
    
    //List of enemies
    public List<Enemy> enemies = new List<Enemy>();

    private void Start()
    {
        StartGame();
    }


    void Update()
    {
        
    }

    private void StartGame()
    {
        //Creates the enemies for that round the number of enemies is equal to 4 + wave^2
        for (int i = 0; i < (4 + Mathf.Pow(currentWave, 2)); i++)
        {
            //Creates new enemy
            Enemy newEnemy = Instantiate(enemyPrefab, new Vector3(Random.Range(-5, 5), 0.5f, Random.Range(-5, 5)), Quaternion.identity).GetComponent<Enemy>();
            
            
            //Gets random numbers for the question
            newEnemy.number1 = QuestionNumber1();
            newEnemy.number2 = QuestionNumber2();
            //changes the text on top of the enemy to match the question
            newEnemy.Spawn();
            enemies.Add(newEnemy);
        }

        playerPower = GetPlayerPower();
    }

    //Generates the first number in the question
    private int QuestionNumber1()
    {
        return Random.Range(0, 12);
    }

    //Generates the second number in the question
    private int QuestionNumber2()
    {
        return Random.Range(0, 12);
    }


    //Function called when player object collides with an enemy
    public void PlayerCollisionWithEnemy(Enemy enemy)
    {
        //Checks if the player's power is equal to the enemy's
        if(playerPower == enemy.number1 * enemy.number2)
        {
            //Removes the enemy from the list
            enemies.Remove(enemy);
            //Destroys the enemy
            Destroy(enemy.gameObject);
            //Records the kill
            kills++;
            //makes sure there are still enemies left
            if (enemies.Count > 0)
            {
                playerPower = GetPlayerPower();
            }

        }
        else
        {
            //records the mistake
            mistakes++;

            //takes health away
            health--;
        }
    }

    //Function to get new player power 
    private int GetPlayerPower()
    {
        int randomEnemy = Random.Range(0, enemies.Count);
        int newPower = enemies[randomEnemy].number1 * enemies[randomEnemy].number2;
        //Sets the display text to the correct number
        playerText.text = (newPower).ToString();
        //selects a new player power
        return enemies[randomEnemy].number1 * enemies[randomEnemy].number2;
    }

}



