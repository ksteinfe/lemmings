/*
    if (go){
      IGH_Param param = Component.Params.Input[1].Sources[0];
      Grasshopper.Kernel.Parameters.Param_Mesh mesh_param = (Grasshopper.Kernel.Parameters.Param_Mesh) param;
      Print(mesh_param.Name);
      mesh_param.BakeGeometry(RhinoDocument, new List<Guid>());
     
      Rhino.RhinoApp.RunScript("SelAll", true);
      Rhino.RhinoApp.RunScript("-Export testX.obj _Enter _Enter", true);
      Rhino.RhinoApp.RunScript("Delete", true);
      //app.RunScript("_-export " & sFileName & "_enter _enter");

      //Grasshopper.Kernel.Special.GH_NumberSlider slider = (Grasshopper.Kernel.Special.GH_NumberSlider) Component.Params.Input[0].Sources[0];
      //Print(slider.Slider.Value.ToString());
    }
*/