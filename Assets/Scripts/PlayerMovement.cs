
using System.Runtime.CompilerServices;
using UnityEngine;
//hi sam
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

    }

    private void SetAnimations(float X, float Z)
    {
        if (X != 0 || Z != 0)
        {
            float yAxisRotation;
            yAxisRotation = Mathf.LerpAngle(transform.rotation.eulerAngles.y, Mathf.Atan2(X, Z) * Mathf.Rad2Deg, Time.time);
            model.transform.rotation = Quaternion.Euler(0, yAxisRotation, 0);
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
