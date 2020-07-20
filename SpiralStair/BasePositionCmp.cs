using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace SpiralStair
{
    public class BasePositionCmp : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the BasePointCmp class.
        /// </summary>
        public BasePositionCmp()
          : base("BasePoint", "Base",
              "起始基点定位，主要定义起点和起始角度",
              "Stair", "参数")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("BasePoint", "BasePoint", "起始点", GH_ParamAccess.item,new Point3d(0,0,0));
            pManager.AddNumberParameter("BaseAngle", "BaseAngle", "起始角度", GH_ParamAccess.item,0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("StartPosition", "Position", "起始位置", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d p1 = new Point3d(0,0,0);
            Boolean bool1 = DA.GetData(0, ref p1);
            Double ang = 0;
            Boolean bool2 = DA.GetData(1, ref ang);
            Position posi = new Position(p1, ang);
            DA.SetData(0, posi);

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
            get { return new Guid("659bc189-86d6-4047-832a-a4e911acfefd"); }
        }
    }
}