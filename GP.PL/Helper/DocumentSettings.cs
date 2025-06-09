using Microsoft.CodeAnalysis;

namespace GP.PL.Helper
{
    public class DocumentSettings
    {
        public static string UpdloadFile(IFormFile file, string folderName)
        {
            //GET LOCATED FOLDER
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\", folderName);
            //GET FILE NAME AND MAKE IT UNIQE
            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            //get file path
            string filePath = Path.Combine(folderPath, fileName);
            //save file as streams (data per time)
            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);
            return fileName;

        }

        public static void DeleteFile(string fileName, string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
