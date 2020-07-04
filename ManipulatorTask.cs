using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class ManipulatorTask
    {
        public class Vector
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Lenght { get => Math.Sqrt(X * X + Y * Y); }
            public double Angle
            {
                get => Math.Atan2(Y, X) > 0 ? Math.Atan2(Y, X) : 2 * Math.PI + Math.Atan2(Y, X);
            }
        }

        public static double[] MoveManipulatorTo(double x, double y, double alpha)
        {
            var wristVector = new Vector() { X = x - Manipulator.Palm * Math.Cos(alpha),
                                             Y = y + Manipulator.Palm * Math.Sin(alpha) };
            var palmEndVector = new Vector() { X = x, Y = y };
            int overlay = palmEndVector.Angle < wristVector.Angle ? 1 : -1;
            if (Math.Abs(palmEndVector.Angle - wristVector.Angle) > Math.PI) overlay *= -1;

            var elbow = TriangleTask.GetABAngle(Manipulator.UpperArm, Manipulator.Forearm, wristVector.Lenght);
            var wrist = (1 - overlay) * Math.PI +
                        overlay * TriangleTask.GetABAngle(Manipulator.Palm, wristVector.Lenght, palmEndVector.Lenght) +
                        TriangleTask.GetABAngle(Manipulator.Forearm, wristVector.Lenght, Manipulator.UpperArm);
            var shoulder = -alpha - wrist - elbow;

            return new[] { shoulder, elbow, wrist };
        }
    }
}