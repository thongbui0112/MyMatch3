using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDot {
    int originCol, originRow;
    int targetCol, targetRow;

    public int OriginCol { get => originCol; set => originCol = value; }
    public int OriginRow { get => originRow; set => originRow = value; }
    public int TargetCol { get => targetCol; set => targetCol = value; }
    public int TargetRow { get => targetRow; set => targetRow = value; }

    public MoveDot(int originCol, int originRow, int targetCol, int targetRow) {
        this.OriginCol = originCol;
        this.OriginRow = originRow;
        this.TargetCol = targetCol;
        this.TargetRow = targetRow;

    }
    public MoveDot() {

    }
}
