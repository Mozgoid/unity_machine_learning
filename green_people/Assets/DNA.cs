using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA : MonoBehaviour
{
    public Color color;
    public float scale = 1.0f;
    public float timeToDeath = 0;
    private bool dead = false;

    // Use this for initialization
    void Start ()
    {
        GetComponent<SpriteRenderer>().color = color;
        transform.localScale = transform.localScale * scale;
    }

    // Update is called once per frame
    void Update ()
    {
        
    }

    private void OnMouseDown()
    {
        dead = true;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        timeToDeath = PopulationManager.elapsed;
    }
}
