namespace NayeemApplication.Helpers
{
    public static class FileSave
    {

        public static string SaveImage(out string fileName, IFormFile img)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png"};
            string message = "success";

            var extention = Path.GetExtension(img.FileName);
            if (img.Length > 2000000)
                message = "Select jpg or jpeg or png less than 2Μ";
            else if (!allowedExtensions.Contains(extention.ToLower()))
                message = "Must be jpeg or png";

            fileName =DateTime.Now.Ticks + extention;
            string filePath= Path.Combine("UsersPhoto", DateTime.Now.Ticks + extention); ;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath);
            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    img.CopyTo(stream);
                }
            }
            catch
            {
                message = "can not upload image";
            }
            return message;
        } 
        

        public static string SaveCV(out string fileName, IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg",".pdf",".txt",".docx" };
            string message = "success";

            var extention = Path.GetExtension(file.FileName);
            if (file.Length > 2000000)
                message = "Select jpg or jpeg or png or pdf less than 2Μ";
            else if (!allowedExtensions.Contains(extention.ToLower()))
                message = "Must be docx,txt or pdf";

            fileName =DateTime.Now.Ticks + extention;
            string filePath = Path.Combine("UsersCV", DateTime.Now.Ticks + extention);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath);
            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            catch
            {
                message = "can not upload image";
            }
            return message;
        }

      
    }
}
