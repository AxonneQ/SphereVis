using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaSphere : MonoBehaviour
{
    public GameObject hexagonPrefab;
    public int size = 1024;
    public float scale = 1000;
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
            hexagons[i].transform.Rotate(90, 0, 0);
            hexagons[i].transform.localScale /= 1.3f;
            hexagonsInitPos.Add(hexagons[i].transform.position);
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hexagons.Count; i++)
        {
            hexagons[i].transform.localPosition = hexagonsInitPos[i];
            Vector3 lscale = hexagons[i].transform.localScale;
            lscale.y = Mathf.Lerp(lscale.y, 1 + (Mathf.Abs(AudioSampler.spectrum[i]) * scale), Time.deltaTime * 3.0f);
            ScaleAround(hexagons[i], hexagons[i].transform.localPosition + new Vector3(lscale.x, lscale.y / 2, lscale.z), lscale);
            hexagons[i].transform.position = hexagonsInitPos[i];
        }
    }

    public void ScaleAround(GameObject target, Vector3 pivot, Vector3 newScale)
    {
        Vector3 pivotDelta = target.transform.localPosition - pivot;
        Vector3 scaleFactor = new Vector3(
            newScale.x / target.transform.localScale.x,
            newScale.y / target.transform.localScale.y,
            newScale.z / target.transform.localScale.z );
        pivotDelta.Scale(scaleFactor);
        target.transform.position = pivot + pivotDelta;
    
        //scale
        target.transform.localScale = newScale;
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

        for (var k = 0; k < n; k++)
        {
            y = k * off - 1 + (off / 2);
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
