using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoostFieldSpawn : MonoBehaviour
{
    [SerializeField] GameObject[] boosts;
    [SerializeField] Tilemap map;
    [SerializeField] float cooldown = 15;
    [SerializeField] int map_width = 31;
    [SerializeField] int map_height = 22;
    float timestap;
    // Start is called before the first frame update
    void Start()
    {
        timestap = Time.time + cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (timestap <= Time.time)
        {
            int boostIndex = Random.Range(0, boosts.Length);
            GameObject new_boost = Instantiate(boosts[boostIndex]);
            new_boost.transform.position = new Vector3(Random.Range(1, map_width), Random.Range(1, map_height));
            timestap = Time.time + cooldown;
        }
    }
}
