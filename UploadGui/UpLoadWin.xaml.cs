﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;

namespace UploadGui
{
    /// <summary>
    /// Interaction logic for UpLoadWin.xaml
    /// </summary>
    public partial class UpLoadWin : Window
    {
        //BackgroundWorker worker = new BackgroundWorker();


        public UpLoadWin()
        {
            InitializeComponent();
            this.DataContext = new UpLoadWinViewModel();
        }
    }


}