using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyFieldSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemys;
    [SerializeField] Tilemap map;
    [SerializeField] float cooldown=5;
    [SerializeField] int map_width = 31;
    [SerializeField] int map_height = 22;
    float timestap;
    // Start is called before the first frame update
    void Start()
    {
        timestap = Time.time+ cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if(timestap <= Time.time)
        {
            int enemyIndex = Random.Range(0, enemys.Length - 1);
            GameObject new_enemy = Instantiate(enemys[enemyIndex]);
            new_enemy.transform.position = new Vector3(Random.Range(1, map_width), Random.Range(1, map_height));
            timestap = Time.time + cooldown;
        }
    }
}
