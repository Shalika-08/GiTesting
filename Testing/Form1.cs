using System;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace Testing
{
    public partial class Form1 : Form
    {
        private static readonly string serverUrl = "http://localhost:5000/api/pdf/upload";
        public Form1()
        {
            InitializeComponent();
        }

        

       
    }
}
