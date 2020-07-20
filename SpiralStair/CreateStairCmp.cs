using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace SpiralStair
{
    public class CreateStairCmp : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateStairCmp class.
        /// </summary>
        public CreateStairCmp()
          : base("CreateStair", "CreateStair",
              "创建旋转楼梯模型，包含曲面、曲线、线段",
              "Stair", "生成模型")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("StairParts", "StairParts", "螺旋楼梯各个部件", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Surfaces", "Srf", "楼梯曲面", GH_ParamAccess.list);
            pManager.AddCurveParameter("Curves", "Crv", "楼梯曲线", GH_ParamAccess.list);
            pManager.AddLineParameter("Lines", "lines", "楼梯线段", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<StairBase> StairGroups = new List<StairBase>();
            DA.GetDataList(0, StairGroups);
            List<Curve> crvs = new List<Curve>();
            List<Line> steps = new List<Line>();
            List<Polyline> beamLines = new List<Polyline>();
            for (int i = 0; i < StairGroups.Count; i++)
            {
                StairGroups[i].GenerateGeom();
                StairMember mi = StairGroups[i].StairMembers;
                crvs.Add(mi.InnerCurve);
                crvs.Add(mi.OutCurve);
                if (StairGroups[i].GetType() == typeof(SpiralPart))
                {
                    steps.AddRange(mi.StepAxis);
                }
                beamLines.Add(mi.InnerAxis);
                beamLines.Add(mi.OutAxis);
            }
            DA.SetDataList(1, crvs);
            DA.SetDataList(2, steps);

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
            get { return new Guid("a2e748df-4020-494e-b5f6-8c36b194f87e"); }
        }
    }
}