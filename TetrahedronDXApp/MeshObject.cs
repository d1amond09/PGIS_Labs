using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D11;
using Buffer11 = SharpDX.Direct3D11.Buffer;

namespace SimpleDXApp
{
    class MeshObject : Game3DObject, IDisposable
    {
        private DirectX3DGraphics _directX3DGraphics;

        private int _verticesCount;
        private Renderer.VertexDataStruct[] _vertices;
        private Buffer11 _vertexBufferObject;
        private VertexBufferBinding _vertexBufferBinding;
        public VertexBufferBinding VertexBufferBinding { get => _vertexBufferBinding; }

        private int _indicesCount;
        public int IndicesCount { get => _indicesCount; }
        private uint[] _indices;
        private Buffer11 _indicesBufferObject;
        public Buffer11 IndicesBufferObject { get => _indicesBufferObject; }

        public MeshObject(DirectX3DGraphics directX3DGraphics,
            Vector4 position, float yaw, float pitch, float roll,
            Renderer.VertexDataStruct[] vertices, uint[] indices)
            : base(position, yaw, pitch, roll)
        {
            _directX3DGraphics = directX3DGraphics;
            _vertices = vertices;
            _verticesCount = _vertices.Length;
            _indices = indices;
            _indicesCount = _indices.Length;

            _vertexBufferObject = Buffer11.Create(
                _directX3DGraphics.Device,
                BindFlags.VertexBuffer,
                _vertices,
                Utilities.SizeOf<Renderer.VertexDataStruct>() * _verticesCount);
            _vertexBufferBinding = new VertexBufferBinding(
                _vertexBufferObject,
                Utilities.SizeOf<Renderer.VertexDataStruct>(),
                0);
            _indicesBufferObject = Buffer11.Create(
                _directX3DGraphics.Device,
                BindFlags.IndexBuffer,
                _indices,
                Utilities.SizeOf<uint>() * _indicesCount);
        }

        public void Dispose()
        {
            Utilities.Dispose(ref _indicesBufferObject);
            Utilities.Dispose(ref _vertexBufferObject);
        }
    }
}
