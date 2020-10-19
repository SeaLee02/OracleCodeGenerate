using RazorEngine;
using RazorEngine.Compilation.ImpromptuInterface;
using RazorEngine.Templating;
using System;

namespace OracleDemo
{
    class Program
    {

        const string connstr = "Data Source=localhost/orcl;User ID=LISAN;Password=1234;";


        static void Main(string[] args)
        {
            //根据demo.txt模板生成
            string templatePath = "";
            string templateKey = Guid.NewGuid().ToString();
            var model = new
            {
                aa = "dwdw",
                bb = "ddd"
            };
            string template = System.IO.File.ReadAllText(templatePath); //从文件中读出模板内容
            var result = Engine.Razor.RunCompile(template, templateKey, null, model);
            string savePath = "保存路径";
            FileHelper.CreateFile(savePath, result, System.Text.Encoding.UTF8);


            Console.WriteLine("DDD");
            Console.ReadKey();
        }
    }
}
 