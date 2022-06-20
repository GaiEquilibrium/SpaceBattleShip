using Godot;
using System;

public class ShootDrawer : ImmediateGeometry
{
    [Export]
    public Color ShipShootingColor = Colors.LightYellow;
    [Export]
    public Color ThisTurnShotColor = Colors.LightYellow;
    [Export]
    public Color HitColor = Colors.Red;
    private float ShipShootiongMarkRarge = 0.4f;
    private float ThisTurnShotMarkRange = 0.3f; 
    private float HitMarkOffset = 0.3f;

    public void ShipShooting(Vector3 shipPos)
    {
        Begin (Mesh.PrimitiveType.Lines);
        //DrawShipShooting
        SetColor  (ShipShootingColor);
        AddVertex (shipPos);
        AddVertex (new Vector3(shipPos.x,                       shipPos.y,                      GridDrawer.GridOffset));
        AddVertex (new Vector3(shipPos.x + ThisTurnShotMarkRange,   shipPos.y,                      GridDrawer.GridOffset));
        AddVertex (new Vector3(shipPos.x - ThisTurnShotMarkRange,   shipPos.y,                      GridDrawer.GridOffset));
        AddVertex (new Vector3(shipPos.x,                       shipPos.y + ThisTurnShotMarkRange,  GridDrawer.GridOffset));
        AddVertex (new Vector3(shipPos.x,                       shipPos.y - ThisTurnShotMarkRange,  GridDrawer.GridOffset));
        End();
    }
    public void ShipReadyToFire(Vector3 shipPos)
    {
        Begin (Mesh.PrimitiveType.Lines);
        SetColor  (ThisTurnShotColor);
        AddVertex (new Vector3(shipPos.x + ThisTurnShotMarkRange,   shipPos.y + ThisTurnShotMarkRange,  GridDrawer.GridOffset));
        AddVertex (new Vector3(shipPos.x - ThisTurnShotMarkRange,   shipPos.y - ThisTurnShotMarkRange,  GridDrawer.GridOffset));
        AddVertex (new Vector3(shipPos.x - ThisTurnShotMarkRange,   shipPos.y + ThisTurnShotMarkRange,  GridDrawer.GridOffset));
        AddVertex (new Vector3(shipPos.x + ThisTurnShotMarkRange,   shipPos.y - ThisTurnShotMarkRange,  GridDrawer.GridOffset));
        End();
    }
    public void FleetHit(Vector3 hitPos)
    {
        Begin (Mesh.PrimitiveType.Lines);
        SetColor  (HitColor);
        AddVertex (new Vector3(hitPos.x + HitMarkOffset, hitPos.y + HitMarkOffset,  GridDrawer.GridOffset));
        AddVertex (new Vector3(hitPos.x + HitMarkOffset, hitPos.y - HitMarkOffset,  GridDrawer.GridOffset));
        AddVertex (new Vector3(hitPos.x + HitMarkOffset, hitPos.y - HitMarkOffset,  GridDrawer.GridOffset));
        AddVertex (new Vector3(hitPos.x - HitMarkOffset, hitPos.y - HitMarkOffset,  GridDrawer.GridOffset));
        AddVertex (new Vector3(hitPos.x - HitMarkOffset, hitPos.y - HitMarkOffset,  GridDrawer.GridOffset));
        AddVertex (new Vector3(hitPos.x - HitMarkOffset, hitPos.y + HitMarkOffset,  GridDrawer.GridOffset));
        AddVertex (new Vector3(hitPos.x - HitMarkOffset, hitPos.y + HitMarkOffset,  GridDrawer.GridOffset));
        AddVertex (new Vector3(hitPos.x + HitMarkOffset, hitPos.y + HitMarkOffset,  GridDrawer.GridOffset));

        AddVertex (new Vector3(hitPos.x, hitPos.y, GridDrawer.GridOffset));
        AddVertex (new Vector3(hitPos));
        if (hitPos.z < GameMain.FieldSize)
        {
            AddVertex (new Vector3(hitPos.x + GridDrawer.GridOffset, hitPos.y + GridDrawer.GridOffset, hitPos.z + GridDrawer.GridOffset));
            AddVertex (new Vector3(hitPos.x - GridDrawer.GridOffset, hitPos.y - GridDrawer.GridOffset, hitPos.z - GridDrawer.GridOffset));
            AddVertex (new Vector3(hitPos.x + GridDrawer.GridOffset, hitPos.y + GridDrawer.GridOffset, hitPos.z - GridDrawer.GridOffset));
            AddVertex (new Vector3(hitPos.x - GridDrawer.GridOffset, hitPos.y - GridDrawer.GridOffset, hitPos.z + GridDrawer.GridOffset));
            AddVertex (new Vector3(hitPos.x - GridDrawer.GridOffset, hitPos.y + GridDrawer.GridOffset, hitPos.z + GridDrawer.GridOffset));
            AddVertex (new Vector3(hitPos.x + GridDrawer.GridOffset, hitPos.y - GridDrawer.GridOffset, hitPos.z - GridDrawer.GridOffset));
            AddVertex (new Vector3(hitPos.x - GridDrawer.GridOffset, hitPos.y + GridDrawer.GridOffset, hitPos.z - GridDrawer.GridOffset));
            AddVertex (new Vector3(hitPos.x + GridDrawer.GridOffset, hitPos.y - GridDrawer.GridOffset, hitPos.z + GridDrawer.GridOffset));
        }
        End();
    }

    public void NextTurnClear()
    {
        Clear();
    }
}
