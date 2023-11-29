using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillProfiBot
{
    public class WorkWithFile
    {
        private long Id;
        string NameFolder => Id.ToString();
        string FilePath => Path.Combine(NameFolder, $"{NameFolder}.txt");
        public DataInFile Data = new DataInFile();

        public WorkWithFile(long id)
        {
            Id = id;
        }

        public async Task<int> GetPosition()
        {
            string json = File.ReadAllText(FilePath).Trim();
            DataInFile data = JsonConvert.DeserializeObject<DataInFile>(json);

            if (data.Date.AddDays(1) < DateTime.Now)
            {
                await SetPosition(1,false);
                return -2;
            }

            return Convert.ToInt32(data.Position);
        }

        public async Task CheckAndCreateData()
        {
            if (!Directory.Exists(NameFolder))
            {
                Directory.CreateDirectory(NameFolder);
                File.Create(FilePath).Close();
                await SetPosition(0, false);
                Console.WriteLine("Папка создана c файлом создана");
                return;
            }

            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Close();
                await SetPosition(0, false);
                Console.WriteLine("Файл создан.");
                return;
            }

            string json = File.ReadAllText(FilePath).Trim();
            Console.WriteLine("Содержимое файла: " + json);
            if (string.IsNullOrEmpty(json))
            {
                await SetPosition(0, false);
                return;
            }
        }

        public async Task SetPosition(int needPosition, bool needSaveOldData)
        {
            string json = string.Empty;
            if (needSaveOldData)
            {
                string jsonFromFile = File.ReadAllText(FilePath).Trim();
                DataInFile oldData = JsonConvert.DeserializeObject<DataInFile>(jsonFromFile);
                oldData.Position = needPosition;
                json = JsonConvert.SerializeObject(oldData);
            }
            else
            {
                json = JsonConvert.SerializeObject(new DataInFile(DateTime.Now, needPosition));
            }
            await File.WriteAllTextAsync(FilePath, json);
        }

        public async Task SetPosition(int needPosition, DataInFile newData)
        {
            string jsonFromFile = File.ReadAllText(FilePath).Trim();
            DataInFile oldData = JsonConvert.DeserializeObject<DataInFile>(jsonFromFile);
            var dataForJson = new DataInFile
            {
                Date = newData.Date,
                Name = newData.Name == string.Empty ? oldData.Name : newData.Name,
                Email = newData.Email == string.Empty ? oldData.Email : newData.Email,
                Text = newData.Text == string.Empty ? oldData.Text : newData.Text,
                Position = needPosition
            };
            string json = JsonConvert.SerializeObject(dataForJson);
            await File.WriteAllTextAsync(FilePath, json);
        }

        public async Task<DataInFile> GetData()
        {
            string jsonFromFile = File.ReadAllText(FilePath).Trim();
            return JsonConvert.DeserializeObject<DataInFile>(jsonFromFile);
        }
    }
}
