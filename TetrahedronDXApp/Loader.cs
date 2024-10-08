using System;
using SharpDX;
using SharpDX.Direct3D11;

namespace SimpleDXApp
{
	class Loader : IDisposable
	{
		private DirectX3DGraphics _directX3DGraphics;

		public Loader(DirectX3DGraphics directX3DGraphics)
		{
			_directX3DGraphics = directX3DGraphics;
		}

		public MeshObject MakeTetrahedron(Vector4 position, float yaw, float pitch, float roll)
		{
			Renderer.VertexDataStruct[] vertices = new Renderer.VertexDataStruct[4]
			{
				new Renderer.VertexDataStruct // top 0
                {
					position = new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
					texCoord = new Vector2(0.0f, 0.0f)
				},
				new Renderer.VertexDataStruct // bottom left 1
                {
					position = new Vector4(-1.0f, -1.0f, -1.0f, 1.0f),
					texCoord = new Vector2(0.5f, 1.0f)
				},
				new Renderer.VertexDataStruct // bottom right 2
                {
					position = new Vector4(1.0f, -1.0f, -1.0f, 1.0f),
					texCoord = new Vector2(1.0f, 0.0f)
				},
				new Renderer.VertexDataStruct // bottom back 3
                {
					position = new Vector4(0.0f, -1.0f, 1.0f, 1.0f),
					texCoord = new Vector2(0.5f, 0.5f)
				}
			};

			uint[] indices = new uint[12]
			{
				0, 1, 2,
				0, 2, 3,
				0, 3, 1,
				1, 2, 3
			};

			Texture2D texture = TextureLoader.LoadTexture(_directX3DGraphics.Device, @"img/tiger.jfif");
			ShaderResourceView textureView = new ShaderResourceView(_directX3DGraphics.Device, texture);
			var samplerDescription = new SamplerStateDescription()
			{
				Filter = Filter.MinMagMipLinear, // Линейная фильтрация
				AddressU = TextureAddressMode.Mirror, // Зацикливание текстуры по оси U
				AddressV = TextureAddressMode.Mirror, // Зацикливание текстуры по оси V
				AddressW = TextureAddressMode.Mirror, // Зацикливание текстуры по оси W
				ComparisonFunction = Comparison.Never,
				MinimumLod = 0,
				MaximumLod = float.MaxValue
			};
			var samplerState = new SamplerState(_directX3DGraphics.Device, samplerDescription);
			_directX3DGraphics.DeviceContext.PixelShader.SetShaderResources(0, textureView);
			_directX3DGraphics.DeviceContext.PixelShader.SetSampler(0, samplerState);
			return new MeshObject(_directX3DGraphics, position,
				yaw, pitch, roll, vertices, indices);
		}

		public void Dispose()
		{
			// Освобождение ресурсов
		}
	}
}
