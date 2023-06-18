using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

    [Header("Spawnables")]
    [SerializeField]
    private SpawnInfo boundaryPrefab;

    [SerializeField]
    private SpawnInfo[] spawnablePrefabs;

    [Header("Others")]
    [SerializeField]
    private Transform actorRoot;

    [SerializeField]
    private TextAsset level;

    [SerializeField]
    private int tilesPerSide = 26;

    private Dictionary<char, SpawnInfo> spawnablePerIdentifier;

    private static readonly char BoundaryIdentifier = 'X';

    private void Awake() {
        this.InitializeSpawnables();
    }

    private void InitializeSpawnables() {
        this.spawnablePerIdentifier = new Dictionary<char, SpawnInfo>();

        foreach (SpawnInfo spawnInfo in spawnablePrefabs) {
            if (this.spawnablePerIdentifier.ContainsKey(spawnInfo.Identifier)) {
                Debug.LogError($"Duplicate spawn identifier found: {spawnInfo.Identifier}");
                continue;
            }

            this.spawnablePerIdentifier[spawnInfo.Identifier] = spawnInfo;
        }
    }

    private void InjectBoundaries(ref string text, ref int tilesPerSide) {
        // add 2 for the boundaries on each side
        tilesPerSide += 2;

        string boundaryLine = string.Empty;

        for (int i = 0; i < tilesPerSide - 1; i++) {
            boundaryLine = $"{BoundaryIdentifier}{boundaryLine}";
        }

        text = $"{boundaryLine}\n{text}\n{boundaryLine}";

        string linebreak = @"(\r\n|\r|\n)";
        string boundary = $"{BoundaryIdentifier}{BoundaryIdentifier}";
        text = Regex.Replace(text, linebreak, boundary);
    }

    private void LoadLevel(TextAsset level) {
        string text = level.text;
        int tilesPerSide = this.tilesPerSide;

        this.InjectBoundaries(ref text, ref tilesPerSide);

        for (int i = 0; i < text.Length; i ++) {
            char identifier = text[i];

            if (char.IsWhiteSpace(identifier)) {
                continue;
            }

            if (! this.spawnablePerIdentifier.ContainsKey(identifier)) {
                Debug.LogWarning($"Unsupported identifier: {identifier}.");
                continue;
            }

            float offset = tilesPerSide * 0.5f;
            float x = i % tilesPerSide - offset;
            float z = tilesPerSide - (i / tilesPerSide) - offset;

            //Debug.Log($"{i}: x:{x} z:{z} char:{text[i]}");
            this.Spawn(this.spawnablePerIdentifier[identifier], x, z);
        }
    }

    private void Spawn(SpawnInfo spawnInfo, float x, float z) {
        Vector2 tileSize = spawnInfo.TileSize;
        Vector3 position = new Vector3(x + tileSize.x * 0.5f, 0.5f, z - tileSize.y * 0.5f);

        GameObject actor = Object.Instantiate<GameObject>(
            spawnInfo.gameObject,
            position,
            Quaternion.identity,
            this.actorRoot
        );

        actor.name = $"{spawnInfo.name} ({x}, {z})";
    }

    private void Start() {
        this.LoadLevel(this.level);
    }
}
