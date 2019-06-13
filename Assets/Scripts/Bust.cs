using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bust : MonoBehaviour
{
    public float increasedSpeed = 0;
    public float increasedDmg = 0;
    public float newCooldown = 0;//in seconds
    public float time = 0;
    public bool piercing;
    public int addBomb = 0;
    [SerializeField] float LiveTime = 60;
    float timestap = 0;
    // Start is called before the first frame update
    void Start()
    {
        timestap = Time.time+LiveTime;
    }

    // Update is called once per frame
    void Update()
    {
       if(Time.time>=timestap)
        {
            Destroy(gameObject);
        }
    }
}
