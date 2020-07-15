namespace NRKernal.NRExamples
{
    using NRKernal;
    using System.Collections.Generic;
    using UnityEngine;

    public class PolygonPlaneVisualizer : NRTrackableBehaviour
    {
        public Material Material;
        private MeshRenderer m_Renderer;
        private MeshFilter m_Filter;
        private MeshCollider m_Collider;
        List<Vector3> m_PosList = new List<Vector3>();
        Mesh m_PlaneMesh = null;

#if UNITY_EDITOR
        public void GetBoundaryPolygon(Transform tran, List<Vector3> polygonList)
        {
            polygonList.Clear();

            float[] point_data = new float[8];
            point_data[0] = 1;
            point_data[1] = 1;
            point_data[2] = 2;
            point_data[3] = -2;
            point_data[4] = -3;
            point_data[5] = -3;
            point_data[6] = -4;
            point_data[7] = 4;
            Pose centerPos = new Pose(tran.position, tran.rotation);
            var unityWorldTPlane = Matrix4x4.TRS(tran.position, tran.rotation, Vector3.one);
            for (int i = point_data.Length - 2; i >= 0; i -= 2)
            {
                Vector3 localpos = new Vector3(point_data[i], 0, point_data[i + 1]);
                polygonList.Add(unityWorldTPlane.MultiplyPoint3x4(localpos));
            }
        }
#endif

        private void Update()
        {
#if UNITY_EDITOR
            var center = new Pose(transform.position, transform.rotation);
            GetBoundaryPolygon(transform, m_PosList);
#else
            var center = Trackable.GetCenterPose();
            ((NRTrackablePlane)Trackable).GetBoundaryPolygon(m_PosList);
#endif
            this.DrawFromCenter(center, m_PosList);
#if !UNITY_EDITOR
            if (Trackable.GetTrackingState() == TrackingState.Stopped)
            {
                Destroy(gameObject);
            }
#endif
        }

        private void DrawFromCenter(Pose centerPose, List<Vector3> vectors)
        {
            if (vectors == null || vectors.Count == 0)
            {
                return;
            }
            var center = centerPose.position;
            Vector3[] vertices3D = new Vector3[vectors.Count + 1];
            vertices3D[0] = transform.InverseTransformPoint(center);
            for (int i = 1; i < vectors.Count + 1; i++)
            {
                vertices3D[i] = transform.InverseTransformPoint(vectors[i - 1]);
            }

            int[] triangles = GenerateTriangles(m_PosList);

            if (m_PlaneMesh == null)
            {
                m_PlaneMesh = new Mesh();
            }
            m_PlaneMesh.vertices = vertices3D;
            m_PlaneMesh.triangles = triangles;

            if (m_Renderer == null)
            {
                m_Renderer = gameObject.GetComponent<MeshRenderer>();
                m_Renderer.material = Material;
            }

            m_Renderer.material.SetVector("_PlaneNormal", centerPose.up);
            if (m_Filter == null)
            {
                m_Filter = gameObject.GetComponent<MeshFilter>();
            }
            m_Filter.mesh = m_PlaneMesh;

            if (m_Collider == null)
            {
                m_Collider = gameObject.GetComponent<MeshCollider>();
            }
            m_Collider.sharedMesh = m_PlaneMesh;
        }

        private int[] GenerateTriangles(List<Vector3> posList)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < posList.Count; i++)
            {
                if (i != posList.Count - 1)
                {
                    indices.Add(0);
                    indices.Add(i + 1);
                    indices.Add(i + 2);
                }
                else
                {
                    indices.Add(0);
                    indices.Add(i + 1);
                    indices.Add(1);
                }
            }
            return indices.ToArray();
        }
    }
}