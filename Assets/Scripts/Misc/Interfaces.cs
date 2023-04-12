using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRangerAbility
{
    public void OnAbilityStart();
    public void OnAbilityEnd();
    public void OnInput(Vector2 input);
}

public interface IGrabbable
{
    public void OnGrabbed();
    public void OnInput(Vector2 input);
    public void OnReleased();
}

public enum TileTypes:int
{
    Solo,
    OneEdge,
    TwoEdge,
    ThreeEdge,
    FourEdge,
    Corner,
    CornerWall,
    CornerFloor,
    Wall
}

public enum ResourceTypes
{
    Destron,
    Anima,
    Aquidia
}
