using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

public class PdfController : ApiController
{
    private static readonly string pdfStoragePath = @"C:\inetpub\wwwroot\pdfs\"; // IIS Public Folder

    [HttpPost]
    [Route("api/pdf/upload")]
    public async Task<IHttpActionResult> UploadPdf()
    {
        if (!Request.Content.IsMimeMultipartContent())
            return BadRequest("Invalid Request");

        var provider = new MultipartMemoryStreamProvider();
        await Request.Content.ReadAsMultipartAsync(provider);

        foreach (var file in provider.Contents)
        {
            var fileName = file.Headers.ContentDisposition.FileName.Trim('"');
            var filePath = Path.Combine(pdfStoragePath, fileName);

            byte[] fileBytes = await file.ReadAsByteArrayAsync();
            File.WriteAllBytes(filePath, fileBytes);
        }

        return Ok("File uploaded successfully!");
    }

    [HttpGet]
    [Route("api/pdf/{fileName}")]
    public HttpResponseMessage GetPdf(string fileName)
    {
        string filePath = Path.Combine(pdfStoragePath, fileName);

        if (!File.Exists(filePath))
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("File not found")
            };
        }

        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(File.ReadAllBytes(filePath))
        };
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
        {
            FileName = fileName
        };
        return response;
    }
}
