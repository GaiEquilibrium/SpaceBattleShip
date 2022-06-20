using System;
using Godot;

public class ObjectMouseHandler : Spatial
{
    //todo: добавить по времени и проверку на смещение?
    private Vector3 _mousePos;
    private bool _isClickble;
    private bool _mouseInside;

    // public Vector3 MouseClickPos
    // { 
    //     get; 
    //     private set; 
    // }
    public Vector3 MousePos 
    {
        get => _mousePos;
        private set
        {
            _mousePos.x = Mathf.Round(value.x);
            _mousePos.y = Mathf.Round(value.y);
            _mousePos.z = Mathf.Round(value.z);
        }
    }
    public bool IsClickble 
    {
        get => _isClickble;
        set
        {
            //отключает собственно колайдер, и после этого, мышь не обнаруживается
            GetNode<CollisionShape>("Area/CollisionShape").Disabled = !value; 
            _isClickble = value;
        }
    }
    public bool IsMouseInside 
    {
        get => _mouseInside;
    }

    [Signal]
    public delegate void Clicked(ObjectMouseHandler node);

    public void OnAreaMouseEntered ()
    {
        _mouseInside = true;
    }
    public void OnAreaMouseExited()
    {
        _mouseInside = false;
    }
    public void OnAreaInputEvent(Node camera, InputEvent inputEvent, Vector3 position, Vector3 normal, int shape_idx)
    {   
        if (inputEvent is InputEventMouse)
        {
            _mouseInside = true;
            MousePos = position;
        }
    }
    public override void _Ready()
    {
        _mousePos = Vector3.Zero;
    }

    public override void _Process(float delta)
    {
        if (Input.IsMouseButtonPressed((int)ButtonList.Left) && _mouseInside)
        {
            _mouseInside = false;
            IsClickble = false;
            EmitSignal("Clicked", this);
        }
    }
}
