using IC.Domain.Entities;
using System;
using System.IO;
using System.Linq;

namespace IC.CrossCutting
{
    /// <summary>
    /// Classe estática responsável por armazenar, remover e atualizar arquivos.
    /// </summary>
    public static class FileService
    {
        /// <summary>
        /// Método responsável por armazenar um arquivo
        /// </summary>
        /// <param name="code"></param>
        /// <param name="photo"></param>
        /// <returns></returns>
        public static string CreateFile(string code, Photo photo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code) || photo == null)
                    throw new Exception("Incomplete data, check and try again.");

                var extension = photo.Name.Split('.').LastOrDefault();

                if (string.IsNullOrWhiteSpace(extension))
                    throw new Exception("File has no extension.");

                int count = 1;
                string complement = string.Empty;

                // Laço para garantir que caso o arquivo exista, não sobrescrever
                while (File.Exists($"{code}{complement}.{extension}"))
                {
                    complement = $"{count}";
                    count++;
                }

                string nameFile = $"{code}{complement}.{extension}";

                File.WriteAllBytes(nameFile, Convert.FromBase64String(photo.PhotoBytes));

                return nameFile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método responsável por remover um arquivo
        /// </summary>
        /// <param name="pathFile"></param>
        public static void DeleteFile(string pathFile)
        {
            try
            {
                if (File.Exists(pathFile))
                    File.Delete(pathFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método responsável por Atualizar um arquivo
        /// Para evitar que perca a imagem anterior por uma atualização errada, o arquivo de imagem é gerado antes da remoção da anterior.
        /// </summary>
        /// <param name="oldFile"></param>
        /// <param name="code"></param>
        /// <param name="photo"></param>
        /// <returns></returns>
        public static string UpdateFile(string oldFile, string code, Photo photo)
        {
            try
            {
                string newFile = string.Empty;
                if (!string.IsNullOrEmpty(photo.Name))
                    newFile = CreateFile(code, photo);
                DeleteFile(oldFile);

                return newFile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
