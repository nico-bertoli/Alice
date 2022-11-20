using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldCell : MonoBehaviour
{
    //static
    public static int maxN;
    public static int maxM;
    public static int numCells = 0;
    public static int registeredCells = 0;

    //object
    public int N { get { return (int)transform.position.z; } }
    public int M { get { return (int)transform.position.x; } }
    public GameObject CurrentObject { get; set; }

    public int Height { get {
            return (int)transform.position.y;
    }}

    /// <summary>
    /// Used for setting player/enemy position on cell
    /// </summary>
    public Vector3 Position { get {
            return (new Vector3(M, Height + 0.5f, N));
    }}

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

    public override string ToString() {
        return ("[" + M + "," + N + "]");
    }
}
