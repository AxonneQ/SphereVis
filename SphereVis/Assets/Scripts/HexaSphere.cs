using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaSphere : MonoBehaviour
{
    public GameObject hexagonPrefab;
    public int size = 60;
    public int subdivisions = 4;

    List<GameObject> hexagons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        float scaling = 16;
        Vector3[] pts = PointsOnSphere(1024);
        int i = 0;
        
        foreach (Vector3 value in pts)
        {
            hexagons.Add(GameObject.Instantiate(hexagonPrefab));
            hexagons[i].transform.parent = transform;
            hexagons[i].transform.position = value * scaling;
            hexagons[i].transform.LookAt(transform.position);
            hexagons[i].transform.Rotate( 90, 0, 0 ) ;
            hexagons[i].transform.localScale /= 1.3f;
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject hex in hexagons) 
        {
            hex.transform.localScale += new Vector3(0, 0.1f, 0);
        }
    }

    Vector3[] PointsOnSphere(int n)
        {
            List<Vector3> upts = new List<Vector3>();
            float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
            float off = 2.0f / n;
            float x = 0;
            float y = 0;
            float z = 0;
            float r = 0;
            float phi = 0;
           
            for (var k = 0; k < n; k++){
                y = k * off - 1 + (off /2);
                r = Mathf.Sqrt(1 - y * y);
                phi = k * inc;
                x = Mathf.Cos(phi) * r;
                z = Mathf.Sin(phi) * r;
               
                upts.Add(new Vector3(x, y, z));
            }
            Vector3[] pts = upts.ToArray();
            return pts;
        }

}
