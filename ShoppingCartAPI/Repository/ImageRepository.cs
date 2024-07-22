using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.Data;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<ProductImage>> GetAll()
        {
            return await dbContext.ProductImages.ToListAsync();
        }

        public async Task<ProductImage> Upload(IFormFile file, ProductImage productImage)
        {
            // 1- Upload the Image to API/Images
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{productImage.FileName}{productImage.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            // 2-Update the database
            // https://codepulse.com/images/somefilename.jpg
            var httpRequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{productImage.FileName}{productImage.FileExtension}";

            productImage.Url = urlPath;

            await dbContext.ProductImages.AddAsync(productImage);
            await dbContext.SaveChangesAsync();

            return productImage;
        }
    }
}