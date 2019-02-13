using Unity.Entities;
using UnityEngine;

public class CameraScaleSytem : ComponentSystem {
    public struct CameraEntity {
        public Camera camera;
        CameraScaleComponent scale;
    }

    ConfigComponent config;

    protected override void OnStartRunning() {
        config = GameObject.FindObjectOfType<ConfigComponent>();
    }

    protected override void OnUpdate() {
        foreach (var entity in GetEntities<CameraEntity>()) {
            entity.camera.orthographicSize = config.CameraSize;
        }
    }


}
