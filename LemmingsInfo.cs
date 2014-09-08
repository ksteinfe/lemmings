using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Lemmings
{
    public class LemmingsProjectInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Lemmings";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("400dd316-b5a3-415b-8ced-b75f71e79f6d");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Kyle Steinfeld";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
