using UnityEngine;
public class TriPlanarTerrain : MonoBehaviour
{
    public Texture2D[] BumpTextures = new Texture2D[8];
    public Texture2D[] SpecTextures = new Texture2D[8];
    public float[] TilesPerMeter = new float[8];
    public TerrainData TerDat;
    private Texture2D blankNormal;
    private Texture2D blankSpec;
    public void Start()
    {
        SetTerrainValues();
    }
    public void SetTerrainValues()
    {
        if (blankNormal == null)
        {
            blankNormal = GenerateBlankNormal();
        }
        if (blankSpec == null)
        {
            blankSpec = GenerateBlankSpec();
        }
        if ((GetComponent<Terrain>() != null) && (TerDat == null))
        {
            TerDat = GetComponent<Terrain>().terrainData;
        }
        if (TerDat != null)
        {
            var splatLength = TerDat.splatPrototypes.Length;
            int i = 0;
            for (i = 0; i < splatLength; i++)
            {
                if (BumpTextures[i] != null)
                {
                    Shader.SetGlobalTexture("_BumpMap" + i, BumpTextures[i]);
                }
                else
                {
                    Shader.SetGlobalTexture("_BumpMap" + i, blankNormal);
                }
                if (SpecTextures[i] != null)
                {
                    Shader.SetGlobalTexture("_SpecMap" + i, SpecTextures[i]);
                }
                else
                {
                    Shader.SetGlobalTexture("_SpecMap" + i, blankSpec);
                }
                if (TilesPerMeter[i] != 0.0f)
                {
                    Shader.SetGlobalFloat("_TerrainTexScale" + i, (1.0f / TilesPerMeter[i]));
                }
                else
                {
                    Shader.SetGlobalFloat("_TerrainTexScale" + i, 1.0f);
                }
            }
            while (i < 8)
            {
                BumpTextures[i] = null;
                SpecTextures[i] = null;
                TilesPerMeter[i] = 1.0f;
                i++;
            }
        }
    }
    public Texture2D GenerateBlankNormal()
    {
        var texture = new Texture2D(16, 16, TextureFormat.ARGB32, false);
        var cols = texture.GetPixels32(0);
        var colsLength = cols.Length;
        for (int i = 0; i < colsLength; i++)
        {
            cols[i] = new Color(0, 0.5f, 0, 0.5f);
        }
        texture.SetPixels32(cols, 0);
        texture.Apply(false);
        texture.Compress(false);
        return texture;
    }
    public Texture2D GenerateBlankSpec()
    {
        var texture = new Texture2D(16, 16, TextureFormat.RGB24, false);
        var cols = texture.GetPixels(0);
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i] = new Color(0.1f, 0.1f, 0, 0);
        }
        texture.SetPixels(cols, 0);
        texture.Apply(false);
        texture.Compress(false);
        return texture;
    }
}