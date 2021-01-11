using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxRespawn : MonoBehaviour
{
    private Vector2 checkPoint; 
    private float maxFall, xAx, yAx;
    // Start is called before the first frame update
    void Start()
    {
        checkPoint = transform.position;
        xAx = checkPoint.x;
        yAx = checkPoint.y;
        maxFall = checkPoint.y - 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < maxFall) {
            transform.position = new Vector2 (xAx,yAx + 2);
        }
    }

    
}
