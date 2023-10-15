using System.Collections;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode {NoiseMap, ColorMap, Mesh, Falloff};
    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeight;
    public float heightMultiplier;
    public AnimationCurve heightCurve;
    public float noiseScale;

    public int octaves;
    public float persistance;
    public float lacunarity;

    public int seed;   
    public Vector2 offset;

    public bool autoUpdate;
    public bool useFalloff;

    [Range(0.0f, 1.0f)]
    public float a;
    [Range(0.0f, 1.0f)]
    public float b;
    
    public TerrainType[] regions;

    float[,] falloffMap;

    void Awake() {
		falloffMap = FalloffGen.GenerateFalloffMap(mapWidth, a, b);
	}

    void Start() {
        GenerateMap();
    }


    public void GenerateMap() {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight,seed, noiseScale, octaves, persistance, lacunarity, offset);
        Color[] colorMap = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth ; x++) {
                float currentHeight = 0;
                if (useFalloff) {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }
                currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++) {
                    if (currentHeight <= regions[i].height) {
                        colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap(noiseMap));
		} else if (drawMode == DrawMode.ColorMap) {
			display.DrawTexture (TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
		}else if (drawMode == DrawMode.Mesh) {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, heightMultiplier, heightCurve), TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        }else if(drawMode == DrawMode.Falloff){
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGen.GenerateFalloffMap(mapWidth, a, b)));
        }
    }

    void OnValidate() {
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}

		falloffMap = FalloffGen.GenerateFalloffMap (mapWidth, a, b);
	}
    

}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color color;
}