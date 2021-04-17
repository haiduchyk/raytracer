namespace ConsoleApplication
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    public class InputProcessor
    {
        public readonly KeyWord Source = new KeyWord {Key = "--source"};
        public readonly KeyWord Goal = new KeyWord {Key = "--goal-format"};
        public readonly KeyWord OutputPath = new KeyWord {Key = "--output"};
        public string SourceExtension => Source.Value.Split('.').Last(); 
        public string OutputExtension => OutputPath.Value.Split('.').Last();
        
        private readonly List<KeyWord> keyWords;
        
        public InputProcessor()
        {
            keyWords = new List<KeyWord> {Source, Goal, OutputPath};
        }
        public void SetupKeyWords(List<string> args)
        {
            foreach (var keyWord in keyWords)
            {
                var rgx = new Regex($@"(?<={keyWord.Key}=).+");
                var arg = args.FirstOrDefault(a => rgx.IsMatch(a));
                if (arg != null) keyWord.Value = rgx.Match(arg).ToString();
            }
        }
    }
}