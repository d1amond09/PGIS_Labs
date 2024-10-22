using System;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Buffer11 = SharpDX.Direct3D11.Buffer;
using Device11 = SharpDX.Direct3D11.Device;

namespace SimpleDXApp
{
	class Renderer : IDisposable
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct VertexDataStruct
		{
			public Vector4 position;
			public Vector4 color;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct PerObjectConstantBuffer
		{
			public Matrix worldViewProjectionMatrix;
			public float time;
			public int timeScaling;
			public Vector2 _padding;
		}

		private DirectX3DGraphics _directX3DGraphics;
		private Device11 _device;
		private DeviceContext _deviceContext;

		private VertexShader _vertexShader;
		private PixelShader _pixelShader;
		private ShaderSignature _shaderSignature;
		private InputLayout _inputLayout;

		private PerObjectConstantBuffer _perObjectConstantBuffer;
		private Buffer11 _perObjectConstantBufferObject;

		public Renderer(DirectX3DGraphics directX3DGraphics)
		{
			_directX3DGraphics = directX3DGraphics;
			_device = _directX3DGraphics.Device;
			_deviceContext = _directX3DGraphics.DeviceContext;

			CompilationResult vertexShaderByteCode =
				ShaderBytecode.CompileFromFile("vertex.hlsl",
				"vertexShader", "vs_5_0");
			_vertexShader = new VertexShader(_device, vertexShaderByteCode);
			CompilationResult pixelShaderByteCode =
				ShaderBytecode.CompileFromFile("pixel.hlsl",
				"pixelShader", "ps_5_0");
			_pixelShader = new PixelShader(_device, pixelShaderByteCode);

			InputElement[] inputElements = new[]
			{
				new InputElement("POSITION", 0, Format.R32G32B32A32_Float,
					0, 0),
				new InputElement("COLOR", 0, Format.R32G32B32A32_Float,
					16, 0)
			};

			_shaderSignature = ShaderSignature.GetInputSignature(vertexShaderByteCode);

			_inputLayout = new InputLayout(_device, _shaderSignature, inputElements);

			Utilities.Dispose(ref vertexShaderByteCode);
			Utilities.Dispose(ref pixelShaderByteCode);

			_deviceContext.InputAssembler.InputLayout = _inputLayout;
			_deviceContext.VertexShader.Set(_vertexShader);
			_deviceContext.PixelShader.Set(_pixelShader);
		}

		public void CreateConstantBuffer()
		{
			_perObjectConstantBufferObject = new Buffer11(
				_device,
				Utilities.SizeOf<PerObjectConstantBuffer>(),
				ResourceUsage.Dynamic,
				BindFlags.ConstantBuffer,
				CpuAccessFlags.Write,
				ResourceOptionFlags.None,
				0);
		}

		public void SetPerObjectConstantBuffer(float time, int timeScaling)
		{
			_perObjectConstantBuffer.time = time;
			_perObjectConstantBuffer.timeScaling = timeScaling;
		}

		public void BeginRender()
		{
			_directX3DGraphics.ClearBuffers(Color.Black);
		}

		public void UpdatePerObjectConstantBuffers(Matrix world, Matrix view,
			Matrix projection)
		{
			_perObjectConstantBuffer.worldViewProjectionMatrix =
				Matrix.Multiply(Matrix.Multiply(world, view), projection);
			_perObjectConstantBuffer.worldViewProjectionMatrix.Transpose();
			DataStream dataStream;
			_deviceContext.MapSubresource(
				_perObjectConstantBufferObject,
				MapMode.WriteDiscard,
				SharpDX.Direct3D11.MapFlags.None,
				out dataStream);
			dataStream.Write(_perObjectConstantBuffer);
			_deviceContext.UnmapSubresource(_perObjectConstantBufferObject, 0);
			_deviceContext.VertexShader.SetConstantBuffer(0, _perObjectConstantBufferObject);
		}

		public void RenderMeshObject(MeshObject meshObject)
		{
			_deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
			_deviceContext.InputAssembler.SetVertexBuffers(0,
				meshObject.VertexBufferBinding);
			_deviceContext.InputAssembler.SetIndexBuffer(meshObject.IndicesBufferObject,
				Format.R32_UInt, 0);
			_deviceContext.DrawIndexed(meshObject.IndicesCount, 0, 0);
		}

		public void EndRender()
		{
			_directX3DGraphics.SwapChain.Present(1, PresentFlags.Restart);
		}

		public void Dispose()
		{
			Utilities.Dispose(ref _perObjectConstantBufferObject);
			Utilities.Dispose(ref _inputLayout);
			Utilities.Dispose(ref _shaderSignature);
			Utilities.Dispose(ref _pixelShader);
			Utilities.Dispose(ref _vertexShader);
		}
	}
}
