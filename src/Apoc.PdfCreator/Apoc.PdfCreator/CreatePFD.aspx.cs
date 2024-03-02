using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml.html;

namespace Apoc.PdfCreator
{
    public partial class CreatePFD : System.Web.UI.Page
    {
        private Document PdfDocument { get; set; }
        private PdfWriter Writer { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cmdMakePDF_Click(object sender, EventArgs e)
        {
            // Instantian HTML to PDF worker
            // Run worker
            // Upbut result as file
            DoPDF();
        }

        public void DoPDF()
        {
            //Our sample HTML and CSS
            var example_html = GetHTML();
                                // @"<p>This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span>" +
                                // @"<strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span></p>";

            var example_css = GetCSS();
                                // @".headline{font-size:200%}";

            Byte[] outputBytes;

            // Create a new PdfWrite object, writing the output to a MemoryStream
            using (var output = new MemoryStream()) {
                using (var PdfDocument = new Document(PageSize.LETTER, 50f, 50f, 0f, 25f)){
                    using (var writer = PdfWriter.GetInstance(PdfDocument, output)) {

                        PdfDocument.Open();
                        using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
                        {
                            using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
                            {
                                //Parse the HTML
                                iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfDocument, msHtml, msCss);
                            }
                        }

                        PdfDocument.Close();
                    }
                }

                outputBytes = output.ToArray();
            }


            string fileName = "testFile.pdf";

            byte[] pdfasBytes = outputBytes;
            //Encoding.ASCII.GetBytes(fileBytes);   // Here the fileBytes are already encoded (Encrypt) value. Just convert from string to byte
            Response.Clear();
            MemoryStream ms = new MemoryStream(pdfasBytes);
            Response.ContentType = "application/pdf";
            Response.Headers.Add("content-disposition", "attachment;filename=" + fileName);
            Response.Buffer = true;
            ms.WriteTo(Response.OutputStream);
            Response.Flush();
            Response.End();
        }


        public string GetHTML()
        {
            if (string.IsNullOrWhiteSpace(txtInputHTML.Text))
            {
                var path = Path.Combine(Request.PhysicalApplicationPath, "App_Data", "Samples", "Sample_Test_HMTL.html");

                StreamReader sr = new StreamReader(path);
                string html = sr.ReadToEnd();
                sr.Close();


                html = html.Replace("<h1>",
                                    "<table style=\" width: 100%; color: red; background: #e8e8ef; padding: 2px; border: 1px solid #27579f; font-size: 1.5em;" +
                                    "margin-top:10px; font-variant: small-caps;" +
                                    "\"><tr><td style=\"top-padding: 0px; bottom-padding: 0px;\"><h1>"

                                    //"<table style=\" width: 100%; color: #fff; font-family: Georgia, " +
                                    //"\"Times New Roman\", Times, serif; font-size: 1.3em; " +
                                    //"background: #15365b; display: block; width: 100%; padding: 3px 0; " +
                                    //"margin-top:10px; font-variant: small-caps;\"><tr><td><h1>"

                                    );

                html = html.Replace("</h1>", "</h1></td></tr></table>");

                html = html.Replace("<h2>",
                                    "<table style=\" width: 100%; color: #fff; background: #15365b; padding: 3px 0; " +
                                    "margin-top:10px; font-variant: small-caps; font-size: 1.3em; margin-bottom: 10px; margin-top: 10px;" +
                                    "margin-top: 10px; font-variant: small-caps;" +
                                    "\"><tr><td style=\"top-padding: 0px; bottom-padding: 0px; top-margin: 15px;\"><h2 style=\"top-margin: 15px;\">"

                                    //"<table style=\" width: 100%; color: #fff; font-family: Georgia, " +
                                    //"\"Times New Roman\", Times, serif; font-size: 1.3em; " +
                                    //"background: #15365b; display: block; width: 100%; padding: 3px 0; " +
                                    //"margin-top:10px; font-variant: small-caps;\"><tr><td><h2>"

                                    );
                html = html.Replace("</h2>", "</h2></td></tr></table>");
                return html;
            }
            else 
                return txtInputHTML.Text.Trim();
        }

        public string GetCSS()
        {
            var path = Path.Combine(Request.PhysicalApplicationPath, "App_Data", "Samples", "Sample_Test_CSS.css");
            StreamReader sr = new StreamReader(path);
            string css = sr.ReadToEnd();

            sr.Close();

            return css;
        }
    }
}


