using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace SpiralStair
{
    public class SpiralPartCmp : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public SpiralPartCmp()
          : base("SpiralPart", "Spiral",
              "螺旋楼梯踏步段",
              "Stair", "分段")
        {
        }

        /// <summary>
        /// 主要包含两个输入参数：起点定位、螺旋段规格
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("StartPosition", "StartPosition", "旋转楼梯螺旋段起点", GH_ParamAccess.item);
            pManager.AddGenericParameter("Dimension", "Dimension", "螺旋段规格尺寸", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)

        {
            pManager.AddGenericParameter("SpiralPart", "SpiralPart", "旋转楼梯螺旋段", GH_ParamAccess.item);
            pManager.AddGenericParameter("EndPosition", "EndPosition", "旋转楼梯螺旋段终点", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Position basePosi = new Position();
            SpiralSpecs dimension = new SpiralSpecs();
            SpiralPart p1 = new SpiralPart();
            if (DA.GetData(0, ref basePosi)&& DA.GetData(1, ref dimension))
            {
                p1 = new SpiralPart(basePosi, dimension);
            }
            DA.SetData(0, p1);
            DA.SetData(1, p1.EndPst);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("e2435208-b363-4086-a74b-808c80a154e3"); }
        }
    }
}
