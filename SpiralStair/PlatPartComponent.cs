using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace SpiralStair
{
    public class PlatPartComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PlatPart class.
        /// </summary>
        public PlatPartComponent()
          : base("PlatPart", "Nickname",
              "平台段",
              "Category", "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("beamCurve", "crv", "主梁弧线", GH_ParamAccess.list);
            pManager.AddLineParameter("StepLines", "step", "踏步轴线", GH_ParamAccess.list);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            SpiralPart prt1 = new SpiralPart();//第一段
            PlatFormPart prt2 = new PlatFormPart();//第二段
            SpiralPart prt3 = new SpiralPart();//第三段
            PlatFormPart prt4 = new PlatFormPart();//第四段
            SpiralPart prt5 = new SpiralPart();//第五段
            List<StairBase> StairGroups = new List<StairBase> { prt1,prt2,prt3,prt4,prt5};

            List<Curve> crvs = new List<Curve>();
            List<Line> steps = new List<Line>();
            for (int i=0;i<StairGroups.Count;i++)
            {
                if (i>0)
                {
                    StairGroups[i].StartPst = StairGroups[i - 1].EndPst;
                }
                StairGroups[i].SetEndPst();
                StairGroups[i].GenerateGeom();
                StairMember mi = StairGroups[i].StairMembers;
                crvs.Add(mi.InnerCurve);
                crvs.Add(mi.OutCurve);
                if(StairGroups[i].GetType()==typeof(SpiralPart))
                {
                    steps.AddRange(mi.StepAxis);
                }
            }
            DA.SetDataList(0, crvs);
            DA.SetDataList(1, steps);



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
            get { return new Guid("e3efbf8e-2c89-4c26-bc04-d2422093ddb8"); }
        }
    }
}