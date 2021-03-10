
void clip_float(float3 planePosition, float3 pos, out float1 alpha)
{
    if (pos.x < 0 || pos.z < 0 || pos.x > planePosition.x || pos.z > planePosition.z)
    {
        alpha = 0;
    }
    alpha = 1;
}