using Godot;
using System;

public class LineDrawer : ImmediateGeometry
{
    private Mesh.PrimitiveType DrawingType = Mesh.PrimitiveType.LineStrip;
    public Vector3[] Verticies = new Vector3[7];
    public Color LineColor = Colors.DarkRed;

    public override void _Process(float delta)
    {
        Clear();
        Begin (DrawingType);
        foreach (Vector3 point in Verticies)
        {
            SetColor(LineColor);
            AddVertex (point);
        }
        End();
    }
}
