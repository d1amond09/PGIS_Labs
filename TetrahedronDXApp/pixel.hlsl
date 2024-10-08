struct pixelData
{
	float4 position : SV_POSITION;
    float4 color : COLOR;
};

float4 pixelShader(pixelData input) : SV_Target
{
    return input.color;
}