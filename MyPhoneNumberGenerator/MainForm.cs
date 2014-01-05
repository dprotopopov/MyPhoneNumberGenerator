using System;
using System.Windows.Forms;

namespace MyPhoneNumberGenerator
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
     {
        private readonly AboutBox _aboutBox = new AboutBox();
        private readonly SetupForm _setupForm = new SetupForm();
        public MainForm()
        {
            InitializeComponent();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _aboutBox.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_setupForm.ShowDialog() == DialogResult.OK)
            {
                ChildForm childForm = new ChildForm(_setupForm.phoneMask.Text, Convert.ToInt32(_setupForm.phoneNumberLength.Value))
                {
                    MdiParent = this
                };
                childForm.Show();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChildForm childForm = ActiveMdiChild as ChildForm;
            if (childForm != null && saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                childForm.SaveAs(saveFileDialog1.FileName);
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _aboutBox.ShowDialog();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_setupForm.ShowDialog() == DialogResult.OK)
            {
                ChildForm childForm = new ChildForm(_setupForm.phoneMask.Text, Convert.ToInt32(_setupForm.phoneNumberLength.Value))
                {
                    MdiParent = this
                };
                childForm.Show();
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ChildForm childForm = ActiveMdiChild as ChildForm;
            if (childForm != null && saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                childForm.SaveAs(saveFileDialog1.FileName);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

    }
}
