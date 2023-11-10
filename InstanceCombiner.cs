using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InstanceCombiner : MonoBehaviour
{
    // Source Meshes you want to combine
    [SerializeField] private List<MeshFilter> listMeshFilter;

    // Make a new mesh to be the target of the combine operation
    [SerializeField] private MeshFilter TargetMesh;

    [ContextMenu("Combine Meshes")]
    private void CombineMesh()
    {
        //Make an array of CombineInstance.
        var combine = new CombineInstance[listMeshFilter.Count];

        //Set Mesh And their Transform to the CombineInstance
        for (int i = 0; i < listMeshFilter.Count; i++)
        {
            combine[i].mesh = listMeshFilter[i].sharedMesh;
            combine[i].transform = listMeshFilter[i].transform.localToWorldMatrix;
        }

        // Create a Empty Mesh
        var mesh = new Mesh();

        //Call targetMesh.CombineMeshes and pass in the array of CombineInstances.
        mesh.CombineMeshes(combine);

        //Assign the target mesh to the mesh filter of the combination game object.
        TargetMesh.mesh = mesh;

        // Save The Mesh To Location
        SaveMesh(TargetMesh.sharedMesh, gameObject.name, false, true);

        // Print Results
        print($"<color=#20E7B0>Combine Meshes was Successful!</color>");
    }


    public static void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
    {
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path);

        Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;

        if (optimizeMesh)
            MeshUtility.Optimize(meshToSave);

        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();
    }
}
