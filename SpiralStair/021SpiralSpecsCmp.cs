using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace SpiralStair
{
    public class SpiralSpecsCmp : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SpiralSpecsCmp class.
        /// </summary>
        public SpiralSpecsCmp()
          : base("SpiralParameters", "Parameters",
              "螺旋段楼梯规格属性，主要包含内半径、宽度、旋转角度、踏步高度、踏步数量",
              "Stair", "01_Define")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Radius", "Radius", "螺旋楼梯内半径", GH_ParamAccess.item,300);
            pManager.AddNumberParameter("Width", "Width", "螺旋楼梯宽度", GH_ParamAccess.item,3000);
            pManager.AddNumberParameter("Angle", "Angle", "螺旋楼梯总旋转角度", GH_ParamAccess.item,286);
            pManager.AddNumberParameter("Height", "Height", "每个踏步高度", GH_ParamAccess.item,150);
            pManager.AddIntegerParameter("Counts", "Counts", "踏步的数量", GH_ParamAccess.item,26);
            pManager.AddBooleanParameter("Direction", "Dir", "旋转方向，true为逆时针,false为顺时针", GH_ParamAccess.item,false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SpriralDimension", "Dimension", "螺旋段楼梯主要尺寸", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Double radius = new double();
            Double width = new double();
            Double angle = new double();
            Double height = new double();
            int counts = new int();
            Boolean boolDir = true;
            DA.GetData(0, ref radius);
            DA.GetData(1, ref width);
            DA.GetData(2, ref angle);
            DA.GetData(3, ref height);
            DA.GetData(4, ref counts);
            DA.GetData(5, ref boolDir);
            SpiralSpecs dimension = new SpiralSpecs(radius, width, angle, height, counts,boolDir);
            DA.SetData(0, dimension);

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("c49aff5e-5991-426f-a6c9-a840e2336a94"); }
        }
    }
}