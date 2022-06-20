using Godot;
using Godot.Collections;
using System;

//todo: сделать класс, который занимается чисто отрисовкой сетки. Одной сетки, а потом просто несколько наплодить в качестве детей класу отвечающему за сетки.

public class GridDrawer : MeshInstance
{
    public static float CellSize = 1;
    public static float GridOffset = -CellSize/2;
    public static float InternalSize = CellSize - 0.1f;

    [Export]
    public Color GridColor = Colors.Gray;
    [Export]
    public int GridSize = GameMain.FieldSize;
    [Export]
    public bool UseOffset = false;
    [Export]
    public bool IsInternal = false;

    public override void _Ready()
    {
        var array_mesh = new ArrayMesh();
        var arrays = new Godot.Collections.Array();
        var verticies = new Array<Vector3>();
        Color[] colors;
        var primitiveType = Mesh.PrimitiveType.Lines;
        
        float horizontalOffset = -CellSize/2f; 
        float verticalOffset = 0;
        if (UseOffset)
        {
            verticalOffset = GridOffset;
        }
        if (GridSize == 1 && IsInternal)
        {
            {
                verticies.Add(new Vector3(-horizontalOffset * InternalSize, 0, -horizontalOffset * InternalSize));
                verticies.Add(new Vector3( horizontalOffset * InternalSize, 0, -horizontalOffset * InternalSize));
                verticies.Add(new Vector3( horizontalOffset * InternalSize, 0,  horizontalOffset * InternalSize));
                verticies.Add(new Vector3(-horizontalOffset * InternalSize, 0,  horizontalOffset * InternalSize));
            }
            colors = new Color[4];
            primitiveType = Mesh.PrimitiveType.LineLoop;
        }
        else
        {
            for (int i = 0; i<= GridSize; i++)
            {
                verticies.Add(new Vector3(horizontalOffset + i,         verticalOffset,  horizontalOffset));
                verticies.Add(new Vector3(horizontalOffset + i,         verticalOffset,  horizontalOffset + GridSize));
                verticies.Add(new Vector3(horizontalOffset,             verticalOffset,  horizontalOffset + i));
                verticies.Add(new Vector3(horizontalOffset + GridSize,  verticalOffset,  horizontalOffset + i));
            }        
            colors = new Color[(GridSize+1) * 4];     
        }
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = GridColor;
        };
        
        arrays.Resize((int)Mesh.ArrayType.Max);
        arrays[(int)Mesh.ArrayType.Vertex] = verticies;
        arrays[(int)Mesh.ArrayType.Color] = colors;
        array_mesh.AddSurfaceFromArrays(primitiveType, arrays);
        Mesh = array_mesh;
    }
}