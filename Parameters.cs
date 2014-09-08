using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;

using Rhino.Geometry;

namespace Lemmings
{

    public class LemmingsIntParameter : GH_Param<GH_Integer>
    {
        public LemmingsIntParameter() :
            base(new GH_InstanceDescription("Lemmings Integer", "LInt",
                                          "Integer for Lemmings Batch Runs",
                                          "Params", "Lemmings")) { 
        }

        public override System.Guid ComponentGuid { get { return new Guid("{97CE5B82-DA1C-4339-8624-CEB548112EC5}"); } }


        public String toJSON() {
            return "-99";
        }

        //public override void CreateAttributes() { m_attributes = new LemmingsIntParameterAttributes(this); }

    }

    public class LemmingsIntParameterAttributes : GH_Attributes<LemmingsIntParameter>
    {
        public LemmingsIntParameterAttributes(LemmingsIntParameter owner) : base(owner) { }

        protected override void Layout()
        {
            // Compute the width of the NickName of the owner (plus some extra padding), 
            // then make sure we have at least 80 pixels.
            int width = GH_FontServer.StringWidth(Owner.NickName, GH_FontServer.Standard);
            width = Math.Max(width + 10, 80);

            // The height of our object is always 60 pixels
            int height = 60;

            // Assign the width and height to the Bounds property.
            // Also, make sure the Bounds are anchored to the Pivot
            Bounds = new RectangleF(Pivot, new SizeF(width, height));
        }

        public override void ExpireLayout()
        {    
          base.ExpireLayout();

          // Destroy any data you have that becomes 
          // invalid when the layout expires.
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel){
          // Render all the wires that connect the Owner to all its Sources.
          if (channel == GH_CanvasChannel.Wires)
          {
            RenderIncomingWires(canvas.Painter, Owner.Sources, Owner.WireDisplay);
            return;
          }

          // Render the parameter capsule and any additional text on top of it.
          if (channel == GH_CanvasChannel.Objects)
          {
            // Define the default palette.
            GH_Palette palette = GH_Palette.Normal;

            // Adjust palette based on the Owner's worst case messaging level.
            switch (Owner.RuntimeMessageLevel)
            {
              case GH_RuntimeMessageLevel.Warning:
                palette = GH_Palette.Warning;
                break;

              case GH_RuntimeMessageLevel.Error:
                palette = GH_Palette.Error;
                break;
             }

            // Create a new Capsule without text or icon.
            GH_Capsule capsule = GH_Capsule.CreateCapsule(Bounds, palette);

            // Render the capsule using the current Selection, Locked and Hidden states.
            // Integer parameters are always hidden since they cannot be drawn in the viewport.
            capsule.Render(graphics, Selected, Owner.Locked, true);

            // Always dispose of a GH_Capsule when you're done with it.
            capsule.Dispose();
            capsule = null;

            // Now it's time to draw the text on top of the capsule.
            // First we'll draw the Owner NickName using a standard font and a black brush.
            // We'll also align the NickName in the center of the Bounds.
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            format.Trimming = StringTrimming.EllipsisCharacter;

            // Our entire capsule is 60 pixels high, and we'll draw 
            // three lines of text, each 20 pixels high.
            RectangleF textRectangle = Bounds;
            textRectangle.Height = 20;

            // Draw the NickName in a Standard Grasshopper font.
            graphics.DrawString(Owner.NickName, GH_FontServer.Standard, Brushes.Black, textRectangle, format);


            // Now we need to draw the median and mean information.
            // Adjust the formatting and the layout rectangle.
            format.Alignment = StringAlignment.Near;
            textRectangle.Inflate(-5, 0);

            textRectangle.Y += 20;
            graphics.DrawString(String.Format("Median: {0}", Owner.Name),
                                GH_FontServer.StandardItalic, Brushes.Black,
                                textRectangle, format);

            textRectangle.Y += 20;
            graphics.DrawString(String.Format("Mean: {0:0.00}", Owner.Name),
                                GH_FontServer.StandardItalic, Brushes.Black,
                                textRectangle, format);

            // Always dispose of any GDI+ object that implement IDisposable.
            format.Dispose();
          }
        }


    }


    /*
    public class LemmingsIntParameter : Grasshopper.Kernel.Parameters.Param_Integer
    {
      // We need to supply a constructor without arguments that calls the base class constructor.
        public LemmingsIntParameter() : base("TriState", "Tri", "Represents a collection of TriState values", "Params", "Primitive") { }

      public override System.Guid ComponentGuid
      {
        // Always generate a new Guid, but never change it once 
        // you've released this parameter to the public.
        get { return new Guid("{97CE5B82-DA1C-4339-8624-CEB548112EC5}"); }
      }
    }
     */


    /*
    public class LemmingsIntParameter2 : GH_PersistentParam<GH_Integer>
    {

      // We need to supply a constructor without arguments that calls the base class constructor.
        public LemmingsIntParameter2() : 
        base("TriState", "Tri", "Represents a collection of TriState values", "Params", "Primitive") { }

      public override System.Guid ComponentGuid
      {
        // Always generate a new Guid, but never change it once 
        // you've released this parameter to the public.
        get { return new Guid("{97CE5B82-DA1C-4339-8624-CEB548112EC5}"); }
      }

      protected override GH_GetterResult Prompt_Singular(ref GH_Integer value)
      {
        return GH_GetterResult.cancel;
      }
      protected override GH_GetterResult Prompt_Plural(ref List<GH_Integer> values)
      {
        return GH_GetterResult.cancel;
      }
    }
    */
}
