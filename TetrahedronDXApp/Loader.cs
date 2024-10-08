using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.D3DCompiler;
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

		public MeshObject MakeSineSurface(Vector4 position, float yaw, float pitch, float roll, int resolution)
		{
			int vertexCount = resolution * resolution;
			Renderer.VertexDataStruct[] vertices = new Renderer.VertexDataStruct[vertexCount];
			List<uint> indices = new List<uint>();

			float size = 100f;
			float halfSize = size / 2.0f;

			for (int i = 0; i < resolution; i++)
			{
				for (int j = 0; j < resolution; j++)
				{
					float x = ((float)i / (resolution - 1)) * size - halfSize; 
					float z = ((float)j / (resolution - 1)) * size - halfSize; 

					vertices[i * resolution + j] = new Renderer.VertexDataStruct
					{
						position = new Vector4(x, 0f, z, 1.0f),
						color = new Vector4(0f, 0f, 0f, 1.0f) 
					};

					// Индексы для каркасной модели
					if (i < resolution - 1 && j < resolution - 1)
					{
						uint a = (uint)(i * resolution + j);
						uint b = (uint)(i * resolution + (j + 1));
						uint c = (uint)((i + 1) * resolution + j);
						uint d = (uint)((i + 1) * resolution + (j + 1));

						indices.Add(a);
						indices.Add(b);
						indices.Add(c);
						indices.Add(b);
						indices.Add(d);
						indices.Add(c);
					}
				}
			}

			uint[] indexArray = indices.ToArray();

			return new MeshObject(_directX3DGraphics, position, yaw, pitch, roll, vertices, indexArray);
		}

		public void Dispose()
		{
			// Освобождение ресурсов
		}
	}
}
