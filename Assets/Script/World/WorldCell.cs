using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCell : MonoBehaviour
{
    public static int maxN;
    public static int maxM;
    public static int numCells = 0;
    public static int registeredCells = 0;

    private void Awake() {
            if (N > maxN) maxN = N;
            if (M > maxM) maxM = M;
            numCells++;
    }

    private void Start() {
        Debug.Log("num cells: " + numCells);
        registeredCells++;
        WorldGrid.Instance.RegisterCell(this);
        Debug.Log("registered cells: " + numCells);
    }

    public int N { get { return (int)transform.position.z; } }
    public int M { get { return (int)transform.position.x; } }
    public int Height { get {
            return (int)transform.position.y;
    }}

    public Vector3 Position { get {
            return (new Vector3(M, Height + 0.5f, N));
    }}
}
