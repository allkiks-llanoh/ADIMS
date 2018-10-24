using ADIMS.APIs.MimeTypeConfig;
using ADIMS.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace ADIMS.APIs
{
    [RoutePrefix("api/photos")]
    public class PhotosController : ApiController
    {
        private readonly adimsEntities db = new adimsEntities();

        [HttpPost]
        [Route("")]
        [MimeMultipart]
        [ResponseType(typeof(int))]
        public async Task<IHttpActionResult> Upload()
        {
            var uploadPath = HttpContext.Current.Server.MapPath("~/API_Uploads");

            var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);

            // Read the MIME multipart asynchronously 
            await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);

            string _localFileName = multipartFormDataStreamProvider
                .FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();

          
            var buffer = File.ReadAllBytes(_localFileName);

            var image = WriteImage(buffer);
            return Ok(image);
            
        }

        private int WriteImage(byte[] arr)
        {
            var filename = $@"\documents\photos\{DateTime.Now.Ticks}.";

            using (var im = Image.FromStream(new MemoryStream(arr)))
            {
                ImageFormat frmt;
                if (ImageFormat.Png.Equals(im.RawFormat))
                {
                    filename += "png";
                    frmt = ImageFormat.Png;
                }
                else
                {
                    filename += "jpg";
                    frmt = ImageFormat.Jpeg;
                }
                string path = HttpContext.Current.Server.MapPath("~/") + filename;
                im.Save(path, frmt);
            }
            string url = $@"http:\\{Request.RequestUri.Host}\{filename}";

            photo _photo = new photo()
            {
                url = url,
                datecreated = DateTime.Now,
                createdby = 1
            };

            db.photos.Add(_photo);
            db.SaveChanges();

            return _photo.id;
        }
    }

    public class ImageModel
    {
        public string Name { get; set; }
        public byte[] Bytes { get; set; }
    }
}
