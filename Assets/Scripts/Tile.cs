using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originColor;

    public Color OriginColor { get => originColor; set => originColor = value; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }

    private void Start() {
        this.SpriteRenderer = GetComponent<SpriteRenderer>();
        this.OriginColor = this.SpriteRenderer.color;
    }
}
