using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowDotEffect : MonoBehaviour
{
    public float tickDamage;
    
    private Enemy parent;
    private float tickDelay = 0.3f;
    private float timeSinceTick = 0;
    private int tickTotal = 3;
    private int ticksDone = 0;

    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform.parent.gameObject.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceTick += Time.deltaTime;

        if (timeSinceTick > tickDelay)
        {
            parent.takeDamage(tickDamage);
            timeSinceTick = 0;
            ticksDone += 1;

            if (ticksDone == tickTotal)
            {
                Destroy(gameObject);
            }
        }
    }
}
