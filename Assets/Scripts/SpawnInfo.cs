using UnityEngine;

public class SpawnInfo : MonoBehaviour {

    [field: SerializeField]
    public char Identifier { get; private set; }

    [field: SerializeField]
    public Vector2 TileSize { get; private set; } = Vector2.one;
}
