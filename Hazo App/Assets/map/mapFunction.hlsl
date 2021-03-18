
void border_float(float3 pos, float4 area, out float alpha)
{
    if (pos.x <= area.x || pos.x >= area.w)
    {
        alpha = 0;
        return;
    }
    if (pos.y <= area.y || pos.y >= area.z)
    {
        alpha = 0;
        return;
    }
    alpha = 1;
}