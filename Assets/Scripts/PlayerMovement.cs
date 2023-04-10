
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //references the character controller compontent on the player
    public CharacterController Controller;
    //the speed the player will move at
    public float Speed = 12f;
    
    public Manager manager;

    void Update()
    {
        //checks for inputs
        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");
        //turns the inputs into vectors that the player can move by
        Vector3 MoveZ = transform.forward * Z * Speed * Time.deltaTime;
        Vector3 MoveX = transform.right * X * Speed * Time.deltaTime;
        //move the player depending on the inputs
        Controller.Move(MoveZ);
        Controller.Move(MoveX);

    }



    private void OnTriggerEnter(Collider other)
    {
        print(other.transform.name);
        if (other.transform.CompareTag("Enemy"))
        {
            manager.PlayerCollisionWithEnemy(other.gameObject.GetComponent<Enemy>());
        }
    }




}
