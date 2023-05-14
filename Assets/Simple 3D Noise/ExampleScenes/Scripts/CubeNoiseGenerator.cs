using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CubeNoiseGenerator : MonoBehaviour {

    public float noiseScale = 0.1f;
    public int gridSize = 30;

    public Material noiseMat;

    void Start() {

        GameObject cubeParent = new GameObject();
        cubeParent.name = "Noise Cubes";

        for(int x = 0; x < gridSize; x++) {
            for(int y = 0; y < gridSize; y++) {
                for(int z = 0; z < gridSize; z++) {
                    float noiseValue = (float)NoiseS3D.NoiseCombinedOctaves(x * noiseScale, y * noiseScale, z * noiseScale);

                    //remap the value to 0 - 1 for color purposes
                    noiseValue = (noiseValue + 1) * 0.5f;

                    GameObject noiseCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					noiseCube.GetComponent<Renderer>().sharedMaterial = Instantiate(noiseMat) as Material;
                    Destroy(noiseCube.GetComponent<Collider>());

                    noiseCube.GetComponent<Renderer>().sharedMaterial.SetColor("_TintColor", new Color(noiseValue, noiseValue, noiseValue, noiseValue * 0.015f));

                    noiseCube.transform.SetParent(cubeParent.transform);
                    noiseCube.transform.position = new Vector3(x, y, z);
                }
            }
        }

        cubeParent.transform.position -= new Vector3(gridSize / 2, 0, gridSize / 2);
        CreateTexture3D(gridSize);

    }
    [MenuItem("CreateExamples/3DTexture")]
    static void CreateTexture3D(int size)
    {
        float noiseScale = 0.1f;
        // 配置纹理
        size = 32;
        TextureFormat format = TextureFormat.RGBA32;
        TextureWrapMode wrapMode =  TextureWrapMode.Clamp;

        // 创建纹理并应用配置
        Texture3D texture = new Texture3D(size, size, size, format, false);
        texture.wrapMode = wrapMode;

        // 创建 3 维数组以存储颜色数据
        Color[] colors = new Color[size * size * size];

        // 填充数组，使纹理的 x、y 和 z 值映射为红色、蓝色和绿色
        for (int z = 0; z < size; z++)
        {
            int zOffset = z * size * size;
            for (int y = 0; y < size; y++)
            {
                int yOffset = y * size;
                for (int x = 0; x < size; x++)
                {
                    float noiseValue = (float)NoiseS3D.NoiseCombinedOctaves(x * noiseScale, y * noiseScale, z * noiseScale);
                    noiseValue = (noiseValue + 1) * 0.5f;
                    
                    colors[x + yOffset + zOffset] = new Color(noiseValue, noiseValue, noiseValue, noiseValue * 0.015f);
                }
            }
        }

        // 将颜色值复制到纹理
        texture.SetPixels(colors);

        // 将更改应用到纹理，然后将更新的纹理上传到 GPU
        texture.Apply();        

        // 将纹理保存到 Unity 项目
        AssetDatabase.CreateAsset(texture, "Assets/Example3DTexture.asset");
    }

}
