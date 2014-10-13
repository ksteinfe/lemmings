using System;
using System.Windows.Forms;
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
    public abstract class NameableComponent : GH_Component  {
        protected String m_name;
        public String ComponentName {
            get { return m_name; }
            set {
                m_name = value;
                Message = m_name;
            }
        }
        //protected abstract void NameSet(String newName);

        protected String nameLetter;
        protected String namePrefix;
        public bool customNameSet;

        public NameableComponent(String name, String nickname, String description)
            : base(name, nickname,
                description,
                "Params", "Lemmings") {
            this.customNameSet = false;
            this.nameLetter = "X";
        }

        private void SetUniqueNameletter(GH_Document document) {
            // finds other NameableComponents in current document
            List<String> existingNames = new List<String>();
            Guid componentid = this.ComponentGuid;
            foreach (IGH_DocumentObject docobj in document.Objects) if (docobj.ComponentGuid == componentid) existingNames.Add(((NameableComponent)docobj).nameLetter);

            this.nameLetter = GH_ComponentParamServer.InventUniqueNickname("abcdefghijklmnopqrstuv", existingNames);
            if (!customNameSet) this.ComponentName = this.namePrefix + this.nameLetter;
        }

        public override void AddedToDocument(GH_Document document) {
            base.AddedToDocument(document);
            SetUniqueNameletter(document);
        }
        public override void MovedBetweenDocuments(GH_Document oldDocument, GH_Document newDocument) {
            base.MovedBetweenDocuments(oldDocument, newDocument);
            SetUniqueNameletter(newDocument);
        }

        public override bool Write(GH_IO.Serialization.GH_IWriter writer) {
            // First add our own field.
            if (this.customNameSet) writer.SetString("ComponentName", this.ComponentName);
            // Then call the base class implementation.
            return base.Write(writer);
        }
        public override bool Read(GH_IO.Serialization.GH_IReader reader) {
            // First read our own field.
            if (reader.ItemExists("ComponentName")) {
                this.customNameSet = true;
                this.ComponentName = reader.GetString("ComponentName");
            }
            // Then call the base class implementation.
            return base.Read(reader);
        }

        public override bool AppendMenuItems(ToolStripDropDown menu) {
            // Place a call to the base class to ensure the default parameter menu is still there and operational.
            base.AppendAdditionalComponentMenuItems(menu);

            // Now insert your own custom menu items.
            //Menu_AppendSeparator(menu);
            Menu_AppendTextItem(menu, this.ComponentName, Menu_ParentLayerNameKeyDown, Menu_ParentLayerNameChanged, true);
            //Menu_AppendItem(menu, "Run Lemmings!", Menu_RunLemmingsClicked);
            return true;
        }

        private void Menu_ParentLayerNameKeyDown(Object sender, EventArgs e) { Menu_ParentLayerNameChanged(sender, ((Grasshopper.GUI.GH_MenuTextBox)sender).Text); }
        public void Menu_ParentLayerNameChanged(Object sender, string text) {
            text = text.Replace(' ', '_');
            StringBuilder sb = new StringBuilder();
            foreach (char c in text) {
                if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || c == '_') {
                    sb.Append(c);
                }
            }
            text = sb.ToString();

            if (text.Length == 0) {
                this.customNameSet = false;
                this.ComponentName = this.namePrefix + this.nameLetter;
                ExpirePreview(true);
                ExpireSolution(true);
                return;
            }
            this.ComponentName = text;
            this.customNameSet = true;
            ExpirePreview(true);
            ExpireSolution(true);
        }

    }
}
