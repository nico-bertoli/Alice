using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCell : MonoBehaviour
{
    public static int maxN;
    public static int maxM;

    private void Awake() {
            if (N > maxN) maxN = N;
            if (M > maxM) maxM = M;
    }

    private void Start() {
        WorldGrid.Instance.RegisterCell(this);
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
