using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spin : MonoBehaviour
{
    private bool spin;
    public float speed;
    private int[] angle = new int[] { 0, 45, 90, 135, 180, 225, 270, 315, 360 };
   
    

    public enum SPIN_DIRECTION
    {
        Clockwise,
        CounterClockwise
    }
    public SPIN_DIRECTION spinDirection;
    // Start is called before the first frame update
    void Start()
    {
        
        spin = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (spin && speed > 0)
        {
           transform.Rotate(0, 0, speed);
            
            speed -= 0.1f;
        }
        if (spin && speed <= 0)//end of speed
        {
            for (int i = 0; i < angle.Length; i++)
            {
                if (transform.rotation.eulerAngles.z == angle[i])
                {
                    transform.Rotate(0, 0, 1);
                    break;
                }
            }
           
           
            speed = 0;
            spin = false;
        }
    }
   
    public void spin_button()
    {
        spin = true;
        speed = Random.Range(20, 50);
        
    }

    
}
