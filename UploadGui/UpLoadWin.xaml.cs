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
using UploadGui.Models;
using UploadGui.ViewModels;

namespace UploadGui
{
    /// <summary>
    /// Interaction logic for UpLoadWin.xaml
    /// </summary>
    public partial class UpLoadWin : Window
    {

        public UpLoadWin(Title sTitle)
        {
            InitializeComponent();
            this.DataContext = new UpLoadWinViewModel(sTitle);
           
        }
    }



}