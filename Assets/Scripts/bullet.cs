using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] float TimeToDie;
    public GameObject owner;
    public float dmg=1;
    public bool pierce = false;
    float baseDmg;
    bool basePierce;
    [SerializeField] bool doneWithAnimation = false;
    // Start is called before the first frame update
    void Start()
    {
        baseDmg = dmg;
        basePierce = pierce;
        if(TimeToDie!=0)TimeToDie += Time.time;
        if (doneWithAnimation)
        {
            Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(TimeToDie!=0 && Time.time >= TimeToDie)
        {
            Destroy(gameObject);
        }
    }

}
