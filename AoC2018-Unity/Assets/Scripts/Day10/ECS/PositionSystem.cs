using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PositionSystem : ComponentSystem {
    public struct ConfigEntity {
        public ConfigComponent config;
    }

    public struct PointEntity {
        public Transform transform;
        public PointComponent point;
    }

    ConfigComponent config;
    int2[] data => RunShader.output;

    protected override void OnStartRunning() {
        config = GameObject.FindObjectOfType<ConfigComponent>();
        int totalEntities = GetEntities<PointEntity>().Length;
        for (int i = totalEntities; i < data.Length; i++) {
            GameObject.Instantiate(config.DotPrefab);
        }
    }

    protected override void OnUpdate() {
        if (data != null) {
            var entities = GetEntities<PointEntity>();
            for (int i = 0; i < entities.Length; i++) {
                var entity = entities[i];
                var point = data[i];
                entity.transform.position = Int22V2Lol(point) * config.PositionScale;
                entity.transform.localScale = Vector3.one * config.SizeScale;
            }
        }
    }

    public static Vector2 Int22V2Lol(int2 i2) {
        return new Vector2(i2.x, i2.y);
    }
}

