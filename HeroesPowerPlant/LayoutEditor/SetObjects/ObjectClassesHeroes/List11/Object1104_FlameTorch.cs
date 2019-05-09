﻿using SharpDX;

namespace HeroesPowerPlant.LayoutEditor
{
    public enum StartMode
    {
        Lit = 0,
        LitOnRange = 1,
        Unlit = 2
    }

    public class Object1104_FlameTorch : SetObjectManagerHeroes
    {
        public override void CreateTransformMatrix(Vector3 Position, Vector3 Rotation)
        {
            this.Position = Position;
            this.Rotation = Rotation;

            transformMatrix = IsUpsideDown ? Matrix.RotationY(MathUtil.Pi) : Matrix.Identity *
                Matrix.Scaling(Scale + 1f) *
                Matrix.RotationX(ReadWriteCommon.BAMStoRadians((int)Rotation.X)) *
                Matrix.RotationY(ReadWriteCommon.BAMStoRadians((int)Rotation.Y)) *
                Matrix.RotationZ(ReadWriteCommon.BAMStoRadians((int)Rotation.Z)) *
                Matrix.Translation(Position);
        }

        public override void Draw(SharpRenderer renderer, string[] modelNames, bool isSelected)
        {
            if (BaseType == BaseTypeEnum.None)
                DrawCube(renderer, isSelected);
            else if (BaseType == BaseTypeEnum.Floor)
            {
                Draw(renderer, "S11_ON_FIREA_BASE.DFF", isSelected);
                if (IsBlue)
                    Draw(renderer, "S11_ON_FIREA_BLUE.DFF", isSelected);
                else
                    Draw(renderer, "S11_ON_FIREA_RED.DFF", isSelected);
            }
            else if (BaseType == BaseTypeEnum.Air)
            {
                Draw(renderer, "S11_ON_FIREB_BASE.DFF", isSelected);
                if (IsBlue)
                    Draw(renderer, "S11_ON_FIREB_BLUE.DFF", isSelected);
                else
                    Draw(renderer, "S11_ON_FIREB_RED.DFF", isSelected);
            }
        }

        public bool IsBlue
        {
            get => ReadInt(4) != 0;
            set => Write(4, value ? 1 : 0);
        }

        public StartMode StartMode
        {
            get => (StartMode)ReadInt(8);
            set => Write(8, (int)value);
        }

        public float Range
        {
            get => ReadFloat(12);
            set => Write(12, value);
        }

        public float Scale
        {
            get => ReadFloat(16);
            set { Write(16, value); CreateTransformMatrix(); }
        }

        public bool IsUpsideDown
        {
            get => ReadByte(20) != 0;
            set => Write(20, (byte)(value ? 1 : 0));
        }

        public enum BaseTypeEnum
        {
            None = 0,
            Floor = 1,
            Air = 2
        }
        public BaseTypeEnum BaseType
        {
            get => (BaseTypeEnum)ReadByte(21);
            set => Write(21, (byte)value);
        }

        public bool HasSE
        {
            get => ReadByte(22) != 0;
            set => Write(22, (byte)(value ? 1 : 0));
        }

        public bool HasCollision
        {
            get => ReadByte(23) != 0;
            set => Write(23, (byte)(value ? 1 : 0));
        }
    }
}
