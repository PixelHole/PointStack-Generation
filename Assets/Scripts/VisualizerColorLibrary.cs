using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VisualizerColorLibrary : ScriptableObject
{
    public List<Vector2> heights = new List<Vector2>();
    public List<Color> colors = new List<Color>();

    public Color GetColorForHeight(float height)
    {
        Color heightColor = Color.magenta;
        for (int i = 0; i < heights.Count; i++)
        {
            if (height > heights[i].x & height <= heights[i].y)
            {
                heightColor = colors[i];
                break;
            }
        }
        return heightColor;
    }
}
