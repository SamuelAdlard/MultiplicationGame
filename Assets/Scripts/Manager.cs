using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;
using Unity.VisualScripting;

public class Manager : MonoBehaviour
{
    //player object
    public GameObject player;
    
    //reference to the player's power
    public int playerAnswer = 10;

    //player display text
    public TextMeshPro playerText;

    //player materials
    public Material normalMaterial;
    public Material gracePeriodMaterial;

    //player's health
    public int health = 3;

    //if the player is alive
    public bool alive = true;

    //Number of kills the player has
    public int kills = 0;

    //Number of mistakes the player has made
    public int mistakes = 0;

    //Current wave level
    public int currentWave = 0;

    //Enemy prefab
    public GameObject enemyPrefab;
    
    //List of enemies
    public List<Enemy> enemies = new List<Enemy>();

    //grace period
    public bool gracePeriod = false;

    //Display texts main UI
    public TextMeshProUGUI waveCounter, killCounter, mistakeCounter, healthCounter, waveCountDown, chargeDisplay, weaponFireInstructionText;

    //Different UI sections
    public GameObject mainUI, endUI, startUI;

    //end UI text objects
    public TextMeshProUGUI endScore, endKills, endWave, endMistakes;

    //start UI variables
    public TextMeshProUGUI startButtonText;

    //input fields for range and times tables
    public TMP_InputField startRange, startTimesTables;

    //The gameobject holding the not random section of the UI
    public GameObject startNotRandomUI;

    //variable that keeps track of whether the player wants random questions 
    public bool random = true;

    //Variables to determine the questions presented
    int range = 12;

    //list of times tables the player wants to practice
    public List<int> timesTables = new List<int>();

    //Enemy death Particles
    public ParticleSystem deathParticles, magicParticles, attackParticles, explosionParticles;

    //Tells the player movement script if the player is firing the weapon
    public bool usingWeapon = false;

    //The charge of the long ranged weapon
    public int weaponCharge = 0;

    //Camera reference
    public Camera playerCamera;
    
    private void Start()
    {
        
    }


    void Update()
    {
        //Checks to see if the weapon can be fired every frame
        CheckWeaponTarget();
    }




    private void StartGameplay()
    {
        //clears the countdown text
        waveCountDown.text = "";
        
        //Goes to the next wave
        currentWave++;

        //updates display text
        waveCounter.text = $"Wave: {currentWave}";

        //Set health
        health = 3;
        //display health
        healthCounter.text = $"Health: {health}";

        
        
        //Creates the enemies for that round the number of enemies is equal to 1 + wave^2
        for (int i = 0; i < (1 + currentWave * 2); i++)
        {
            //Creates new enemy
            Enemy newEnemy = Instantiate(enemyPrefab, new Vector3(20, 0.5f, Random.Range(-10, 10)), Quaternion.identity).GetComponent<Enemy>();
            
            
            //Gets random numbers for the question
            newEnemy.number1 = QuestionNumber1();
            newEnemy.number2 = QuestionNumber2();
            //changes the text on top of the enemy to match the question
            newEnemy.Spawn();
            enemies.Add(newEnemy);
        }

        playerAnswer = GetPlayerQuestion();
    }

    //Generates the first number in the question
    private int QuestionNumber1()
    {
        
        
        //checks if the player wants times tables that are random or chosen
        if (random)
        {
            return Random.Range(0, range);
        }
        else
        {
            return timesTables[Random.Range(0, timesTables.Count)];
        }
         

    }

    //Generates the second number in the question
    private int QuestionNumber2()
    {
        
        return Random.Range(0, range);
    }


    //Function called when player object collides with an enemy
    public void PlayerCollisionWithEnemy(Enemy enemy)
    {
        //Checks if the player's power is equal to the enemy's and if there is a grace period
        if(playerAnswer == enemy.number1 * enemy.number2 && !gracePeriod && alive)
        {
            AnswerCorrect(enemy);
        }
        else if(!gracePeriod && alive)
        {
            AnswerIncorrect(enemy);
        }
    }

    //Called when the answer is correct
    private void AnswerCorrect(Enemy enemy)
    {
        //Removes the enemy from the list
        enemies.Remove(enemy);
        //kills the enemy
        enemy.living = false;
        //Shows enemy death particles
        Instantiate(deathParticles, enemy.transform.position, Quaternion.identity);
        //Records the kill
        kills++;
        //sets the display text
        killCounter.text = $"Kills: {kills}";
        //makes sure there are still enemies left
        if (enemies.Count > 0)
        {
            //charges the ranged weapon for the player
            ChargeWeapon();
            //sets the question for the player, and sets the text to the questiom as well as finding the answer to the question.
            //Which is show below
            playerAnswer = GetPlayerQuestion();
        }
        else
        {
            //Charges the ranged weapon
            ChargeWeapon();
            //starts the countdown for the next wave
            StartCoroutine(CountDown(10));
        }
    }

    //Counts down to the next wave while setting the text to the number of seconds the player has left
    private IEnumerator CountDown(int countDownTime)
    {
        //goes backwards from the countdowntime to zero
        for (int i = countDownTime; i > 0; i--)
        {
            //sets the text to the amount of time left
            waveCountDown.text = i.ToString();
            //waits for a second
            yield return new WaitForSeconds(1);
        }

        //Starts next wave
        StartGameplay();
    }


    //Called when the answer isn't correct
    private void AnswerIncorrect(Enemy enemy)
    {
        //records the mistake
        mistakes++;
        //sets display text
        mistakeCounter.text = $"Mistakes: {mistakes}";
        //takes health away
        health--;
        //checks if player is still alive
        if (health <= 0)
        {
            alive = false;
            EndGame();
        }


        //sets display text
        healthCounter.text = $"Health: {health}";

        //freezes the enemy
        enemy.StartCoroutine(enemy.Freeze());
    }

    //Ends the game and brings up the total score of the player
    private void EndGame()
    {
        //open menu
        endUI.SetActive(true);
        //close old menu
        mainUI.SetActive(false);
        //sets the number of waves survived
        endWave.text = $"Waves Survived: {currentWave - 1}";
        //shows the number of kills
        endKills.text = $"Kills: {kills}";
        //shows the total number of mistakes
        endMistakes.text = $"Mistakes: {mistakes}";
        //shows the total score
        endScore.text = $"Total Score: {kills + currentWave - mistakes}";
        //Turns the player movement off
        player.GetComponent<PlayerMovement>().enabled = false;
        //Plays player death animation
        player.GetComponent<PlayerMovement>().model.GetComponent<Animator>().SetBool("Die", true);
        //sets player model to the correct heigh for the animation
        player.GetComponent<PlayerMovement>().model.transform.localPosition = new Vector3(0, -3.11f, 0);
    }


    //Function to get new player power 
    private int GetPlayerQuestion()
    {
        //finds the index of a random enemy
        int randomEnemy = Random.Range(0, enemies.Count);
        string question = $"{enemies[randomEnemy].number1} × {enemies[randomEnemy].number2}";
        //Sets the display text to the correct number
        playerText.text = question.ToString();
        //selects a new player power
        return enemies[randomEnemy].number1 * enemies[randomEnemy].number2;
    }

    //Starts the grace period that allows the player to move away from the enemies
    private void StartGracePeriod()
    {
        gracePeriod = true;

        //gets the player movement script
        PlayerMovement movementScript = player.GetComponent<PlayerMovement>();
        //gets the player collider
        Collider collider = player.GetComponent<Collider>();
        //gets player speed
        float speed = movementScript.Speed;
        //sets new player speed
        movementScript.Speed = speed + 1;
        //turns off collider
        collider.enabled = false;
        //sets new material
        player.GetComponent<MeshRenderer>().material = gracePeriodMaterial;
        //starts timer to end the grace period
        Invoke("EndGracePeriod", 2.5f);
    }

    //Charges the long ranged weapon
    private void ChargeWeapon()
    {
        //makes sure the weapon charge doesn't go above 3
        if (weaponCharge < 3)
        {
            //turns off the instruction to fire the weapon
            weaponFireInstructionText.gameObject.SetActive(false);
            //adds one to the charge
            weaponCharge++;
            //updates the text
            chargeDisplay.text = $"Charge: {weaponCharge}/3";
            //if the weapon charge is equal to 3 after it has been charged the instruction text will be shown
            if (weaponCharge == 3)
            {
                //turns on the instruction text
                weaponFireInstructionText.gameObject.SetActive(true);
            }
        }
        else
        {
            //turns on the instruction to fire the weapon
            weaponFireInstructionText.gameObject.SetActive(true);
        }
        
    }

    //Checks the if the player can fire the weapon
    private void CheckWeaponTarget()
    {
        //Creates a ray based of the player mouse position
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        //Stores the hit information of the ray
        RaycastHit hit;
        //Checks to see if the player has enough charge to fire the weapon, is pressing 'e' and whether the ray will actually hit an object
        if (weaponCharge > 2 && Input.GetKeyDown("e") && Physics.Raycast(ray, out hit) && alive) 
        {
            //Gets the enemy component from the object the ray is hitting
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            //Makes sure the object is actually and enemy
            if (enemy != null)
            {
                //Sets the weapon charge to 0
                weaponCharge = 0;
                //Sets the display text to the value of the charge
                chargeDisplay.text = $"Charge: {weaponCharge}/3";
                //Turns of the fire instruction text
                weaponFireInstructionText.gameObject.SetActive(false);
                //Fires the weapon and plays the animation
                StartCoroutine(FireWeapon(enemy));
            }
        }
    }

    
    private IEnumerator FireWeapon(Enemy enemy)
    {
        //sets the usingWeapon variable to true
        usingWeapon = true;
        //Starts particle effect
        magicParticles.playbackSpeed = 1;
        magicParticles.Play();
        //stops the player from moving
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        //makes the player look at the enemy
        playerMovement.model.transform.LookAt(enemy.transform.position);
        //original speed of player recorded
        float initialSpeed = playerMovement.Speed;
        //sets the player speed to 0
        playerMovement.Speed = 0;
        //Plays the attack particles
        attackParticles.Play();
        //Waits for 2 seconds
        yield return new WaitForSeconds(1.5f);
        
        //makes the particles faster
        magicParticles.playbackSpeed = 8;
        yield return new WaitForSeconds(0.5f);
        //Plays the explosion particles
        Instantiate(explosionParticles, enemy.transform.position, Quaternion.Euler(0,0,90));
        //sets the player speed back to normal
        playerMovement.Speed = initialSpeed;
        //Stops particle effect
        attackParticles.Stop();
        attackParticles.Clear();
        magicParticles.Stop();
        //sets the usingWeapon variable to false
        usingWeapon = false;
        //Calls the function that checks if answers are correct
        PlayerCollisionWithEnemy(enemy);
        print("fired");
    }

    //ends the grave period
    private void EndGracePeriod()
    {
        gracePeriod = false;
        
        //gets player movement
        PlayerMovement movementScript = player.GetComponent<PlayerMovement>();
        //gets collider
        Collider collider = player.GetComponent<Collider>();
        //gets speed
        float speed = movementScript.Speed;
        //sets new speed
        movementScript.Speed = speed - 1;
        //turns collider on
        collider.enabled = true;
        //sets material
        player.GetComponent<MeshRenderer>().material = normalMaterial;
    }

    //Handles the player pressing the checkbox button
    public void CheckBoxTicked()
    {
        //checks if the player has already selected the button
        if (!random)
        {
            //turns the not random UI elements off
            startNotRandomUI.SetActive(false);
            //sets random variable to true
            random = true;
            //puts the X on the check box
            startButtonText.text = "X";
        }
        else
        {
            //puts the not random UI section on
            startNotRandomUI.SetActive(true);
            //sets random to false
            random = false;
            //removes X from checkbox
            startButtonText.text = "";
        }
    }

    //Starts the game when the button in the start UI menu is pressed
    public void BeginGame()
    {
        //gets the range from the range input box
        range = int.Parse(startRange.text);
        //gets the times tables entered in the input field and puts them in the times tables list
        GetTimesTables();
        //allows the player to move
        player.GetComponent<PlayerMovement>().enabled = true;
        //turns off start UI
        startUI.SetActive(false);
        //turns on main UI
        mainUI.SetActive(true);
        //properly starts the game
        StartGameplay();
    }

    //function for splitting up the input field meant for the times tables
    private void GetTimesTables()
    {
        //checks if the player wants random or non random questions
        if (!random)
        {
            //saves the player input in a string
            string timesTablesString = startTimesTables.text;
            //creates a list to store the number characters in the string
            List<char> currentNumber = new List<char>();
            //loops through all the characters in the string
            for (int i = 0; i < timesTablesString.Length; i++)
            {
                //checks to see if there is a colon
                if (timesTablesString[i] == ',')
                {
                    //adds the number to the times tables list
                    timesTables.Add(int.Parse(currentNumber.ToArray()));
                    //clears the current number list so it can be filled with new characters
                    currentNumber.Clear();
                }
                else
                {
                    //if there isn't a colon the character is added to the current number list
                    currentNumber.Add(timesTablesString[i]);
                }

                //checks if it is at the end of the array
                if (i == timesTablesString.Length - 1)
                {
                    //adds the final number
                    timesTables.Add(int.Parse(currentNumber.ToArray()));
                }

            }
        }
    }
    //takes player to main menu
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}



