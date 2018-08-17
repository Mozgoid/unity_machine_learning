using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class Brain : MonoBehaviour
{
    enum Movement
    {
        forward,
        left,
        rigth,
    }

    public float moveSpeed = 2;
    public float turnSpeed = 2;
    public float distanceAlive { get; private set; }
    public float maxDistanceFromStart { get; private set; }
    public DNA dna;
    public GameObject eyes;

    public GameObject ethanPrefab;
    GameObject ethan;

    Vector3 startPos;
    bool alive = true;
    PopulationManager populationManager;
    bool seeGround = false;

    private void OnDestroy()
    {
        Destroy(ethan);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (alive && collision.gameObject.tag == "dead")
        {
            alive = false;
            this.GetComponentInChildren<Renderer>().material.color = Color.red;
            //ethan.GetComponentInChildren<Renderer>().material.color = Color.red;
        }
    }
    
    public void Init(int dnaLength)
    {
        dna = new DNA(dnaLength, Enum.GetValues(typeof(Movement)).Length);
        alive = true;
        startPos = transform.position;
        //ethan = Instantiate(ethanPrefab, transform.position, transform.rotation);
        //ethan.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive)
        {
            return;
        }

        seeGround = false;
        RaycastHit hit;

        if (Physics.Raycast(eyes.transform.position, eyes.transform.forward * 10, out hit))
        {
            if (hit.collider.gameObject.tag == "platform")
            {
                seeGround = true;
            }
        }

        Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 10, seeGround ? Color.green : Color.red);

        var gene = (Movement)dna.genes[seeGround ? 0 : 1];
        float h = 0;
        float v = 0;

        switch (gene)
        {
            case Movement.forward:
                v = 1;
                break;
            case Movement.left:
                h = -90;
                break;
            case Movement.rigth:
                h = 90;
                break;
            default:
                break;
        }

        var moveDistance = v * Time.deltaTime * moveSpeed;
        distanceAlive += moveDistance;

        transform.Translate(0, 0, moveDistance);
        transform.Rotate(0, h * Time.deltaTime * turnSpeed, 0);

        maxDistanceFromStart = Math.Max(maxDistanceFromStart, Vector3.Distance(startPos, transform.position));

    }
}
