using System.IO;

namespace Task2.Cmd.DataManager
{
    public class FileManager
    {
        public string Read(string path)
        {
            var data = "";
            using (var sr = new StreamReader(path))
            {
                data =  sr.ReadToEnd();
            }
            return data;
        }
        public void Write(string data, string path)
        {
            using (var sw = new StreamWriter(path))
            {
                sw.Write(data);
            }
        }
        //public string Read(string name)
        //{
        //    return File.ReadAllText($"{Environment.CurrentDirectory}\\Data\\{name}.json");
        //}
        //public void Write(string text, string name)
        //{
        //    File.WriteAllText($"{Environment.CurrentDirectory}\\Data\\{name}.json", text);
        //}
    }
}
