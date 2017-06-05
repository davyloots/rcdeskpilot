using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.DirectX;

namespace RCSim.DataClasses
{
    class AirplaneState
    {
        public Vector3 Position;
        public Vector3 Orientation;
        public double Rudder;
        public double Throttle;
        public double Elevator;
        public double Ailerons;
        public bool Smoke;
        public bool Flaps;
        public bool Gear;
        public bool OnWater;

        public void Write(BinaryWriter writer)
        {
            writer.Write(Position.X);
            writer.Write(Position.Y);
            writer.Write(Position.Z);
            writer.Write(Orientation.X);
            writer.Write(Orientation.Y);
            writer.Write(Orientation.Z);
            writer.Write((short)Math.Round(Rudder*100));
            writer.Write((short)Math.Round(Throttle * 100));
            writer.Write((short)Math.Round(Elevator * 100));
            writer.Write((short)Math.Round(Ailerons * 100));
            writer.Write((byte)(((OnWater ? 1: 0) << 3) |
                ((Flaps ? 1 : 0) << 2) | 
                ((Gear ? 1 : 0) << 1) | 
                (Smoke ? 1: 0)));
        }

        public void Read(BinaryReader reader)
        {
            Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Orientation = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Rudder = (double)(reader.ReadInt16() / 100.0);
            Throttle = (double)(reader.ReadInt16() / 100.0);
            Elevator = (double)(reader.ReadInt16() / 100.0);
            Ailerons = (double)(reader.ReadInt16() / 100.0);
            byte switches = reader.ReadByte();
            Smoke = (switches & 1) > 0;
            Gear = (switches & 2) > 0;
            Flaps = (switches & 4) > 0;
            OnWater = (switches & 8) > 0;
        }
    }
}
