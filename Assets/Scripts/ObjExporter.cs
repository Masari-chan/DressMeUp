using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class ObjExporter : MonoBehaviour
{

    int currentModel = 0;

    [SerializeField]
    public GameObject meshParaExportar;

    private string objElement;

    public void exportObject()
    {
        // SkinnedMeshRenderer[] meshfilterRender = meshParaExportar.GetComponentsInChildren<SkinnedMeshRenderer>();
        // Debug.Log("Se van a exportar " + meshfilterRender.Length + " archivos");

        // for (int i = 0; i < meshfilterRender.Length; i++)
        // {
        //     objElement += MeshToString( meshfilterRender[i].sharedMesh, sb);
        // }

        // StringToFile( objElement, Application.dataPath + "/exportedFBX/prueba" + currentModel + ".obj" );

        
        if (meshParaExportar != null)
        {
            MeshToFile(meshParaExportar.GetComponent<MeshFilter>(), Application.dataPath + "/exportedFBX/prueba" + currentModel + ".obj");
        } else {
            Debug.LogError("No se ha localizado el mesh filter.");
        }

    }

    // public static string MeshToString(Mesh m, StringBuilder sb)
    // public static string MeshToString(Mesh m)
    public static string MeshToString(MeshFilter mf)
    {
        Mesh m = mf.mesh;
        Material[] mats = mf.GetComponent<Renderer>().sharedMaterials;

        StringBuilder sb = new StringBuilder();

        sb.Append("g ").Append("prueba").Append("\n");
        foreach (Vector3 v in m.vertices)
        {
            sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.normals)
        {
            sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.uv)
        {
            sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
        }
        for (int material = 0; material < m.subMeshCount; material++)
        {
            sb.Append("\n");
            sb.Append("usemtl ").Append(mats[material].name).Append("\n");
            sb.Append("usemap ").Append(mats[material].name).Append("\n");

            int[] triangles = m.GetTriangles(material);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                    triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1));
            }
        }
        return sb.ToString();
    }

    public static void MeshToFile(MeshFilter mf, string filename)
    {
        using (StreamWriter sw = new StreamWriter(filename))
        {
            sw.Write(MeshToString(mf));
        }
    }

    public static void StringToFile ( string st, string filename ) {
        using (StreamWriter sw = new StreamWriter(filename))
        {
            sw.Write(st);
            Debug.Log("Se ha generado el archivo obj");
        }
    }
}
