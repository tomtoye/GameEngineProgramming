using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralGrid : MonoBehaviour
{
    public int rows, columns;
    private Vector3[] _vertices;
    private Vector3[] _normals;
    // Use this for initialization



    private void OnDrawGizmos()
    {
        if (_vertices != null)
        {
            Gizmos.color = Color.black;
            foreach (Vector3 vec3 in _vertices)
            {
                Gizmos.DrawSphere(vec3, 0.1f);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        int iTotalVertices = (rows + 1) * (columns + 1);
        _vertices = new Vector3[iTotalVertices];
        _normals = new Vector3[iTotalVertices];
        Generate();
    }

    private void Generate()
    {
        //WaitForSeconds wait = new WaitForSeconds(0.05f);
        MeshFilter _meshFilter = GetComponent<MeshFilter>();
        Mesh _mesh = new Mesh();
        _meshFilter.mesh = _mesh;

        int x = 0, y = 0;
        for (int i = 0; i < _vertices.Length; ++i)
        {
            _vertices[i].Set(x, y, 0);
            _normals[i].Set(0f, 0f, -1f);
            ++x;
            if (x % (columns + 1) == 0)
            {
                x = 0;
                ++y;
            }
           
            //yield return wait;
        }

        _mesh.vertices = _vertices;



          int numIndicies = columns * rows * 6;
          int[] triangles = new int[numIndicies];
        //for (int ti = 0, vi = 0; vi< (columns* (rows+1)); ++vi, ti += 6 )
        //{
        //    ///=================================================================
        //    /// This is an indication of how our Tri's are composed
        //    /// x----x----x----x----x----x
        //    /// |\   |\   |\   |\   |\   |
        //    /// | \  | \  | \  | \  | \  |
        //    /// |  \ |  \ |  \ |  \ |  \ |
        //    /// |   \|   \|   \|   \|   \|
        //    /// x----x----x----x----x----x
        //    /// With indices ordered as follows per each quad
        //    /// 1----3 Indices are then ordered as 
        //    /// |\   | [0,1,2,2,1,3] in the index buffer
        //    /// | \  |
        //    /// |  \ |
        //    /// |   \|
        //    /// 0----2
        //
        //    triangles[ti] = vi;
        //    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
        //    triangles[ti + 4] = triangles[ti + 1] = vi + columns + 1;
        //    triangles[ti + 5] = vi + columns + 2;
        // 
        //    if( vi != 0 && (vi+2) % (columns+1) == 0)
        //    {
        //        ++vi;
        //    }
        //    
        //    //yield return wait;
        //
        //}




       int k = 0;
       for (int i = 0; i < (rows); ++i)
       {
           for (int j = 0; j < (columns); ++j)
           {
                int tempi = j + ((columns + 1) * i) ;
               int tempinextloop = j + (columns + 1) * (i + 1);
       
       
               triangles[k++] = tempi;

                triangles[k++] = tempinextloop;
                triangles[k++] = tempi + 1;
       
               triangles[k++] = tempinextloop;
               triangles[k++] = tempinextloop + 1;
               triangles[k++] = tempi + 1;
               Debug.Log(tempi);
               //Debug.Log(tempinextloop);
            }
       
       }


                //triangles[0] = 0;
                //triangles[1] = columns + 1;
                //triangles[2] = 1;
                //
                //
                //triangles[3] = columns + 1;
                //triangles[4] = columns + 2;
                //triangles[5] = 1;





        _mesh.triangles = triangles;

        _mesh.RecalculateNormals();

    }


  













}



























//int numIndicies = columns * rows * 6;
//int[] triangles = new int[numIndicies];
//for (int ti = 0, vi = 0; vi< (columns* (rows+1)); ++vi, ti += 6 )
//{
//    ///=================================================================
//    /// This is an indication of how our Tri's are composed
//    /// x----x----x----x----x----x
//    /// |\   |\   |\   |\   |\   |
//    /// | \  | \  | \  | \  | \  |
//    /// |  \ |  \ |  \ |  \ |  \ |
//    /// |   \|   \|   \|   \|   \|
//    /// x----x----x----x----x----x
//    /// With indices ordered as follows per each quad
//    /// 1----3 Indices are then ordered as 
//    /// |\   | [0,1,2,2,1,3] in the index buffer
//    /// | \  |
//    /// |  \ |
//    /// |   \|
//    /// 0----2
//
//    triangles[ti] = vi;
//    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
//    triangles[ti + 4] = triangles[ti + 1] = vi + columns + 1;
//    triangles[ti + 5] = vi + columns + 2;
// 
//    if( vi != 0 && (vi+2) % (columns+1) == 0)
//    {
//        ++vi;
//    }
//    _mesh.triangles = triangles;
//    yield return wait;
//
//}
//
//_mesh.RecalculateNormals();