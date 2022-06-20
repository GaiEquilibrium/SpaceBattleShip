using Godot;
using System;

public class CameraControl : Camera
{
    [Export]
    public float CameraLinearSpeed = 5;
    [Export]
    public float CameraRotationSpeed = 0.1f;
    [Export]
    public int MouseMoveTrashhold = 10;

    private bool _pressedMouse = false;
    private bool _pressedMouseMoving = false;
    private Vector2 _mousePos = Vector2.Zero;

    public override void _Process(float delta)
    {
        //отслеживаем что хотим повернуть только по расстоянию, на которую перемещена мышь, или и по времени?
        if (_pressedMouseMoving)
        {
            //GD.Print("Rotation angle = " + ((GetViewport().GetMousePosition() - _mousePos)*CameraRotationSpeed).ToString());
            var dif = (GetViewport().GetMousePosition() - _mousePos)*CameraRotationSpeed;
            RotationDegrees += new Vector3(dif.y, dif.x, 0);
            _mousePos = GetViewport().GetMousePosition();
        }

        if (_pressedMouse && (GetViewport().GetMousePosition() - _mousePos).Length() > MouseMoveTrashhold && !_pressedMouseMoving)
        {
            _pressedMouseMoving = true;
            _mousePos = GetViewport().GetMousePosition();
            //GD.Print("mouse moved");
        }

        if (Input.IsMouseButtonPressed((int)ButtonList.Right) && !_pressedMouse)
        {
            _pressedMouse = true;
            _mousePos = GetViewport().GetMousePosition();
            //GD.Print("mouse pressed at " + _mousePos.ToString());
        }

        if (!Input.IsMouseButtonPressed((int)ButtonList.Right) && _pressedMouse)
        {
            _pressedMouse = false;
            _pressedMouseMoving = false;
            //GD.Print("mouse released");
        }


        var direction = Vector3.Zero;
        if (Input.IsActionPressed("CameraUp"))
        {
            direction += Vector3.Up;
        }
        if (Input.IsActionPressed("CameraDown"))
        {
            direction += Vector3.Down;

        }
        if (Input.IsActionPressed("CameraForward"))
        {
            direction += Vector3.Forward;
        }
        if (Input.IsActionPressed("CameraBack"))
        {
            direction += Vector3.Back;
        }
        if (Input.IsActionPressed("CameraRight"))
        {
            direction += Vector3.Right;
        }
        if (Input.IsActionPressed("CameraLeft"))
        {
            direction += Vector3.Left;
        }
        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized() * CameraLinearSpeed * delta;
            Translate(direction);
        }

        Vector3 currentPositionNormolized = Translation;
        currentPositionNormolized.x = Mathf.Clamp(currentPositionNormolized.x, -GameMain.FieldSize, 2 * GameMain.FieldSize - 1);
        currentPositionNormolized.y = Mathf.Clamp(currentPositionNormolized.y, -GameMain.FieldSize, 2 * GameMain.FieldSize - 1);
        currentPositionNormolized.z = Mathf.Clamp(currentPositionNormolized.z, -GameMain.FieldSize, 2 * GameMain.FieldSize - 1);
        Translation = currentPositionNormolized;
    }
}
