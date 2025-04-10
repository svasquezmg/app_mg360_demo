using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using iText.Html2pdf;
using iText.IO.Source;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using System.Text;
using System.Threading.Tasks;
using iText.Html2pdf.Css.Apply.Impl;
using MySqlX.XDevAPI;
using iText.Layout;
using System.Net.Http;



namespace WebAppMontGroup
{
    public class PdfService
    {
        public byte[] GeneratePdfFromHtml(string htmlContent)
        {
            using (var memoryStream = new MemoryStream())
            {
                HtmlConverter.ConvertToPdf(htmlContent, memoryStream);
                return memoryStream.ToArray();
            }

        }


        public byte[] GeneratePdfHtml(string html)
        {

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(html));
            ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
            PdfWriter writer = new PdfWriter(byteArrayOutputStream);
            PdfDocument pdfDocument = new PdfDocument(writer);
          
            pdfDocument.SetDefaultPageSize(PageSize.A4);
            /*var document = new Document(pdfDocument);
            document.SetMargins(0, 0, 0, 0);

            var converterProperties = new ConverterProperties();*/
            HtmlConverter.ConvertToPdf(stream, pdfDocument); /*, converterProperties);*/
            pdfDocument.Close();

            return byteArrayOutputStream.ToArray();
        }

    }
}