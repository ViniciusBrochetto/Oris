using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(TriPlanarTerrain))]
class TriPlanarTerrainEditor : UnityEditor.Editor
{
    public TriPlanarTerrain terScript;
    public Terrain terrain;
    public TerrainData terDat;
    public bool[] showTextures = new bool[8];
    public bool showHelp = false;
    public override void OnInspectorGUI()
    {
        terScript = (TriPlanarTerrain)target;
        if (terrain == null)
        {
            EditorGUI.indentLevel = 0;
            GUILayout.Label("Attach this script to a terrain.");
            terrain = terScript.GetComponent<Terrain>();
        }
        if ((terrain != null) && (terDat == null))
        {
            EditorGUI.indentLevel = 0;
            GUILayout.Label("Attach terrain data to this terrain.");
            terDat = terrain.terrainData;
        }
        if (terDat != null)
        {
            EditorGUI.indentLevel = 0;
            showHelp = EditorGUILayout.Foldout(showHelp, "Help");
            if (showHelp)
            {
                EditorGUI.indentLevel = 1;
                GUILayout.Label(
                "  Diffuse textures:\n   These are assigned in the regular Terrain script.\n   They are shown here only for reference.\n\n  Specular/gloss textures:\n   Specular value is taken from the red channel.\n   Gloss value is taken from the green channel.\n   The blue channel is unused."
                );
            }
            for (int i = 0; i < terDat.splatPrototypes.Length; i++)
            {
                EditorGUI.indentLevel = 0;
                showTextures[i] = EditorGUILayout.Foldout(showTextures[i], "Layer " + i + " (" + terDat.splatPrototypes[i].texture.name + ")");
                if (showTextures[i])
                {
                    EditorGUI.indentLevel = 1;
                    terScript.TilesPerMeter[i] = EditorGUILayout.Slider("Tiling Amount", terScript.TilesPerMeter[i], 0.1f, 100.0f);
                    EditorGUILayout.ObjectField("Diffuse", terDat.splatPrototypes[i].texture, typeof(Texture), false);
                    terScript.BumpTextures[i] = (Texture2D)EditorGUILayout.ObjectField("Normal", terScript.BumpTextures[i], typeof(Texture), false);
                    terScript.SpecTextures[i] = (Texture2D)EditorGUILayout.ObjectField("Spec / Gloss", terScript.SpecTextures[i], typeof(Texture), false);
                    EditorGUILayout.Space();
                }
            }
        }
        terScript.SetTerrainValues();
    }
}