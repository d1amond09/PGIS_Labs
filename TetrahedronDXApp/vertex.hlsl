struct vertexData
{
    float4 position : POSITION;
    float4 color    : COLOR; 
};

struct pixelData
{
    float4 position : SV_POSITION;
    float4 color    : COLOR;
};

cbuffer perObjectData : register(b0)
{
    float4x4 worldViewProjectionMatrix; 
    float    time; 
    int      timeScaling;
    float2   _padding;
}

float4 CalculateColor(float y)
{
    return float4(y, 0.0f, 1.0f - y, 1.0f);
}

pixelData vertexShader(vertexData input)
{
    pixelData output = (pixelData)0;
    float4 position = input.position;

    float distance = sqrt(position.x * position.x + position.z * position.z); 
    float y = sin(distance); 

    //y = max(0.0f, min(y, 1.0f)); 

    position.y = y;

    output.color = CalculateColor(y); 

    float scale = 0.5f * sin(time * 0.785f) * 0 + 1.0f; 
    if (timeScaling > 0)
        position.xyz *= scale; 

    output.position = mul(position, worldViewProjectionMatrix);

    return output; 
}