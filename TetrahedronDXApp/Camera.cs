using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace SimpleDXApp
{
    class Camera : Game3DObject
    {
        private float _fovY;
        public float FOVY { get => _fovY; set => _fovY = value; }

        private float _aspect;
        public float Aspect { get => _aspect; set => _aspect = value; }
		public Vector3 ViewTo { get; private set; }
		public Vector3 ViewRight { get; private set; }
		public Vector3 ViewUp { get; private set; }
		public Camera(Vector4 position,
            float yaw = 0.0f, float pitch = 0.0f, float roll = 0.0f,
            float fovY = MathUtil.PiOverTwo, float aspect = 1.0f)
            : base (position, yaw, pitch, roll)
        {
            _fovY = fovY;
			_aspect = aspect;
        }
		
		public void MoveForward(float distance)
		{
			_position += new Vector4(ViewTo, 0f) * distance;
		}
		public void MoveRight(float distance)
		{
			_position += new Vector4(ViewRight, 0f) * distance;
		}

		public Matrix GetProjectionMatrix()
        {
            return Matrix.PerspectiveFovLH(_fovY, _aspect, 0.1f, 100.0f);
        }

		public Matrix GetViewMatrix()
        {
            Matrix rotation = Matrix.RotationYawPitchRoll(_yaw, _pitch, _roll);
			ViewTo = (Vector3)Vector4.Transform(Vector4.UnitZ, rotation);
            ViewUp = (Vector3)Vector4.Transform(Vector4.UnitY, rotation);
			ViewRight = Vector3.Cross(ViewTo, ViewUp);

			return Matrix.LookAtLH((Vector3)_position,
                (Vector3)_position + ViewTo, ViewUp);
        }

		public override void YawBy(float deltaYaw)
		{
			_yaw = deltaYaw;
			LimitAngleByPlusMinusPi(ref _yaw);
		}

		public override void PitchBy(float deltaPitch)
		{
			_pitch = deltaPitch;
			LimitAngleByPlusMinusPi(ref _pitch);
		}

		public override void RollBy(float deltaRoll)
		{
			_roll = deltaRoll;
			LimitAngleByPlusMinusPi(ref _roll);
		}
	}
}
