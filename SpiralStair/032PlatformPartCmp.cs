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
              "Stair", "02_Part")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PreviousPart", "PrePart", "前一段", GH_ParamAccess.item);
            pManager.AddNumberParameter("RotateAngle", "Angle", "平台规格尺寸", GH_ParamAccess.item,90);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PlatformPart", "PlatformPart", "平台段", GH_ParamAccess.item);
            pManager.AddGenericParameter("EndPostion", "EndPosition", "平台段末段定位", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            SpiralPart part1 = new SpiralPart();
            Position pos;
            PlatformSpecs spcs = new PlatformSpecs();
            double angle = 0;

            DA.GetData(0, ref part1);
            DA.GetData(1, ref angle);
            //获得前一个部件的属性，然后分配给这个部件
            pos = part1.EndPst;
            var partDim = part1.SpiralDimension;
            spcs.InnerR = partDim.InnerR;
            spcs.Width = partDim.Width;
            spcs.RotateAngle = angle;
            spcs.Direction = partDim.Direction;
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