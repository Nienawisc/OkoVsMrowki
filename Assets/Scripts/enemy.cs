using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    [SerializeField] int points;
    [SerializeField] float hp;
    float maxHp;
    [SerializeField] float speed=4;
    public int rare = 1;
    bool spawner = false;
    [SerializeField] GameObject[] spawnObject;
    [SerializeField] float spawnCooldown=3;
    float timestap;
    [SerializeField] GameObject NearPlayer;
    [SerializeField] GameObject hpBar;
    private float startHpBarX;
    // Start is called before the first frame update
    void Start()
    {
        if(hpBar!=null)startHpBarX = hpBar.transform.localScale.x;
        maxHp = hp;
        timestap = Time.time + spawnCooldown;
        NearPlayer = GameObject.FindGameObjectWithTag("Player");
        if (spawnObject.Length > 0) spawner = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(speed>0)movement();
        spawn();
    }
    void movement()
    {
        transform.position = Vector2.MoveTowards(transform.position, NearPlayer.transform.position, speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            takeDmg(collision.gameObject.GetComponent<bullet>().dmg, collision.gameObject.GetComponent<bullet>().owner);
            if (!collision.gameObject.GetComponent<bullet>().pierce)
            {
                Destroy(collision.gameObject);
            }
        }
        if (!spawner)
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<Player>().deadset(true);
            }
    }
    private void spawn()
    {
        if (spawner && timestap <= Time.time)
        {
            int sum = 0;
            foreach(var mrowka in spawnObject)
            {
                sum += mrowka.GetComponent<enemy>().rare;
            }
            int AntRand = Random.Range(1, sum+1);
            Debug.Log(AntRand);
            foreach (var mrowka in spawnObject)
            {
                AntRand -= mrowka.GetComponent<enemy>().rare;
                if(AntRand<=0)
                {
                    GameObject m = Instantiate(mrowka);
                    m.transform.position = transform.position;
                    break;
                }
            }
            timestap = Time.time + spawnCooldown;
        }
    }
    private void takeDmg(float dmg, GameObject player)
    {
        hp -= dmg;
        hp_bar();
        if (hp <= 0)
        {
            player.GetComponent<Player>().increasePoints(points);
            Destroy(gameObject);
        }
    }
    private void hp_bar()
    {
        if(hpBar != null)
        {
            Vector3 temp = hpBar.transform.localScale;
            float value = (hp/maxHp)*startHpBarX;
            temp.x = value;
            hpBar.transform.localScale = temp;
        }
    }
}
