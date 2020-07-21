using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace SpiralStair
{
    public class PlatformSpecsCmp : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PlatformSpecsCmp class.
        /// </summary>
        public PlatformSpecsCmp()
          : base("PlatformParameters", "Parameters",
              "休息平台尺寸,主要包含内半径、宽度、旋转角度",
              "Stair", "01_Define")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Radius", "Radius", "螺旋楼梯内半径", GH_ParamAccess.item, 700);
            pManager.AddNumberParameter("Width", "Width", "螺旋楼梯宽度", GH_ParamAccess.item, 1500);
            pManager.AddNumberParameter("Angle", "Angle", "休息平台总旋转角度", GH_ParamAccess.item, 90);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PlatformDimension", "Dimension", "休息平台主要尺寸", GH_ParamAccess.item);
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
            DA.GetData(0, ref radius);
            DA.GetData(1, ref width);
            DA.GetData(2, ref angle);
            PlatformSpecs dimension = new PlatformSpecs(radius, width, angle);
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
            get { return new Guid("f6db7513-476f-48e9-a094-e43051db45ed"); }
        }
    }
}