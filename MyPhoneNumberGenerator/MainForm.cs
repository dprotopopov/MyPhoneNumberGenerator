using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyPhoneNumberGenerator
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
     {
        private AboutBox aboutBox = new AboutBox();
        private SetupForm setupForm = new SetupForm();
        public MainForm()
        {
            InitializeComponent();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.aboutBox.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (setupForm.ShowDialog() == DialogResult.OK)
            {
                ChildForm childForm = new ChildForm(this.setupForm.phoneMask.Text, Convert.ToInt32(this.setupForm.phoneNumberLength.Value));
                childForm.MdiParent = this;
                childForm.Show();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChildForm childForm = this.ActiveMdiChild as ChildForm;
            if (childForm != null && this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                childForm.SaveAs(this.saveFileDialog1.FileName);
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.aboutBox.ShowDialog();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (setupForm.ShowDialog() == DialogResult.OK)
            {
                ChildForm childForm = new ChildForm(this.setupForm.phoneMask.Text, Convert.ToInt32(this.setupForm.phoneNumberLength.Value));
                childForm.MdiParent = this;
                childForm.Show();
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ChildForm childForm = this.ActiveMdiChild as ChildForm;
            if (childForm != null && this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                childForm.SaveAs(this.saveFileDialog1.FileName);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

    }
}
