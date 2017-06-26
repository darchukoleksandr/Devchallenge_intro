using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DAL;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Main controller for uploading and parsing PDF files. 
    /// </summary>
    public class DocumentsController : ApiController
    {
        // POST: api/documents
        /// <summary>
        /// Uploads and parsing PDF files (multipart/form-data)
        /// </summary>
        /// <returns>Redirect to upload PDF view if upload is success or specific error</returns>
        public async Task<HttpResponseMessage> Post()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            //string root = @"C:\PDF\";
            string root = HttpContext.Current.Server.MapPath("~/App_Data/PDF");
            var provider = new MultipartFormDataStreamProvider(root);
            
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (var file in provider.FileData)
                {
                    DocumentSaverService.Instance.AddDocument(file.LocalFileName);
                }
                
                var result = Request.CreateResponse(HttpStatusCode.Redirect);
                result.Headers.Location = new Uri("http://localhost:50001/Home/UploadPdf");
                return result;
            }
            catch (ArgumentException e)
            {
                foreach (var file in provider.FileData)
                {
                    File.Delete(file.LocalFileName);
                }
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            catch (IOException e)
            {
                if (e.InnerException is HttpException && ((HttpException)e.InnerException).WebEventCode == 3004)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception: e, message: "Request exceeded maximum allowed length. Limit is 4 MB.");
                }
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
