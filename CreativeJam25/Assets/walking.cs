using Unity.Mathematics;
using UnityEngine;

// hello :D

public class walkingIdkIWillRenameItLater : MonoBehaviour
{
    public Rigidbody2D rigid; // the rigid body attached to the player
    [SerializeField] private int speed; //how fast the plr moves. Feel free to turn it to a float if you want idc. Idk how fast or slow you want the guy to move

    void Update()
    {
        
        if (Input.GetMouseButton(0)) // if LMB is held down
        {
            
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // position of mouse
            var plrPos = transform.position; // position of plr
            mousePos.z = plrPos.z; // make the z's equal because for some fucking reason we need to worry about z in a 2D environment
            var velocityVector = (mousePos - plrPos).normalized; // turns it into a unit vector so movement doesnt change depending on how far mouse is from plr
            rigid.linearVelocity = velocityVector * speed; // sets the velocity, tweak speed in the properties of the script
            
        }
        
        
        

    }
}

