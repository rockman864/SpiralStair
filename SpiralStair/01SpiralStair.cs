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
/// StairBase:
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
            BasePoint = new Point3d(0, 0, 0);
            Angle = 0;
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
    /// 尺寸基类，有内半径、宽度、旋转角属性
    /// </summary>
    class BaseSpecs
    {
        int dirMultiple;
        public double InnerR { get; set; }
        public double Width { get; set; }
        public double RotateAngle { get; set; }
        public Boolean Direction { get; set; }
        public int DirMultiple
        {
            get
            {
                if (Direction == true)
                {
                    dirMultiple = 1;
                }
                else
                {
                    dirMultiple = -1;
                }
                return dirMultiple;
            }
        }
        public BaseSpecs()
        {
            InnerR = 700;
            Width = 1500;
            RotateAngle = 360;
            Direction = true;
        }
        public BaseSpecs(double innerR, double width,double angle,Boolean dir)
        {
            InnerR = innerR;
            Width = width;
            RotateAngle = angle;
            Direction = dir;

        }

    }

    /// <summary>
    /// 平台段规格，扩展BaseSpecs
    /// </summary>
    class PlatformSpecs:BaseSpecs
    {
        public PlatformSpecs():base()
        {
        }
        public PlatformSpecs(double innerR = 700, double width = 1500, double rotateAngle = 90,Boolean dir = true):base(innerR,width,rotateAngle,dir)
        {
        }
    }
    /// <summary>
    /// 螺旋段规格类，，扩展BaseSpecs,除了规格基类的属性（内半径、宽度、旋转角度）外，还有踏步高度、踏步数量、总高度属性
    /// </summary>

    class SpiralSpecs : BaseSpecs
    {
        public double StepH { get; set; }
        public int StepCount { get; set; }
        public double Height { get; set; }
        public SpiralSpecs() : base()
        {
            StepH = 160;
            RotateAngle = 360;
            StepCount = 25;
            Height = StepH * StepCount;
        }

        public SpiralSpecs(double innerR = 700, double width = 1500, double rotateAngle = 360, double stepH = 160, int stepCount = 25,Boolean dir=true) : base(innerR, width, rotateAngle,dir)
        {
            StepH = stepH;
            StepCount = stepCount;
            Height = StepH * StepCount;
        }
    }
    /// <summary>
    /// 楼梯构件类，主要有以下属性：内主梁几何体（曲面、曲线、多段线）、外主梁几何体（曲面、曲线、多段线）、踏步线段
    /// </summary>
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
    /// 旋转楼梯基类，具有属性：构件、起始定位、终点定位、部件规格，具有方法：设置终点定位、生成构件
    /// </summary>
    abstract class StairBase
    {
        StairMember stairMember = new StairMember();
        Position strPosition = new Position(Point3d.Origin, 0);
        Position endPosition = new Position();
        PlatformSpecs platformDimension = new PlatformSpecs();
        SpiralSpecs spiralDimension = new SpiralSpecs();
        public Position StartPst
        {
            get { return strPosition; }
            set { strPosition = value; }
        }
        public Position EndPst
        {
            get
            {
                return endPosition;
            }
            set
            {
                endPosition = value;
            }
        }
        public StairMember StairMembers
        {
            get { return stairMember; }
            set { stairMember = value; }
        }
        public PlatformSpecs PlatformDimesion
        {
            get { return platformDimension; }
            set { platformDimension = value; }
        }
        public SpiralSpecs SpiralDimension
        {
            get { return spiralDimension; }
            set { spiralDimension = value; }
        }
        protected StairBase()
        {
        }
        protected StairBase(Position strPst, SpiralSpecs spirDim)
        {
            StartPst = strPst;
            SpiralDimension = spirDim;
        }
        protected StairBase(Position strPst, PlatformSpecs platDim)
        {
            StartPst = strPst;
            PlatformDimesion = platDim;
        }
        public virtual void SetEndPst()
        {
        }
        public virtual void GenerateGeom()
        { }
    }
    /// <summary>
    /// 螺旋部分的楼梯，扩展StairBase,主要是重写了生成构件方法
    /// </summary>
    class SpiralPart : StairBase
    {
        public SpiralPart() : base()
        {
        }
        /// <summary>
        /// 螺旋段构造函数，主要使用起点参数和螺旋段规格参数
        /// </summary>
        /// <param name="strPosition"></param>
        /// <param name="spiralDimension">螺旋段规格参数：包含内半径、宽度、旋转角度、踏步高度、踏步数量</param>
        public SpiralPart(Position strPosition, SpiralSpecs spiralDimension) : base(strPosition, spiralDimension)
        {
            SetEndPst();
        }
        public override void SetEndPst()
        {
            Point3d pt = StartPst.BasePoint;
            EndPst = new Position(new Point3d(pt.X, pt.Y, pt.Z + SpiralDimension.Height), StartPst.Angle + SpiralDimension.DirMultiple*SpiralDimension.RotateAngle);
        }

        /// <summary>
        /// 构件生成方法，主要生成主构件曲线、多段线、踏步线段；曲面生成后续添加
        /// </summary>
        public override void GenerateGeom()
        {
            List<Point3d> innerPts = CreatPoints(this.StartPst, SpiralDimension, SpiralDimension.InnerR);
            List<Point3d> outPts = CreatPoints(this.StartPst, SpiralDimension, SpiralDimension.InnerR + SpiralDimension.Width);
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
        private static List<Point3d> CreatPoints(Position strPst, SpiralSpecs dimension, double radius)
        {
            List<Point3d> plist = new List<Point3d>();
            int stepCount = dimension.StepCount;
            double stepAngle = dimension.RotateAngle / stepCount;
            Point3d basePt = strPst.BasePoint;
            double strAngle = strPst.Angle;
            for (int i = 0; i < stepCount + 1; i++)
            {
                double angleRotate = (strAngle + dimension.DirMultiple* i * stepAngle) * Math.PI / 180;
                double x = basePt.X + radius * Math.Cos(angleRotate);
                double y = basePt.Y + radius * Math.Sin(angleRotate);
                double z = basePt.Z + i * dimension.StepH;
                Point3d pt = new Point3d(x, y, z);
                plist.Add(pt);
            }
            return plist;
        }
    }
    /// <summary>
    /// 平台段类，主要重写构件生成方法。
    /// </summary>
    class PlatFormPart : StairBase
    {

        public PlatFormPart() : base()
        {

        }
        /// <summary>
        /// 平台端构造函数，主要使用起点参数和平台规格参数
        /// </summary>
        /// <param name="strPst">起点参数，主要包含起点和角度</param>
        /// <param name="platFormSpecs">平台规格参数，主要包含内半径、宽度、旋转角度</param>
        public PlatFormPart(Position strPst, PlatformSpecs platFormSpecs) : base(strPst, platFormSpecs)
        {
            SetEndPst();
        }
        public override void SetEndPst()
        {
            Point3d pt = StartPst.BasePoint;
            EndPst = new Position(new Point3d(pt.X, pt.Y, pt.Z), StartPst.Angle + PlatformDimesion.DirMultiple* PlatformDimesion.RotateAngle);
        }
        /// <summary>
        /// 平台段几何生成方法，目前可以生成主梁曲线和线段
        /// </summary>
        public override void GenerateGeom()
        {
            double strAngle = (StartPst.Angle) * Math.PI / 180;//弧线起始角度
            double rotAngle = PlatformDimesion.DirMultiple*(PlatformDimesion.RotateAngle) * Math.PI / 180;//弧线旋转角度
            int segs =(int) Math.Round(Math.Abs(rotAngle) / (10 * Math.PI / 180));
            Plane p1 = new Plane(StartPst.BasePoint, Vector3d.ZAxis);
            p1.Rotate(strAngle, Vector3d.ZAxis);
            Arc ar1 = new Arc(p1, PlatformDimesion.InnerR, rotAngle);
            Arc ar2 = new Arc(p1, PlatformDimesion.InnerR + PlatformDimesion.Width, rotAngle);
            StairMembers.InnerCurve = ar1.ToNurbsCurve();
            StairMembers.OutCurve = ar2.ToNurbsCurve();
            Point3d[] ptsIn, ptsOut;
            StairMembers.InnerCurve.DivideByCount(segs, true, out ptsIn);
            StairMembers.OutCurve.DivideByCount(segs, true, out ptsOut);
            StairMembers.InnerAxis = new Polyline(ptsIn);
            StairMembers.OutAxis = new Polyline(ptsOut);

        }
    }
}
