using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudinary
{
    public class ImageFile
    {
        /// <summary>
        /// The public id used by Cloudinary
        /// </summary>
        public string PublicId { get; set; }
        /// <summary>
        /// File upload version
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// The width of the uploaded image
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// The height of the uploaded image
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Image format: JPEG, JPG, PNG
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// The size of the uploaded image in bytes
        /// </summary>
        public int ImageSize { get; set; }
        /// <summary>
        /// URL to access the image [HTTP]
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// URL to access the image [HTTPs]
        /// </summary>
        public string SecureUrl { get; set; }
				public DateTime CreatedAt { get; set; }
		}
}
