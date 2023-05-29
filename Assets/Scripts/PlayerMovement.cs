
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //references the character controller compontent on the player
    public CharacterController Controller;
    //the speed the player will move at
    public float Speed = 12f;
    //animation component
    public Animator animator;
    //The game manager script reference
    public Manager manager;
    //The detailed model
    public GameObject model;
    //To check if the player is firing the weapon
    public bool usingWeapon = false;
    //Stores the interpolation time in order to lerp smoothly between directions
    float interpolationTime = 0;
    //Audiosource for the feet of the player
    public AudioSource walkSound;
    


    void Update()
    {
        //checks for inputs
        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");
        //turns the inputs into vectors that the player can move by
        Vector3 MoveZ = Vector3.forward * Z * Speed * Time.deltaTime;
        Vector3 MoveX = Vector3.right * X * Speed * Time.deltaTime;
        //move the player depending on the inputs
        Controller.Move(MoveZ);
        Controller.Move(MoveX);

        SetAnimations(X, Z);
        //Adds variation to the sound
        walkSound.volume = Mathf.Clamp((Mathf.Abs(X) + Mathf.Abs(Z)) / 2, 0, 0.15f);
        
    }

    //sets the correct animation for the player model
    private void SetAnimations(float X, float Z)
    {
        if (manager.usingWeapon)
        {
            //plays the firing weapon animation
            animator.SetBool("Running", false);
            animator.SetBool("Idle", false);
            animator.SetBool("Shoot", true);
            //stops the walking sound
            walkSound.enabled = false;
        }
        else if (X != 0 || Z != 0) //checks if the player is moving
        {
            //variable to store the target rotation of the player
            float yAxisRotation;
            //allows the change in angle to be smooth
            yAxisRotation = Mathf.LerpAngle(transform.rotation.eulerAngles.y, Mathf.Atan2(X, Z) * Mathf.Rad2Deg, interpolationTime);
            //Adds time to the interpolation time
            interpolationTime += 0.1f;
            //sets the rotation
            model.transform.rotation = Quaternion.Euler(0, yAxisRotation, 0);
            //plays the running animation
            animator.SetBool("Running", true);
            animator.SetBool("Idle", false);
            animator.SetBool("Shoot", false);
            //starts the walking sound
            walkSound.enabled = true;
            
        }
        else
        {
            //sets the interpolation time back to 0
            interpolationTime = 0;
            //play the idle animation
            animator.SetBool("Idle", true);
            animator.SetBool("Running", false);
            animator.SetBool("Shoot", false);
            //stops the walking sound
            walkSound.enabled = false;
            
        }
        
        
    }

    //detects player collision
    private void OnTriggerEnter(Collider other)
    {
        //checks if the collision is with any enemy
        if (other.transform.CompareTag("Enemy"))
        {
            //tells the manager there was a collision
            manager.PlayerCollisionWithEnemy(other.gameObject.GetComponent<Enemy>());
        }
    }




}
