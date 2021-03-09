
void transparency_float(float1 areaCond, float1 transparencyCond, out float1 transparencyOutput)
{
    if (areaCond > 0.95)
    {
        transparencyOutput = transparencyCond;
    }
    else
    {
        transparencyOutput = 1;
    }
}