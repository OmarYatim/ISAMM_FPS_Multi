using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayersPositions : MonoBehaviour {

    #region Private List
    List<Vector3> Positions;
    #endregion

    #region Private Params
    Vector3 Firstpos;
    #endregion

    #region MonoBehaviour
    void Start () {
        Positions = new List<Vector3>();
        Reader(Positions);
        Firstpos = Positions[0];
        Positions.RemoveAt(0);
        foreach (Vector3 p in Positions)
        {
            Debug.DrawRay(Firstpos, p - Firstpos, Color.red, Mathf.Infinity);
            Firstpos = p;
        }
    }

    #endregion

    #region Static Methods
    public static void Reader(List<Vector3> Positions)
    {
        using (var reader = new StreamReader(@"C:\Users\Omar Yatim\Desktop\UnityProjects\ISAMMFPSMulti\server2\out.csv"))
        {
            reader.ReadLine();
            while(!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                Vector3 position = new Vector3(float.Parse(values[1]),float.Parse(values[2]),float.Parse(values[3]));
                Positions.Add(position);
            }
        }
    }
    #endregion
}
