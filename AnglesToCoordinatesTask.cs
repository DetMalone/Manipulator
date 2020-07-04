using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            Func<PointF, float, double, PointF> JointPosition = (O, l, phi) =>
                new PointF(O.X + l * (float)Math.Cos(phi), O.Y + l * (float)Math.Sin(phi));
            var elbowPos = JointPosition(new PointF(), Manipulator.UpperArm, shoulder);
            var wristPos = JointPosition(elbowPos, Manipulator.Forearm, elbow + shoulder - Math.PI);
            var palmEndPos = JointPosition(wristPos, Manipulator.Palm, wrist + elbow + shoulder - 2 * Math.PI);
            return new PointF[]
            {
                elbowPos,
                wristPos,
                palmEndPos
            };
        }
    }
    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
        }

    }

}