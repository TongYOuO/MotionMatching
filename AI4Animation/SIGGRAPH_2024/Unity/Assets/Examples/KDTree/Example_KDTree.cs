﻿using UnityEngine;

public class Example_KDTree : MonoBehaviour {

    public int PointCount = 10000;
    public int Neighbours = 10;
    public float Min = -5f;
    public float Max = 5f;
    public float Size = 0.1f;
    public bool DrawPointCloud = true;
    public bool DrawNeighbours = true;
    public bool DrawPivot = true;
    public bool LinearSearch = false;
    public double ProcessingTime = 0.0;

    private KDTree.KDTree<Vector3> KDTree = null;
    private Vector3[] Points = null;

    void Start() {
        KDTree = new KDTree.KDTree<Vector3>(3);
        Points = new Vector3[PointCount];
        for(int i=0; i<PointCount; i++) {
            Vector3 point = new Vector3(Random.Range(Min, Max), Random.Range(Min, Max), Random.Range(Min, Max));
            KDTree.AddPoint(new double[3]{point.x, point.y, point.z}, point);
            Points[i] = point;
        }
    }

    void OnRenderObject() {
        UltiDraw.Begin();
        if(DrawPointCloud) {
            foreach(Vector3 point in Points) {
                UltiDraw.DrawSphere(point, Quaternion.identity, Size, Color.black);
            }
        }

        if(DrawPivot) {
            UltiDraw.DrawSphere(transform.position, Quaternion.identity, Size, Color.red);
        }

        if(DrawNeighbours) {
            if(LinearSearch) {
                System.DateTime timestamp = Utility.GetTimestamp();
                Vector3 pivot = transform.position;
                Vector3 closest = Points[0];
                float distance = Vector3.Distance(pivot, closest);
                for(int i=1; i<Points.Length; i++) {
                    float d = Vector3.Distance(pivot, Points[i]);
                    if(d < distance) {
                        closest = Points[i];
                        distance = d;
                    }
                }
                ProcessingTime = Utility.GetElapsedTime(timestamp);
                UltiDraw.DrawSphere(closest, Quaternion.identity, 2f*Size, Color.green);
            } else {
                System.DateTime timestamp = Utility.GetTimestamp();
                KDTree.NearestNeighbour<Vector3> neighbours = KDTree.NearestNeighbors(new double[3]{transform.position.x, transform.position.y, transform.position.z}, Neighbours);
                ProcessingTime = Utility.GetElapsedTime(timestamp);
                foreach(Vector3 neighbour in neighbours) {
                    UltiDraw.DrawSphere(neighbour, Quaternion.identity, 2f*Size, Color.green);
                }
            }
        }
        UltiDraw.End();
    }

}