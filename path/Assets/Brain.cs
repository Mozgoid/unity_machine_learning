using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class Brain : MonoBehaviour
{
    enum Movement
    {
        forward,
        back,
        left,
        rigth,
        jump,
        crouch
    }
 
    public float timeAlive;
    public float distanceAlive;
    public DNA dna;

    ThirdPersonCharacter character;
    Vector3 move;
    bool alive = true;
    PopulationManager populationManager;
    Vector3 startPos;

    private void OnCollisionEnter(Collision collision)
    {
        if (alive && collision.gameObject.tag == "dead")
        {
            alive = false;
        }
    }
    
    public void Init (int dnaLength, PopulationManager manager)
    {
        populationManager = manager;
        dna = new DNA(dnaLength, Enum.GetValues(typeof(Movement)).Length);
        character = GetComponent<ThirdPersonCharacter>();
        alive = true;
        timeAlive = 0;
        startPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var gene = (Movement)dna.genes[(int)populationManager.elapsed];
        float h = 0;
        float v = 0;
        bool crouch = false;
        bool jump = false;

        switch (gene)
        {
            case Movement.forward:
                v = 1;
                break;
            case Movement.back:
                v = -1;
                break;
            case Movement.left:
                h = -1;
                break;
            case Movement.rigth:
                h = 1;
                break;
            case Movement.jump:
                jump = true;
                break;
            case Movement.crouch:
                crouch = true;
                break;
            default:
                break;
        }

        move = Vector3.forward * v + Vector3.right * h;
        character.Move(move, crouch, jump);
        if (alive)
        {
            timeAlive += Time.deltaTime;
            distanceAlive = (transform.position - startPos).magnitude;
        }
    }
}
