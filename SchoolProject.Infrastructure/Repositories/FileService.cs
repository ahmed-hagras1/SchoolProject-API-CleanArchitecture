using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Implementations
{
    public class FileService : IFileService
    {
        #region Fields
        private readonly IWebHostEnvironment _webHostEnvironment;
        #endregion

        #region Constructor
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            
        }
        #endregion

        #region Methods / Handle functions
        public async Task<string> UploadImage(string location, IFormFile file)
        {
            // 2. التحقق من أن الملف ليس فارغاً
            if (file == null || file.Length == 0)
            {
                return null; // أو يمكنك رمي Exception إذا كان رفع الصورة إجبارياً
            }

            // 3. الحصول على المسار الرئيسي لمجلد wwwroot
            var webRootPath = _webHostEnvironment.WebRootPath;

            // 4. تحديد مسار المجلد الذي سيتم حفظ الصورة فيه (مثل: wwwroot/Images/Instructors)
            var uploadPath = Path.Combine(webRootPath, location);

            // 5. إنشاء المجلد إذا لم يكن موجوداً بالفعل
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // 6. إنشاء اسم فريد للملف لتجنب استبدال ملفات بنفس الاسم
            var extension = Path.GetExtension(file.FileName); // الحصول على امتداد الملف (مثل .jpg)
            var fileName = Guid.NewGuid().ToString().Replace("-", "") + extension; // اسم عشوائي جديد

            // 7. دمج مسار المجلد مع اسم الملف الجديد
            var fullPhysicalPath = Path.Combine(uploadPath, fileName);

            // 8. حفظ الملف فعلياً على الخادم
            using (var fileStream = new FileStream(fullPhysicalPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // 9. إرجاع المسار النسبي لحفظه في قاعدة البيانات (مثل: /Images/Instructors/12345.jpg)
            return $"/{location}/{fileName}";
        }
        #endregion
    }
}
