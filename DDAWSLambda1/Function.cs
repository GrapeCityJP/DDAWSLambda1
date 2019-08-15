using Amazon.Lambda.Core;
using System;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace DDAWSLambda1
{
    public class Function
    {
        public string FunctionHandler(string input, ILambdaContext context)
        {
            var ddexcel = new DDExcel();
            var ddpdf = new DDPdf();

            try
            {
                ddexcel.Create(input);

                ddpdf.Create(input);

                return "OK";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }
}
