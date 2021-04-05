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
    public partial class Home : MaterialForm
    {
        readonly MaterialSkin.MaterialSkinManager materialskinmanager;
        public Home()
        {
            InitializeComponent();
            materialskinmanager = MaterialSkin.MaterialSkinManager.Instance;
            materialskinmanager.EnforceBackcolorOnAllComponents = true;
            materialskinmanager.AddFormToManage(this);
            materialskinmanager.Theme = MaterialSkin.MaterialSkinManager.Themes.LIGHT;
            materialskinmanager.ColorScheme = new MaterialSkin.ColorScheme(MaterialSkin.Primary.Indigo500, MaterialSkin.Primary.Indigo700, MaterialSkin.Primary.Indigo100, MaterialSkin.Accent.Pink200, MaterialSkin.TextShade.WHITE);


            materialTabControl1.SelectedIndex = 2;
        }
        

        private void Home_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            AddStudent std = new AddStudent();
            std.Show();
            this.Visible = false;
        }
    }
}
