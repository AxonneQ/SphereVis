# SphereVis

## Student Info:
**Student Name**: Igor Bolek

**Student Number**: C17487376

**Course**: DT228/4  **|**  TU856/4

# Description of the project

SphereVis is an audio visualizer that takes a form of a sphere made out of 8192 hexagon models.
As the audio track plays, the hexagon's Y scale increases representing the audio spectrum.
There is a fog around the sphere and lights inside the sphere to create a nice planet eruption visual effect.

Image of SphereVis in Action:

![SphereVis in Action](images/demo.gif)

Video Demonstration using song "Deviant" by DEADLIFE:

[![Video demo of SphereVis](https://img.youtube.com/vi/Bv5cSYdBws8/0.jpg)](https://www.youtube.com/watch?v=Bv5cSYdBws8)

# Instructions for use

1. When Cloned/Downloaded from this repository, open the `SphereVis` folder in Unity as a project.
2. Open the `SphereVisScene` scene and run.
3. 8192 is a high number for computers without a dedicated GPU, In the Unity player that was around 30 fps, but when built and ran, I got stable 60+ fps (clamped by vsync)
4. To reduce the size, adjust the `frameSize` in AudioSampler and `size` HexaSphere GameObjects. They must be the same value.
5. To increase/decrease size of the hexagons in x and z axis, Increase the divisor on line 29 in HexaSphere.cs script. Default is `4.0f`
6. Lastly, to change a song, Drag it from Music directory to `Track` field in AudioSampler GameObject.

# How it works

Most of the code is commented, but the main algorithm is basically:
**Creation of the Sphere and Hexagons:**
1. Generate Points on the Sphere. using the following function:
```C#
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
```
2. Using the returned vectors, We create and instantiate the Hexagon GameObjects which are prefabs containing a Hexagon FBX model made in Blender.
3. Assign the HexaSphere GameObject `transform` as `transform.parent` of each of the hexagons.
4. Change the LOCAL position of each hexagon to the corresponding Vector position obtained from the returned Vectors.
5. Use LookAt function to look at the center of the sphere.
6. Lastly "record" each initial position and scale for each hexagon.

**Update Function:**
For each hexagon do the following:
1. Reset the localposition to initial.
2. If the expected scale would exceed the limit, divide the result by 3, else multiply by 3 to increase the effect.
3. If the new localScale.y value is less than previous one, smoothe out the transition (like audio equilizer bars go rapidly up but fall slowly)
4. Update the y scale of the hexagon and set previous scale to the same value.
5. Update light intensity based on the amplitude and scale it up for a more visible effect.

# References
References that inspired me to create this project:

[WebGL Globe](https://experiments.withgoogle.com/chrome/globe)

[Loop WaveForm](https://www.uberviz.io/viz/loop/)

[HexaSphere Thread](https://forum.unity.com/threads/make-procedural-hexagons-on-a-sphere-is-there-a-tool-voxels-perhaps.330907/)
# What I am most proud of in the assignment
I am most proud of the fact that I had an idea in my head and I executed it more less exactly how I wanted.
What I have not implemented:
1. Transitions into different colour schemes - I had difficulty finding the right colours and I didnt want a simple RGB change). It had to feel right.
2. Adding UI for the user to add their own songs.
3. Having hexagons align with each other. This is a complex mathematical task that I spend quite a bit of time on, but in the end when I increased the count of the hexagons from 1024 to 8192, it turned out that they are so "small" that it didn't matter if they were aligned side by side but also the light from inside has a gap to shine onto the columns. Unexpected behaviour became a feature.
4. Have the camera go around the sphere "on rail".

Learning Unity is fun, and allowing my creative side to make something aesthetically pleasing with code which made me happy. Even though I worked on this project for a couple of days, I still find myself watching the demo video multiple times to look at my own creation.

---

# Proposal:
## Description
Simply: A Sphere that visualises music.

Sounds simple, but it requires a lot of maths! 

First inspiration came from a WebGL globe. I want my sphere to shoot out hexagonal pillars depending on audio frequency. 

![WebGL globe](images/pillars.png)

I would love to create something like that but on a hollow sphere, made of hexagons like so:

![](images/hexasphere.png)

Once the sphere is made from hundreds of hexagons, and working, then the next step will be to add background waves like those:

![wave](images/wave.jpg)

When those 3 things are combined together, it will look interesting when looking how it reacts to music. I also plan to add a lot of lighting effects and transitions.

## Challenges
- Maths.. lot's of maths in every aspect of this project. 
- Time management, I'm hoping to achieve what I can within the little time we have between all assignments and FYP project.
- Learning Unity. Labs are great, but this will be the real challenge.

## References
[WebGL Globe](https://experiments.withgoogle.com/chrome/globe)

[Loop WaveForm](https://www.uberviz.io/viz/loop/)

[HexaSphere Thread](https://forum.unity.com/threads/make-procedural-hexagons-on-a-sphere-is-there-a-tool-voxels-perhaps.330907/)
