using System;
using SharpDX;

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
					color = new Vector4(1.0f, 1.0f, 0.0f, 1.0f)
				},
				new Renderer.VertexDataStruct // bottom left 1
                {
					position = new Vector4(-1.0f, -1.0f, -1.0f, 1.0f),
					color = new Vector4(0.0f, 1.0f, 1.0f, 1.0f)
				},
				new Renderer.VertexDataStruct // bottom right 2
                {
					position = new Vector4(1.0f, -1.0f, -1.0f, 1.0f),
					color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f)
				},
				new Renderer.VertexDataStruct // bottom back 3
                {
					position = new Vector4(0.0f, -1.0f, 1.0f, 1.0f),
					color = new Vector4(1.0f, 0.0f, 1.0f, 1.0f)
				}
			};

			uint[] indices = new uint[12]
			{
				0, 1, 2,
				0, 2, 3,
				0, 3, 1,
				1, 2, 3
			};

			return new MeshObject(_directX3DGraphics, position, yaw, pitch, roll, vertices, indices);
		}

		public void Dispose()
		{
			// Освобождение ресурсов
		}
	}
}
