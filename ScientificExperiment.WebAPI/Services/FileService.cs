using DAL;
using Microsoft.EntityFrameworkCore;

namespace WebApi_CSV.Services
{
    public class FileService
    {
        private readonly DataContext _context;
        public FileService(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Возвращает Id файла в базе данных по имени файла.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Id файла.</returns>
        public async Task<int?> GetFileId(string fileName) =>
                   await _context.Files
                   .Where(file => file.FileName == fileName)
                   .Select(file => (int?)file.Id)
                   .FirstOrDefaultAsync();
        /// <summary>
        /// Возвращает имя файла в базе данных по Id.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>Имя файла</returns>
        public async Task<string?> GetFileName(int fileId) =>
                   await _context.Files
                   .Where(file => file.Id == fileId)
                   .Select(file => (string?)file.FileName)
                   .FirstOrDefaultAsync();
    }
}
