
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

[DefaultExecutionOrder(int.MinValue)]
public class RunShader : MonoBehaviour {
    [SerializeField] private ComputeShader shader;
    [SerializeField] private TextAsset input;
    [SerializeField] private int frame;
    [SerializeField] private Vector2Int dimension;
    [SerializeField] private Vector2Int threadGroups;

    private bool ready;
    private int kernelHandle;
    private int4[] data;
    public static int2[] output;
    private ComputeBuffer outBuffer, inBuffer;

    private void OnValidate() {
        if (ready) {
            Calculate(ref output);
        }
    }

    private void OnEnable() {
        if (input == null) {
            data = new int4[0];
        } else {
            data = Day10Parser.ParseInput(input.text);
        }
        kernelHandle = shader.FindKernel("ComputeSignal");
        inBuffer = new ComputeBuffer(data.Length * sizeof(int) * 4, sizeof(int) * 4);
        inBuffer.SetData(data);
        outBuffer = new ComputeBuffer(data.Length * sizeof(int) * 2, sizeof(int));
        output = new int2[data.Length];
        shader.SetBuffer(kernelHandle, "input", inBuffer);
        shader.SetBuffer(kernelHandle, "output", outBuffer);
        ready = true;

        Calculate(ref output);
    }


    public void Calculate(ref int2[] output) {
        shader.SetInt(Shader.PropertyToID("frame"), frame);
        shader.Dispatch(kernelHandle, threadGroups.x, threadGroups.y, 1);
        outBuffer.GetData(output);
    }
}
