using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Windows.Controls;
using Image = iTextSharp.text.Image;
using Rectangle = iTextSharp.text.Rectangle;

namespace ConvertToPdfs
{
    public class PDFWrapper
    {
        public static string CreatePDF(ItemCollection files, string fileName)
        {
            try
            {
                var document = new Document(PageSize.LETTER);

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));

                // step 3: we open the document
                document.Open();

                foreach (var file in files)
                {
                    var pic = Image.GetInstance(file.ToString());

                    if (pic.Height > pic.Width)
                    {
                        //Maximum height is 800 pixels.
                        float percentage = 0.0f;
                        percentage = 700 / pic.Height;
                        pic.ScalePercent(percentage * 100);
                    }
                    else
                    {
                        //Maximum width is 600 pixels.
                        float percentage = 0.0f;
                        percentage = 540 / pic.Width;
                        pic.ScalePercent(percentage * 100);
                    }

                    pic.Border = Rectangle.BOX;
                    pic.BorderColor = BaseColor.BLACK;
                    pic.BorderWidth = 3f;
                    document.Add(pic);
                    document.NewPage();
                }

                document.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Success";
        }
    }
}
