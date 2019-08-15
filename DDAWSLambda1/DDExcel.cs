using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using GrapeCity.Documents.Excel;
using System.IO;

namespace DDAWSLambda1
{
    class DDExcel
    {
        private const string bucketName = "diodocs-storage";
        private const string uploadExcelName = "Result.xlsx";

        public void Create(string input)
        {
            //// トライアル用
            //string key = Environment.GetEnvironmentVariable("DdExcelLicenseString");
            //Workbook.SetLicenseKey(key);

            // ワークブックの作成
            Workbook workbook = new Workbook();

            // ワークシートの取得
            IWorksheet worksheet = workbook.Worksheets[0];

            // セル範囲を指定して文字列を設定
            worksheet.Range["B2"].Value = "Hello DioDocs!";
            worksheet.Range["B3"].Value = "from " + input;

            // メモリストリームに保存
            MemoryStream ms = new MemoryStream();
            workbook.Save(ms, SaveFileFormat.Xlsx);
            ms.Seek(0, SeekOrigin.Begin);

            // S3にアップロード
            AmazonS3Client client = new AmazonS3Client(RegionEndpoint.APNortheast1);

            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = uploadExcelName,
                InputStream = ms
            };

            client.PutObjectAsync(request);
        }
    }
}
