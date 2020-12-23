using System.Collections.Generic;
using UnityEngine;

public class HexaSphere : MonoBehaviour
{
    public GameObject hexagonPrefab; // Hexagon Prefab made in blender
    public Light columnLights; // Light shining inbetween the columns
    public Light underLights;  // Light shining on the floor from inside of the sphere
    public int size = 8192;    // Amount of hexagon columns     
    public float scale = 600;  // Scale factor for columns
    public float limit = 50;   // Threshold of hexagon's localScale.y to get rid of unnatural extreme peaks
    List<GameObject> hexagons = new List<GameObject>();
    List<Vector3> hexagonsInitPos = new List<Vector3>();
    List<float> hexagonsLastScale = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        float scaling = 14; // Scale of the sphere
        Vector3[] pts = GeneratePoints(size);
    
        for (int j = pts.Length - 1, i = 0; j >= 0; j--, i++)
        {
            hexagons.Add(GameObject.Instantiate(hexagonPrefab));
            hexagons[i].transform.parent = transform;
            hexagons[i].transform.localPosition = pts[j] * scaling;
            hexagons[i].transform.LookAt(transform.position);
            hexagons[i].transform.Rotate(90, 0, 0);
            hexagons[i].transform.localScale /= 4.0f;
            hexagonsInitPos.Add(hexagons[i].transform.localPosition);
            hexagonsLastScale.Add(hexagons[i].transform.localScale.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // For each hexagon, update it's scale based on Audio Spectrum
        for (int i = 0; i < hexagons.Count; i++)
        {
            hexagons[i].transform.localPosition = hexagonsInitPos[i];

            Vector3 lscale = hexagons[i].transform.localScale;

            // If the y scale of the hexagon would exceed the limit, divide it by 3 to lower the peak, else multiply it by 3 to make the movement more interesting.
            if((Mathf.Abs(AudioSampler.spectrum[i]) * scale) > limit) {
                lscale.y = Mathf.Lerp(lscale.y, 1 + (Mathf.Abs(AudioSampler.spectrum[i]) * scale / 3), Time.deltaTime * 3.0f);
            } else {
                lscale.y = Mathf.Lerp(lscale.y, 1 + (Mathf.Abs(AudioSampler.spectrum[i]) * scale * 3), Time.deltaTime * 3.0f);
            }

            // If the previous scale is greater than current scale, decrease the scale smoothly
            if(hexagonsLastScale[i] > lscale.y) {
                lscale.y = Mathf.Lerp(hexagonsLastScale[i], lscale.y, Time.deltaTime * 16.0f);
            }

            hexagonsLastScale[i] = lscale.y;

            // Scale the hexagon around a point instead of from the center. (Didn't get it to work properly).
            ScaleAround(hexagons[i], hexagons[i].transform.localPosition + new Vector3(lscale.x, lscale.y / 2, lscale.z), lscale);
            hexagons[i].transform.localPosition = hexagonsInitPos[i];
        }

        // Make the lights react to the max aplitude and multiply to increase the effect.
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

    // Creation of the sphere points at equal distance from each other.
    Vector3[] GeneratePoints(int n)
    {
        List<Vector3> points = new List<Vector3>();
        float x, y, z, r, phi = 0;
        float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
        float offset = 2.0f / n;

        for (var i = 0; i < n; i++)
        {
            y = i * offset - 1 + (offset / 2);
            phi = i * inc;
            r = Mathf.Sqrt(1 - y * y);
            x = Mathf.Cos(phi) * r;
            z = Mathf.Sin(phi) * r;

            points.Add(new Vector3(x, y, z));
        }
        Vector3[] ptsArray = points.ToArray();

        return ptsArray;
    }
}
