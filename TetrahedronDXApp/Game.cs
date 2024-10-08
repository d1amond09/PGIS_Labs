using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Windows;

namespace SimpleDXApp
{
    class Game : IDisposable
    {
        private RenderForm _renderForm;

        private MeshObject _cube;
        private Camera _camera;

        private DirectX3DGraphics _directX3DGraphics;
        private Renderer _renderer;

        private TimeHelper _timeHelper;

        public Game()
        {
            _renderForm = new RenderForm();
            _renderForm.UserResized += RenderFormResizedCallback;
            _directX3DGraphics = new DirectX3DGraphics(_renderForm);
            _renderer = new Renderer(_directX3DGraphics);
            _renderer.CreateConstantBuffer();

            Loader loader = new Loader(_directX3DGraphics);
            _cube = loader.MakeSineSurface(new Vector4(0.0f, 0.0f, 0.0f, 1.0f), 0.0f, 0.0f, 0.0f, 2000);
            _camera = new Camera(new Vector4(0.0f, 0.0f, -16.0f, 0.0f));
            _timeHelper = new TimeHelper();
            loader.Dispose();
            loader = null;
        }

        public void RenderFormResizedCallback(object sender, EventArgs args)
        {
            _directX3DGraphics.Resize();
            _camera.Aspect = _renderForm.ClientSize.Width /
                (float)_renderForm.ClientSize.Height;
        }

        private bool _firstRun = true;

        public void RenderLoopCallback()
        {
            if (_firstRun)
            {
                RenderFormResizedCallback(this, EventArgs.Empty);
                _firstRun = false;
            }
            _timeHelper.Update();
            _renderForm.Text = "FPS: " + _timeHelper.FPS.ToString();
            _cube.YawBy(Cursor.Position.X/100f);
            //_cube.RollBy(_timeHelper.DeltaT * MathUtil.TwoPi * 0.05f);
            _cube.PitchBy(Cursor.Position.Y / 100f);

            Matrix viewMatrix = _camera.GetViewMatrix();
            Matrix projectionMatrix = _camera.GetProjectionMatrix();

            _renderer.BeginRender();

            _renderer.SetPerObjectConstantBuffer(_timeHelper.Time, 1);

            _renderer.UpdatePerObjectConstantBuffers(_cube.GetWorldMatrix(),
                viewMatrix, projectionMatrix);
            _renderer.RenderMeshObject(_cube);

            _renderer.EndRender();
        }

        public void Run()
        {
            RenderLoop.Run(_renderForm, RenderLoopCallback);
        }

        public void Dispose()
        {
            _cube.Dispose();
            _renderer.Dispose();
            _directX3DGraphics.Dispose();
        }
    }
}
