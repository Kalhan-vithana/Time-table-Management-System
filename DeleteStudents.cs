using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Time_Table_managemnt
{
    public partial class DeleteStudents : MaterialForm
    {
        readonly MaterialSkin.MaterialSkinManager materialskinmanager;
        public DeleteStudents()
        {
            InitializeComponent();
            materialskinmanager = MaterialSkin.MaterialSkinManager.Instance;
            materialskinmanager.EnforceBackcolorOnAllComponents = true;
            materialskinmanager.AddFormToManage(this);
            materialskinmanager.Theme = MaterialSkin.MaterialSkinManager.Themes.LIGHT;
            materialskinmanager.ColorScheme = new MaterialSkin.ColorScheme(MaterialSkin.Primary.Indigo500, MaterialSkin.Primary.Indigo700, MaterialSkin.Primary.Indigo100, MaterialSkin.Accent.Pink200, MaterialSkin.TextShade.WHITE);


        }
        
        private void DeleteStudents_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Visible = false;
        }
    }
}
