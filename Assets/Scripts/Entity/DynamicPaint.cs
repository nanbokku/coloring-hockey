using UnityEngine;

public class DynamicPaint : MonoBehaviour
{
    [SerializeField]
    private Renderer paintRenderer = null;
    [SerializeField]
    private Collider paintCollider = null;
    private MaterialPropertyBlock materialBlock = null;

    private Vector4[] drawWorldPosition1 = new Vector4[1000];
    private Vector4[] drawWorldPosition2 = new Vector4[1000];
    private int currentWorldPosition1 = 0;
    private int currentWorldPosition2 = 0;

    private int drawWorldPosition1Id = 0;
    private int drawWorldPosition2Id = 0;
    private int currentWorldPosition1Id = 0;
    private int currentWorldPosition2Id = 0;

    void Awake()
    {
        drawWorldPosition1Id = Shader.PropertyToID("_DrawWorldPosition1");
        drawWorldPosition2Id = Shader.PropertyToID("_DrawWorldPosition2");
        currentWorldPosition1Id = Shader.PropertyToID("_CurrentWorldPosition1");
        currentWorldPosition2Id = Shader.PropertyToID("_CurrentWorldPosition2");

        materialBlock = new MaterialPropertyBlock();

        Clear();
    }

    public Vector3 ClosestPoint(Vector3 position)
    {
        return paintCollider.ClosestPoint(position);
    }

    public void AddDrawPoint(Vector3 point, PlayerType type)
    {
        Vector4 point4 = point;

        if (type == PlayerType.Human)
        {
            drawWorldPosition1[currentWorldPosition1] = point4;
            currentWorldPosition1++;

            materialBlock.SetVectorArray(drawWorldPosition1Id, drawWorldPosition1);
            materialBlock.SetInt(currentWorldPosition1Id, currentWorldPosition1);
        }
        else if (type == PlayerType.Ai)
        {
            drawWorldPosition2[currentWorldPosition2] = point4;
            currentWorldPosition2++;

            materialBlock.SetVectorArray(drawWorldPosition2Id, drawWorldPosition2);
            materialBlock.SetInt(currentWorldPosition2Id, currentWorldPosition2);
        }

        paintRenderer.SetPropertyBlock(materialBlock);
    }

    public void Clear()
    {
        drawWorldPosition1 = new Vector4[2000];
        drawWorldPosition2 = new Vector4[2000];
        currentWorldPosition1 = 0;
        currentWorldPosition2 = 0;

        materialBlock.SetVectorArray(drawWorldPosition1Id, drawWorldPosition1);
        materialBlock.SetVectorArray(drawWorldPosition2Id, drawWorldPosition2);
        materialBlock.SetInt(currentWorldPosition1Id, currentWorldPosition1);
        materialBlock.SetInt(currentWorldPosition2Id, currentWorldPosition2);

        paintRenderer.SetPropertyBlock(materialBlock);
    }
}