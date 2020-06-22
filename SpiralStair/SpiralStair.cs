using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Rhino.Input;
/// <summary>
/// Position:定位功能，有基点和初始角属性
/// SpecsBase:楼梯规格基类，有内半径、宽度、旋转角度属性
/// SpiralSpecs:继承SpecsBase，还有总高、踏步高、踏步数量属性
/// StairMember:保存旋转楼梯构件的轴线、曲线、曲面
/// StairBase:/。p
/// </summary>
namespace SpiralStair
{
    /// <summary>
    /// 定位功能，有基点和初始角属性
    /// </summary>
    class Position
    {
        public Point3d BasePoint { get; set; }
        public double Angle { get; set; }

        public Position()
        {
        }
        public Position(Point3d pt, double angle)
        {
            BasePoint = pt;
            Angle = angle;
        }
        public Position(Position pos)
        {
            BasePoint = pos.BasePoint;
            Angle = pos.Angle;
        }
    }
    /// <summary>
    /// 螺旋楼梯的规格基类，有内半径、宽度、旋转角度属性
    /// </summary>
    class SpecsBase
    {
        public double InnerR { get; set; }
        public double Width { get; set; }
        public double RotateAngle { get; set; }
        public double Height { get; set; }
        public SpecsBase()
        {
            InnerR = 700;
            Width = 1500;
            RotateAngle = 90;
        }
        public SpecsBase(double innerR, double width, double rotateAngle)
        {
            InnerR = innerR;
            Width = width;
            RotateAngle = rotateAngle;
        }

    }

    class SpiralSpecs : SpecsBase
    {
        public double StepH { get; set; }
        public int StepCount { get; set; }
        public SpiralSpecs() : base()
        {
            StepH = 165;
            RotateAngle = 135;
            StepCount = 14;
            Height = StepH * StepCount;
        }
        public SpiralSpecs(double innerR, double width, double height, double rotateAngle, double stepH) : base(innerR, width, rotateAngle)
        {
            Height = height;
            StepH = stepH;
            StepCount = Convert.ToInt32(Height / StepH);

        }
        public SpiralSpecs(double innerR, double width, double rotateAngle, double stepH, int stepCount) : base(innerR, width, rotateAngle)
        {
            StepH = stepH;
            StepCount = stepCount;
            Height = StepH * StepCount;
        }
    }
    class StairMember
    {
        public Polyline InnerAxis { get; set; }
        public Curve InnerCurve { get; set; }
        public List<Brep> InnerBeam { get; set; }

        public Polyline OutAxis { get; set; }
        public Curve OutCurve { get; set; }
        public List<Brep> OutBeam { get; set; }
        public List<Line> StepAxis { get; set; }
        public StairMember()
        {

        }
    }
    /// <summary>
    /// 旋转楼梯基类，具有构件、起始定位、终点定位属性
    /// </summary>
    abstract class StairBase
    {
        StairMember stairMember = new StairMember();
        Position strPosition = new Position(Point3d.Origin, 0);
        Position endPosition = new Position();
        SpecsBase partSpecs = new SpecsBase();
        public Position StartPst
        {
            get { return strPosition; }
            set { strPosition = value; }
        }
        public Position EndPst
        {
            get { return endPosition; }
            set { endPosition = value; }
        }
        public StairMember StairMembers
        {
            get { return stairMember; }
            set { stairMember = value; }
        }
        public SpecsBase PartSpecs
        {
            get { return partSpecs; }
            set { partSpecs = value; }
        }
        protected StairBase()
        {
        }
        protected StairBase(Position strPst)
        {
            StartPst = strPst;
        }
        public void SetEndPst()
        {
            Point3d pt = StartPst.BasePoint;
            EndPst = new Position(new Point3d(pt.X, pt.Y, pt.Z+ partSpecs.Height), StartPst.Angle+partSpecs.RotateAngle);
        }
        public virtual void GenerateGeom()
        { }
    }
    class SpiralPart : StairBase
    {
        public SpiralPart() : base()
        {
            PartSpecs = new SpiralSpecs();
        }
        public SpiralPart(Position strPosition, SpiralSpecs spriralProp) : base(strPosition)
        {
            PartSpecs = spriralProp;
        }


        public override void GenerateGeom()
        {
            SpiralSpecs spcs = (SpiralSpecs)PartSpecs;
            List<Point3d> innerPts = CreatPoints(this.StartPst,spcs, this.PartSpecs.InnerR);
            List<Point3d> outPts = CreatPoints(this.StartPst, spcs, spcs.InnerR + spcs.Width);
            StairMembers.InnerCurve = Curve.CreateInterpolatedCurve(innerPts, 3);
            StairMembers.OutCurve = Curve.CreateInterpolatedCurve(outPts, 3);
            StairMembers.InnerAxis = new Polyline(innerPts);
            StairMembers.OutAxis = new Polyline(outPts);
            List<Line> stepAxis = new List<Line>();
            for (int i = 0; i < innerPts.Count; i++)
            {
                stepAxis.Add(new Line(innerPts[i], outPts[i]));
            }
            StairMembers.StepAxis = stepAxis;
        }
        private static List<Point3d> CreatPoints(Position strPst, SpiralSpecs stairProp, double radius)
        {
            List<Point3d> plist = new List<Point3d>();
            int stepCount = stairProp.StepCount;
            double stepAngle = stairProp.RotateAngle / stepCount;
            Point3d basePt = strPst.BasePoint;
            double strAngle = strPst.Angle;
            for (int i = 0; i < stepCount + 1; i++)
            {
                double angleRotate = (strAngle + i * stepAngle) * Math.PI / 180;
                double x = basePt.X + radius * Math.Cos(angleRotate);
                double y = basePt.Y + radius * Math.Sin(angleRotate);
                double z = basePt.Z + i * stairProp.StepH;
                Point3d pt = new Point3d(x, y, z);
                plist.Add(pt);
            }
            return plist;
        }
    }
    class PlatFormPart : StairBase
    {

        public PlatFormPart() : base()
        {
            PartSpecs = new SpecsBase();

        }
        public PlatFormPart(Position strPst, SpecsBase platFormSpecs) : base(strPst)
        {
            PartSpecs = platFormSpecs;
        }
        public override void GenerateGeom()
        {
            double strAngle = (StartPst.Angle) * Math.PI / 180;//弧线起始角度
            double rotAngle = (PartSpecs.RotateAngle) * Math.PI / 180;//弧线旋转角度

            Plane p1 = new Plane(StartPst.BasePoint, Vector3d.ZAxis);
            p1.Rotate(strAngle, Vector3d.ZAxis);
            Arc ar1 = new Arc(p1, PartSpecs.InnerR, rotAngle);
            Arc ar2 = new Arc(p1, PartSpecs.InnerR + PartSpecs.Width, rotAngle);
            StairMembers.InnerCurve = ar1.ToNurbsCurve();
            StairMembers.OutCurve = ar2.ToNurbsCurve();
        }
    }
}
