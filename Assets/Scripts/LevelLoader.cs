using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

    [SerializeField]
    private Transform actorRoot;

    [SerializeField]
    private TextAsset level;

    [SerializeField]
    private SpawnTag[] spawnablePrefabs;

    [SerializeField]
    private int tilesPerSide = 26;

    private Dictionary<char, GameObject> spawnablePerIdentifier;

    private void Awake() {
        this.InitializeSpawnables();
    }

    private void InitializeSpawnables() {
        this.spawnablePerIdentifier = new Dictionary<char, GameObject>();

        foreach (SpawnTag tag in spawnablePrefabs) {
            if (this.spawnablePerIdentifier.ContainsKey(tag.Identifier)) {
                Debug.LogError($"Duplicate spawn tag found: {tag.Identifier}");
                continue;
            }

            this.spawnablePerIdentifier[tag.Identifier] = tag.gameObject;
        }
    }

    private void LoadLevel(TextAsset level) {
        string text = level.text.Replace("\n", string.Empty).Replace("\r", string.Empty);

        for (int i = 0; i < text.Length; i ++) {
            char identifier = text[i];

            if (char.IsWhiteSpace(identifier)) {
                continue;
            }

            if (! this.spawnablePerIdentifier.ContainsKey(identifier)) {
                Debug.LogWarning($"Unsupported identifier: {identifier}.");
                continue;
            }

            int x = i % tilesPerSide;
            int z = tilesPerSide - (i / tilesPerSide);
            float offset = tilesPerSide * 0.5f;

            //Debug.Log($"{i}: x:{x} z:{z} char:{text[i]}");
            GameObject prefab = this.spawnablePerIdentifier[identifier];

            GameObject actor = Object.Instantiate<GameObject>(
                prefab,
                new Vector3(x - offset + 0.5f, 0.5f, z - offset - 0.5f),
                Quaternion.identity,
                this.actorRoot
            );

            actor.name = $"{prefab.name} ({x}, {z})";
        }
    }

    private void Start() {
        this.LoadLevel(this.level);
    }
}
