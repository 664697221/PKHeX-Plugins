﻿using System;
using System.Windows.Forms;
using QRPlugins;

namespace AutoModPlugins
{
    public class ExportQRCodes : AutoModPlugin
    {
        public override string Name => "Export QR Codes";
        public override int Priority => 1;

        protected override void AddPluginControl(ToolStripDropDownItem modmenu)
        {
            var ctrl = new ToolStripMenuItem(Name) { Image = QRResources.exportqrcode };
            ctrl.Click += ExportQRs;
            modmenu.DropDownItems.Add(ctrl);
        }

        private void ExportQRs(object? sender, EventArgs e)
        {
            var sav = SaveFileEditor.SAV;
            if (!sav.HasBox)
            {
                WinFormsUtil.Error("Save file does not have box data.");
                return;
            }
            var boxData = sav.GetBoxData(sav.CurrentBox);
            QRCodeDumper.DumpQRCodes(boxData);
        }
    }
}
