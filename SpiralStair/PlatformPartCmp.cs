using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace SpiralStair
{
    public class PlatformPartCmp : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PlatformPartCmp class.
        /// </summary>
        public PlatformPartCmp()
          : base("PlatformPart", "Platform",
              "螺旋楼梯平台段",
              "Stair", "分段")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("StartPosition", "StartPosition", "平台段起点", GH_ParamAccess.item);
            pManager.AddGenericParameter("Dimension", "Dimension", "平台规格尺寸", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SpiralPart", "SpiralPart", "平台段", GH_ParamAccess.item);
            pManager.AddGenericParameter("EndPosition", "EndPosition", "平台段终点", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Position pos = new Position();
            DA.GetData(0, ref pos);
            SpecsBase spcs = new SpecsBase();
            DA.GetData(1, ref spcs);
            PlatFormPart platform = new PlatFormPart(pos, spcs);
            DA.SetData(0, platform);
            DA.SetData(1, platform.EndPst);
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
            get { return new Guid("ff7dd75b-1423-4444-8642-cd90f57aa803"); }
        }
    }
}