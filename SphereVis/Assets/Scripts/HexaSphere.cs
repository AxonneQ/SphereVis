using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaSphere : MonoBehaviour
{
    public GameObject hexagonPrefab;
    public Light columnLights;
    public Light underLights;
    public int size = 1024;
    public float scale = 1000;
    public float limit = 400;
    List<GameObject> hexagons = new List<GameObject>();
    List<Vector3> hexagonsInitPos = new List<Vector3>();
    List<float> hexagonsLastScale = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        float scaling = 14;
        Vector3[] pts = PointsOnSphere(size);
        int i = 0;

        for (int j = pts.Length - 1; j >= 0; j--)
        {
            hexagons.Add(GameObject.Instantiate(hexagonPrefab));
            hexagons[i].transform.parent = transform;
            hexagons[i].transform.localPosition = pts[j] * scaling;
            hexagons[i].transform.LookAt(transform.position);
            hexagons[i].transform.Rotate(90, 0, 0);
            hexagons[i].transform.localScale /= 1.3f;
            hexagonsInitPos.Add(hexagons[i].transform.localPosition);
            hexagonsLastScale.Add(hexagons[i].transform.localScale.y);
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
            if(hexagonsLastScale[i] > lscale.y) {
                lscale.y = Mathf.Lerp(hexagonsLastScale[i], lscale.y, Time.deltaTime * 0.5f);
            } else {
                if((Mathf.Abs(AudioSampler.spectrum[i]) * scale) > limit) {
                    lscale.y = Mathf.Lerp(lscale.y, 1 + (Mathf.Abs(AudioSampler.spectrum[i]) * scale / 3), Time.deltaTime * 3.0f);
                } else {
                    lscale.y = Mathf.Lerp(lscale.y, 1 + (Mathf.Abs(AudioSampler.spectrum[i]) * scale * 3), Time.deltaTime * 3.0f);
                }
            }
            
            ScaleAround(hexagons[i], hexagons[i].transform.localPosition + new Vector3(lscale.x, lscale.y / 2, lscale.z), lscale);
            hexagons[i].transform.localPosition = hexagonsInitPos[i];
        }
        underLights.intensity = Mathf.Lerp(underLights.intensity, AudioSampler.amplitude * 20, 0.05f);
        columnLights.intensity = Mathf.Lerp(columnLights.intensity, AudioSampler.amplitude * 150, Time.deltaTime * 3.0f);
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

        target.transform.localScale = newScale;
    }

    Vector3[] PointsOnSphere(int n)
    {
        List<Vector3> upts = new List<Vector3>();
        float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
        float off = 2.0f / n;
        float x, y, z, r, phi = 0;

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
