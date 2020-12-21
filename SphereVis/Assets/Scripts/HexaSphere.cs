using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaSphere : MonoBehaviour
{
    public GameObject hexagonPrefab;
    public int size = 1024;
    public float scale = 10000;
    List<GameObject> hexagons = new List<GameObject>();
    List<Vector3> hexagonsInitPos = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        float scaling = 16;
        Vector3[] pts = PointsOnSphere(size);
        int i = 0;
        
        for (int j = pts.Length - 1; j >= 0; j--)
        {
            Debug.Log(pts.Length);

            hexagons.Add(GameObject.Instantiate(hexagonPrefab));
            hexagons[i].transform.parent = transform;
            hexagons[i].transform.position = pts[j] * scaling;
            hexagons[i].transform.LookAt(transform.position);
            hexagons[i].transform.Rotate( 90, 0, 0 ) ;
            hexagons[i].transform.localScale /= 1.3f;
            hexagonsInitPos.Add(hexagons[i].transform.position);
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // foreach(GameObject hex in hexagons) 
        // {
        //     hex.transform.localScale += new Vector3(0, 0.1f, 0);
        // }
        for (int i = 0; i < hexagons.Count; i++) {
            // Vector3 ls = hexagons[i].transform.localScale;
            // ls.y = Mathf.Lerp(ls.y, 1 + (AudioSampler.bands[i] * scale), Time.deltaTime * 3.0f);
            // hexagons[i].transform.localScale = ls;
            // Vector3 pos = hexagons[i].transform.position;
            // pos.y = hexagonsInitPos[i].y + (ls.y / 2);
            // hexagons[i].transform.position = pos;
            Vector3 lscale = hexagons[i].transform.localScale; 
            lscale.y = Mathf.Lerp(lscale.y, 1 + (Mathf.Abs(AudioSampler.spectrum[i]) * scale), Time.deltaTime * 3.0f);
            hexagons[i].transform.localScale = lscale;

            Vector3 pos = hexagons[i].transform.position;
            pos.y = hexagonsInitPos[i].y + (lscale.y / 2);
            hexagons[i].transform.position = pos;

            hexagons[i].transform.LookAt(transform.position);
            hexagons[i].transform.Rotate( 90, 0, 0 ) ;
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
