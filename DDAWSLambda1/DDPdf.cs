using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using GrapeCity.Documents.Pdf;
using GrapeCity.Documents.Text;
using System;
using System.Drawing;
using System.IO;

namespace DDAWSLambda1
{
    class DDPdf
    {
        private const string bucketName = "diodocs-storage";
        private const string uploadPdfName = "Result.pdf";

        public void Create(string input)
        {
            //// トライアル用
            //string key = Environment.GetEnvironmentVariable("DdPdfLicenseString");
            //GcPdfDocument.SetLicenseKey(key);

            // PDFドキュメントを作成します。
            GcPdfDocument doc = new GcPdfDocument();

            // ページを追加し、そのグラフィックスを取得します。
            GcPdfGraphics g = doc.NewPage().Graphics;

            // ページに文字列を描画します。
            g.DrawString("Hello DioDocs!" + Environment.NewLine + "from " + input,
                new TextFormat() { Font = StandardFonts.Helvetica, FontSize = 12 },
                new PointF(72, 72));

            // メモリストリームに保存
            MemoryStream ms = new MemoryStream();
            doc.Save(ms, false);
            ms.Seek(0, SeekOrigin.Begin);

            // S3にアップロード
            AmazonS3Client client = new AmazonS3Client(RegionEndpoint.APNortheast1);

            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = uploadPdfName,
                InputStream = ms
            };

            client.PutObjectAsync(request);
        }
    }
}
