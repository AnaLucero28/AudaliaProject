using DevExpress.Utils.Win;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication
{
    public partial class CurtainForm : DevExpress.XtraEditors.XtraForm
    {
        public CurtainForm()
        {
            InitializeComponent();
            pictureEdit.StartAnimation();
        }        
    }
}