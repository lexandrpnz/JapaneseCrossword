using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using JCClasses;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace JapaneseСrossword
{
    public partial class SudocuForm : Form
    {
        public SudocuForm()
        {
            InitializeComponent();
        }


        public void LoadSudocu(String Path)
        {
            _sudocuControl.LoadSudocu(Path);
        }

        public void SaveSudocu(String Path)
        {
            _sudocuControl.SaveSudocu(Path);
        }

    }
}





